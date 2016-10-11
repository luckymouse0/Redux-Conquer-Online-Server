using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Game_Server;
using Redux.Managers;
using Redux.Structures;
using Redux.Enum;

namespace Redux.Managers
{
    public static class GuildWar
    {
        public static Guild CurrentWinner;
        public static bool Running = false;
        public static bool StartNewRound = false;
        public static long NewRoundTime = Common.Clock;
        public static long LastUpdate = Common.Clock;
        public static uint LeftGateHp =  10000000;
        public static uint RightGateHp = 10000000;
        public static Dictionary<Structures.Guild, ulong> GuildScores = new Dictionary<Structures.Guild, ulong>();
       
        public static void RoundEnd()
        {
            foreach (var role in PlayerManager.Players)
            {
                if (role.Value.MapID == 1038)
                    role.Value.CombatManager.AbortAttack(true);
            }
            var sorted = from I in GuildScores orderby I.Value descending select I;
            var winner = sorted.First();
            PlayerManager.SendToServer(new Packets.Game.TalkPacket(Enum.ChatType.GM, winner.Key.Name + " has emerged victorious with a score of: " + winner.Value + " Congratulations for their valiant efforts"));
            
            CurrentWinner = winner.Key;
            
            var toUpdate = MapManager.PullMapByID(1038);
            if (toUpdate != null)
            {
                var sob = toUpdate.Search<SOB>(6700);//.Maps[1038].Search<SOB>(6700);//pole
                sob.Name = CurrentWinner.Name;
                sob.SendToScreen(Packets.Game.SpawnSob.Create(sob), true);
            }

            var dbsob = Database.ServerDatabase.Context.SOB.GetByUID(6700);

            dbsob.Name = winner.Key.Name;
            Database.ServerDatabase.Context.SOB.Update(dbsob);
            StartNewRound = true;
            NewRoundTime = Common.Clock + (Common.MS_PER_SECOND * 10);
            
            //Database save?           
        }

        public static void GuildWarEnd()
        {
            PlayerManager.SendToServer(new Packets.Game.TalkPacket(ChatType.GM, CurrentWinner + " has won! Congratulations to everyone for their efforts."));
            Running = false;
            
            SOB leftGate = MapManager.PullMapByID(1038).Search<SOB>(6701);
            
                
                leftGate.Life = 10000000;
                leftGate.Mesh = 241;
                leftGate.SendToScreen(Packets.Game.SpawnSob.Create(leftGate), true);

                SOB rightGate = MapManager.PullMapByID(1038).Search<SOB>(6702);
            
                rightGate.Life = 10000000;
                rightGate.Mesh = 241;
                rightGate.SendToScreen(Packets.Game.SpawnSob.Create(rightGate), true);

                var toUpdate = MapManager.PullMapByID(1038).Search<SOB>(6700);//pole
            if (toUpdate != null)
            {
                if (CurrentWinner != null)
                    toUpdate.Name = CurrentWinner.Name;
                toUpdate.Life = 20000000;
                toUpdate.SendToScreen(Packets.Game.SpawnSob.Create(toUpdate), true);
            }

           
            GuildScores.Clear();
            
        }
        public static void StartRound()
        {
            StartNewRound = false;
            LeftGateHp = 10000000;
            RightGateHp = 10000000;

            var toUpdate = MapManager.PullMapByID(1038).Search<SOB>(6700);//pole
            if (toUpdate != null)
            {
                if(CurrentWinner != null)
                    toUpdate.Name = CurrentWinner.Name;
                toUpdate.Life = 20000000;
                toUpdate.SendToScreen(Packets.Game.SpawnSob.Create(toUpdate), true);
            }

            toUpdate = MapManager.PullMapByID(1038).Search<SOB>(6701);//LeftGate
            if (toUpdate != null)
            {
                toUpdate.Mesh = 241;
                toUpdate.Life = 10000000;
                toUpdate.SendToScreen(Packets.Game.SpawnSob.Create(toUpdate), true);
            }

            toUpdate = MapManager.PullMapByID(1038).Search<SOB>(6702); //RightGate
            if (toUpdate != null)
            {
                toUpdate.Mesh = 271;
                toUpdate.Life = 10000000;
                toUpdate.SendToScreen(Packets.Game.SpawnSob.Create(toUpdate), true);
            }

            GuildScores.Clear();
            
                
        }
        public static void UpdateScoreTable()
        {
            try
            {
                if (GuildScores.Count > 0)
                {
                    var clear = new Packets.Game.TalkPacket(ChatType.SynWarFirst, "");
                    var ts = from I in GuildScores orderby I.Value descending select I;
                    int loop = 0;
                    foreach (Player role in PlayerManager.Select(u => u != null && u.Map.ID == 1038))
                    {
                        role.Send(new Packets.Game.TalkPacket(ChatType.SynWarFirst, ""));
                    }
                    foreach (KeyValuePair<Structures.Guild, ulong> score in ts)
                    {
                        if (loop > 5)
                            return;
                        loop++;
                        foreach (Player role in PlayerManager.Players.Values)
                            if (role.Map.ID == 1038)
                                role.Send(new Packets.Game.TalkPacket(ChatType.SynWarNext, "N° " + loop + ": " + score.Key.Name + ": " + score.Value));
                    }  
                }
            }
            catch (Exception P) { Console.WriteLine(P); }
            LastUpdate = Common.Clock;
            
        }
        public static void KillLeftGate()
        {
            try
            {
                SOB leftGate = MapManager.PullMapByID(1038).Search<SOB>(6701);
                
                    //leftGate.MaxHP = 1;
                    LeftGateHp = 1;// leftGate.Health;
                    leftGate.Mesh = 251;
                    leftGate.Life = 1;
                    leftGate.SendToScreen(Packets.Game.SpawnSob.Create(leftGate), true);
                
               
            }
            catch (Exception P) { Console.WriteLine(P);  }
        }
        public static void KillRightGate()
        {
            try
            {
                SOB rightGate = MapManager.PullMapByID(1038).Search<SOB>(6702);

                //rightGate.MaxHP = 1;
                RightGateHp = 1;// leftGate.Health;
                rightGate.Mesh = 287;
                rightGate.Life = 1;
                rightGate.SendToScreen(Packets.Game.SpawnSob.Create(rightGate), true);


            }
            catch (Exception P) { Console.WriteLine(P); }
        }
        public static bool ToggleLeftGate()
        {
            try
            {
                SOB leftGate = MapManager.PullMapByID(1038).Search<SOB>(6701);
                if (leftGate.Mesh == 241)
                {
                    //leftGate.MaximumLife = 0;
                    LeftGateHp = leftGate.Life;
                    leftGate.Mesh = 251;
                    //leftGate.Life = 0;
                    leftGate.SendToScreen(Packets.Game.SpawnSob.Create(leftGate), true);
                }
                else
                {
                    leftGate.MaximumLife = 10000000;
                    leftGate.Life = LeftGateHp;
                    leftGate.Mesh = 241;
                    leftGate.SendToScreen(Packets.Game.SpawnSob.Create(leftGate), true);
                }
            }
            catch (Exception P) { Console.WriteLine(P); return false; }
            return true;

        }

        public static void RepairLeftGate()
        {
            SOB leftGate = MapManager.PullMapByID(1038).Search<SOB>(6701);
            if (leftGate.Life == 1 && GuildWar.CurrentWinner.Money >= 2000000)
            {
                leftGate.MaximumLife = 10000000;
                leftGate.Life = 10000000;
                leftGate.Mesh = 241;
                leftGate.SendToScreen(Packets.Game.SpawnSob.Create(leftGate), true);

                GuildWar.CurrentWinner.Money -= 2000000;
            }
            

        }

        public static void RepairRightGate()
        {
            SOB rightGate = MapManager.PullMapByID(1038).Search<SOB>(6702);
            if (rightGate.Life == 1 && GuildWar.CurrentWinner.Money >= 2000000)
            {
                rightGate.MaximumLife = 10000000;
                rightGate.Life = 10000000;
                rightGate.Mesh = 241;
                rightGate.SendToScreen(Packets.Game.SpawnSob.Create(rightGate), true);

                GuildWar.CurrentWinner.Money -= 2000000;
            }


        }
        public static bool ToggleRightGate()
        {
            try
            {
                SOB rightgate = MapManager.PullMapByID(1038).Search<SOB>(6702);
                if (rightgate.Mesh == 271)
                {
                    
                    RightGateHp = rightgate.Life;
                    rightgate.Mesh = 287;
                    //rightgate.Life = 0;
                    rightgate.SendToScreen(Packets.Game.SpawnSob.Create(rightgate), true);
                }
                else
                {
                    rightgate.MaximumLife = 10000000;
                    rightgate.Life = RightGateHp;
                    rightgate.Mesh = 271;
                    rightgate.SendToScreen(Packets.Game.SpawnSob.Create(rightgate), true);
                }
            }
            catch (Exception P) { Console.WriteLine(P); return false; }
            return true;
        }

        public static void GuildWar_Tick()
        {
            //If GW is running and 10 seconds since last update then update scores
            if (GuildWar.Running == true && Common.Clock - GuildWar.LastUpdate > Common.MS_PER_SECOND * 10)
            {
                GuildWar.UpdateScoreTable();
            }

            //If new round is starting
            if (GuildWar.Running == true && GuildWar.StartNewRound == true && Common.Clock > GuildWar.NewRoundTime)
            {
                GuildWar.StartRound();
            }
        }
    }
}
