using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [6120 - 6126] Ghost
    /// Written by Matt
    /// </summary>

    #region Peace Tatic
    public class NPC_6120 : INpc
    {
        public NPC_6120(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6120;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721010))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721010))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Chaos Tatic
    public class NPC_6121 : INpc
    {
        public NPC_6121(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6121;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721011, 1))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721011))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Deserted Tatic
    public class NPC_6122 : INpc
    {
        public NPC_6122(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6122;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721012))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721012))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Prosperous Tatic
    public class NPC_6123 : INpc
    {
        public NPC_6123(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6123;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721013, 1))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721013))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Disturbed Tatic
    public class NPC_6124 : INpc
    {
        public NPC_6124(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6124;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721014))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721014))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Calmed Tatic
    public class NPC_6125 : INpc
    {
        public NPC_6125(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6125;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721015))
                    {
                        AddText("Since you have a token to this tatic, I am able to harness its power to send you out. Are you ready? ");
                        AddOption("Yes, send me out", 1);
                        AddOption("Thanks", 255);
                    }
                    else
                    {
                        AddText("I wish I could help you adventurer but I need the token the monsters are holding in order to send you out.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(721015))
                        _client.ChangeMap(1042, 35, 48);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion

    #region Death Tatic
    public class NPC_6126 : INpc
    {
        public NPC_6126(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6126;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I am so sorry but I cannot help you. You must die like i did in this tatic.");
                    AddOption("Oh my", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
    #endregion
}