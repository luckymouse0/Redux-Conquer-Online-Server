using System;
using Redux.Game_Server;
using Redux.Structures;

namespace Redux.Items
{
	/// <summary>
	/// Handles item usage for [723584] Black Tulip
	/// </summary>
	public class Item_723584:IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
        	ConquerItem arm;
            _client.TryGetEquipmentByLocation(Enum.ItemLocation.Armor, out arm);

            if (arm != null && arm.Color != 2)
            {
                arm.Color = 2;
                _client.SpawnPacket.ArmorColor = 2;
                _client.SendToScreen(_client.SpawnPacket, true);
                _client.Send(Packets.Game.ItemInformationPacket.Create(arm, Enum.ItemInfoAction.Update));
                arm.Save();
                _client.DeleteItem(_item);
            }
		}
	}
}
