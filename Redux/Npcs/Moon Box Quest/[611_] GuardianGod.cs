using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [6110 - 6116] GuardianGod
    /// Written by Matt
    /// </summary>

    #region Peace Tatic
    public class NPC_6110 : INpc
    {
        public NPC_6110(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6110;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Peace Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Chaos Tatic
    public class NPC_6111 : INpc
    {
        public NPC_6111(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6111;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Chaos Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Deserted Tatic
    public class NPC_6112 : INpc
    {
        public NPC_6112(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6112;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Deserted Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Prosperous Tatic
    public class NPC_6113 : INpc
    {
        public NPC_6113(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6113;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Prosperous Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Disturbed Tatic
    public class NPC_6114 : INpc
    {
        public NPC_6114(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6114;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Disturbed Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Calmed Tatic
    public class NPC_6115 : INpc
    {
        public NPC_6115(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6115;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You have entered the Calmed Tatic. If you have not already, kill the monsters until you get a CommandToken. That is the ");
                    AddText("only way you can leave this awful place. Good luck adventurer.");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Death Tatic
    public class NPC_6116 : INpc
    {
        public NPC_6116(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6116;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I am so sorry to tell you that you are in the Death Tatic. There is no way to leave this place besides death.");
                    AddOption("Oh my", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion
}