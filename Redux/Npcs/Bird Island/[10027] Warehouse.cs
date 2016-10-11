using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_10027 : INpc
    {

        public NPC_10027(Game_Server.Player _client)
            : base(_client)
        {
    		ID = 10027;	
			Face = 5;    
    	}
    	
        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            _client.Send(GeneralActionPacket.Create(_client.UID, Enum.DataAction.OpenWindow, 4));
        }
    }
}
