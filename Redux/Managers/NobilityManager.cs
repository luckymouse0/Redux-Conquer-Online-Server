using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Managers
{
    public static class NobilityManager
    {
        /*public ConcurrentDictionary<uint, Associate> Friends;
        public ConcurrentDictionary<uint, Associate> Enemies;
        public ConcurrentDictionary<uint, Associate> EnemyOf;*/

        public static int PageCount, ListCount;
        public static Packets.NetStringPacker Page1 = new Packets.NetStringPacker();
        public static Packets.NetStringPacker Page2 = new Packets.NetStringPacker();
        public static Packets.NetStringPacker Page3 = new Packets.NetStringPacker();
        public static Packets.NetStringPacker Page4 = new Packets.NetStringPacker();
        public static Packets.NetStringPacker Page5 = new Packets.NetStringPacker();
        public static IList<DbNobility> AllRanks;
        public static List<uint> RankingList = new List<uint>();

        public static Packets.NetStringPacker GetPage(int PageNo)
        {
            switch (PageNo)
            {
                case 0:
                    return Page1;
                case 1:
                    return Page2;
                case 2:
                    return Page3;
                case 3:
                    return Page4;
                case 4:
                    return Page5;
                default:
                    return Page5;
            }
        }

        public static void UpdatePlayers()
        {
            Nobility packet;
            foreach (var player in PlayerManager.Players)
            {
                packet = Nobility.UpdateIcon(player.Value);
                player.Value.Send(packet);
            }
        }
        public static void UpdateNobility()
        {
            AllRanks = ServerDatabase.Context.Nobility.NobilityPages();

            PageCount = (int)Math.Ceiling((double)AllRanks.Count / 10);// +1;

            if (AllRanks.Count == 0 || AllRanks == null)
                return;

            RankingList.Clear();

            if (PageCount >= 1)
            {
                Page1 = new Packets.NetStringPacker();
                if (AllRanks.Count >= 10)
                    ListCount = 10;
                else
                    ListCount = AllRanks.Count;

                //UID 0 0 Name Donation Medal Rank
                for (int i = 0; i < ListCount; i++)
                {
                    RankingList.Add(AllRanks[i].UID);

                    var player = ServerDatabase.Context.Characters.GetByUID(AllRanks[i].UID);

                    if (i >= 0 || i <= 2)
                        Page1.AddString(AllRanks[i].UID + " 1 " + player.Lookface + player.Name + " " + AllRanks[i].Donation + " " + NobilityMedal(i, AllRanks[i].Donation) + " " + i);
                    else
                        Page1.AddString(AllRanks[i].UID + " 0 0 " + player.Name + " " + AllRanks[i].Donation + " " + NobilityMedal(i, AllRanks[i].Donation) + " " + i);

                }
            }

            if (PageCount >= 2)
            {
                Page2 = new Packets.NetStringPacker();
                if (AllRanks.Count >= 20)
                    ListCount = 10;
                else
                    ListCount = AllRanks.Count - 10;

                for (int i = 0; i < ListCount; i++)
                {
                    Page2.AddString(AllRanks[i + 10].UID + " 0 0 " + ServerDatabase.Context.Characters.GetByUID(AllRanks[i + 10].UID).Name + " " + AllRanks[i + 10].Donation + " " + NobilityMedal(i + 10, AllRanks[i + 10].Donation) + " " + i + 10);
                    RankingList.Add(AllRanks[i].UID);
                }
            }

            if (PageCount >= 3)
            {
                Page3 = new Packets.NetStringPacker();

                if (AllRanks.Count >= 30)
                    ListCount = 10;
                else
                    ListCount = AllRanks.Count - 20;

                for (int i = 0; i < ListCount; i++)
                {
                    Page3.AddString(AllRanks[i + 20].UID + " 0 0 " + ServerDatabase.Context.Characters.GetByUID(AllRanks[i + 20].UID).Name + " " + AllRanks[i + 20].Donation + " " + NobilityMedal(i + 20, AllRanks[i + 20].Donation) + " " + i + 20);
                    RankingList.Add(AllRanks[i].UID);
                }
            }

            if (PageCount >= 4)
            {
                Page4 = new Packets.NetStringPacker();

                if (AllRanks.Count >= 40)
                    ListCount = 10;
                else
                    ListCount = AllRanks.Count - 30;

                for (int i = 0; i < ListCount; i++)
                {
                    Page4.AddString(AllRanks[i + 30].UID + " 0 0 " + ServerDatabase.Context.Characters.GetByUID(AllRanks[i + 30].UID).Name + " " + AllRanks[i + 30].Donation + " " + NobilityMedal(i + 30, AllRanks[i + 30].Donation) + " " + i + 30);
                    RankingList.Add(AllRanks[i].UID);
                }
            }

            if (PageCount >= 5)
            {
                Page5 = new Packets.NetStringPacker();
                if (AllRanks.Count >= 50)
                    ListCount = 10;
                else
                    ListCount = AllRanks.Count - 40;

                for (int i = 0; i < ListCount; i++)
                {
                    Page5.AddString(AllRanks[i + 40].UID + " 0 0 " + ServerDatabase.Context.Characters.GetByUID(AllRanks[i + 40].UID).Name + " " + AllRanks[i + 40].Donation + " " + NobilityMedal(i + 40, AllRanks[i + 40].Donation) + " " + i + 40);
                    RankingList.Add(AllRanks[i].UID);
                }

            }
        }

        public static int NobilityMedal(int Rank, long Donation)
        {

            if (Rank >= 0 && Rank <= 2)
                return (int)Enum.NobilityRank.King;
            else if (Rank >= 3 && Rank <= 14)
                return (int)Enum.NobilityRank.Prince;
            else if (Rank >= 15 && Rank <= 49)
                return (int)Enum.NobilityRank.Duke;
            else if (Donation >= 200000000)
                return (int)Enum.NobilityRank.Earl;
            else if (Donation >= 100000000)
                return (int)Enum.NobilityRank.Baron;
            else if (Donation >= 30000000)
                return (int)Enum.NobilityRank.Knight;
            else
                return (int)Enum.NobilityRank.Serf;
        }
    }
}