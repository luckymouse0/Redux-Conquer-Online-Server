/*
 * User: cookc
 * Date: 27/11/2013
 * Time: 4:10 PM 
 */
using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Packets.Game;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [723017] ExpPotion
	/// </summary>
    public class Item_723017: IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            _client.Character.DoubleExpExpires = DateTime.Now.AddHours(1);
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.DoubleExpTime, Common.SecondsFromNow(_client.Character.DoubleExpExpires)));
            _client.DeleteItem(_item);
		}
	}
}
