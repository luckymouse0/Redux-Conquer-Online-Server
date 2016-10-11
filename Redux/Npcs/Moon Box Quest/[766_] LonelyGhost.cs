using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [7660 - 7665] LonelyGhost
    /// Written by Matt
    /// </summary>

    public class NPC_7660 : INpc
    {
        public NPC_7660(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7660;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }

    public class NPC_7661 : INpc
    {
        public NPC_7661(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7661;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }

    public class NPC_7662 : INpc
    {
        public NPC_7662(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7662;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }

    public class NPC_7663 : INpc
    {
        public NPC_7663(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7663;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }

    public class NPC_7664 : INpc
    {
        public NPC_7664(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7664;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }

    public class NPC_7665 : INpc
    {
        public NPC_7665(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7665;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721072))
                    {
                        AddText("Can it be? Is that... YES!!! it's a SoulJade. I thought I would never see one of those. Please free me from this prision");
                        AddText("so I can join my family. I will give you this MoonBox I acquired before I died. Will you help me?");
                        AddOption("Yes, (use SoulJade)", 1);
                        AddOption("Sorry I cannoy help you yet", 255);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
                case 1:

                    if (_client.HasItem(721072))
                    {
                        //tele player out of map
                        _client.ChangeMap(1002, 417, 516);

                        //remove the SoulJade
                        _client.DeleteItem(721072);

                        //Add MoonBox
                        _client.CreateItem(Constants.MOONBOX_ID);
                    }
                    else
                    {
                        AddText("You cannot help us without a SoulJade. Please leave here until you get one.");
                        AddOption("I will get one at once", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}