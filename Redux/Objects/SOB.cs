using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Managers;
using Redux.Packets;
using Redux.Enum;
using Redux.Database.Domain;

namespace Redux.Game_Server
{
    public class SOB : Entity
    {
        public new String Name = "None";
        public ushort Mesh, Flag;
        private uint _maxhp;
        private byte _lvl = 0;

        public SOB (DbSob sob)
        {
            UID = sob.UID;
            Mesh = sob.Mesh;
            Flag = sob.Flag;
            MaximumLife = sob.MaxHp;
            Life = MaximumLife;
            X = sob.X;
            Y = sob.Y;
            Name = sob.Name;
            Level = sob.Level;

        }

        public override void SetDisguise(Database.Domain.DbMonstertype _mob, long _duration) { }

        public bool SendSpawn(Player to)
        {
            try
            {
                to.Send(Packets.Game.SpawnSob.Create(this));
            }
            catch { return false; }
            return true;
        }


        public new uint MaximumLife
        {
            get { return _maxhp; }
            set { _maxhp = value; }
        }
        public override void Kill(uint _dieType, uint _attacker)
        {
            if (UID > 100000)
            {
                Life = MaximumLife;
                SendToScreen(Packets.Game.UpdatePacket.Create(UID, Enum.UpdateType.Life, Life), true);
            }
            else
                Life = 1;

            switch (UID)
            {
                case 6700://pole
                    GuildWar.RoundEnd();
                    break;
                case 6701://left gate
                    GuildWar.KillLeftGate();
                    break;
                case 6702://right gate
                    GuildWar.KillRightGate();
                    break;
            }





            SendToScreen(Packets.Game.InteractPacket.Create(_attacker, UID, X, Y, Enum.InteractAction.Kill, _dieType), true);
        }

        public override void ReceiveDamage(uint _dmg, uint _attacker)
        {
            if (Alive)
            {

                if (Life > _dmg)
                {
                    Life -= (ushort)_dmg;
                }
                else
                {
                    Life = 0;
                }
                if (PlayerManager.Players.ContainsKey(_attacker))
                {
                    var role = PlayerManager.Players[_attacker];
                    if (UID == 6700 && role.Guild != null)
                    {
                        //Guild Scores
                        if (!GuildWar.GuildScores.ContainsKey(role.Guild))
                            GuildWar.GuildScores.Add(role.Guild, 0);
                        GuildWar.GuildScores[role.Guild] += _dmg / 3;

                        //Money from pole
                        if (role.Guild != null)
                            role.Guild.Money += _dmg / 500;
                        role.Money += _dmg / 250;
                        if (GuildWar.CurrentWinner != null)
                            GuildWar.CurrentWinner.Money -= _dmg / 500;
                    }
                }
            }
        }
        public void DealKillDmg(Entity owner, Packets.Game.InteractPacket packet)
        {
            owner.SendToScreen(packet, true);
        }
        #region Junk from Entity
        public override void SendToScreen(byte[] data, bool self)
        {
            try
            {
                if (Map == null)
                    return;
                var toSend = from I in Map.QueryScreen(this)
                             where I is Player
                             select I;

                if (toSend.Count() < 0)
                    return;
                foreach (ILocatableObject i in toSend.ToArray())
                {
                    Player role = i as Player;
                    if (role != null)
                    {
                        role.Send(data.UnsafeClone());
                    }
                }
            }
            catch (Exception P) { Console.WriteLine(P); }
        }
        public override void Send(byte[] data)
        {

        }

        public override byte Stamina
        {
            get
            {
                return 0;
            }
            set
            { }
        }
        public override byte Level
        {
            get
            {
                return _lvl;
            }
            set
            {
                _lvl = value;
            }
        }
        public override ushort Mana
        {
            get { return 0; }
            set { }
        }
        private int _attackrange = 2;
        public override int AttackRange
        {
            get { return _attackrange; }
        }
        #endregion

    }
}
