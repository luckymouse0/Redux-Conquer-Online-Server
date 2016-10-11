using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{
    public class INpc
    {
    	public uint ID;
    	public ushort Face;
        private Player client;
        public bool IsGlobal = false;
        public INpc(Player _client)
        {
            client = _client;
        }
        public virtual void Run(Player _client, ushort _linkback) { }
        public List<NpcDialogPacket> Responses;

        public void Send()
        {
            if (Responses.Count > 2)
                foreach (var response in Responses)
                    client.Send(response);
        }
        
        
        public void AddText(string _value)
        {
        	
        	var packet = NpcDialogPacket.Create();
        	packet.Action= DialogAction.Dialog;
        	packet.Strings.AddString(_value);
        	Responses.Add( packet);
        }

        public void AddInput(string _value, byte _linkback)
        {

            var packet = NpcDialogPacket.Create();
            packet.Action = DialogAction.Input;
            packet.Linkback = _linkback;
            packet.Strings.AddString(_value);
            Responses.Add(packet);
        }
        
        public void AddAvatar()
        {
        	var packet = NpcDialogPacket.Create();
        	packet.Action= DialogAction.Avatar;
            packet.ID = Face;
        	Responses.Add( packet);        	
        }

        public void AddOption(string _value, byte _linkback)
        {
            var packet = NpcDialogPacket.Create();
            packet.Action = DialogAction.Option;
            packet.Linkback = _linkback;
            packet.Strings.AddString(_value);
            Responses.Add(packet);
        }

        public void AddFinish()
        {
            var packet = NpcDialogPacket.Create();
            packet.Action = DialogAction.Finish;
            packet.Linkback = 255;
            Responses.Add(packet);
        }

        public void Popup(ushort _type)
        {
            client.Send(GeneralActionPacket.Create(client.UID, DataAction.OpenWindow, _type));
        }
        
    }
}
