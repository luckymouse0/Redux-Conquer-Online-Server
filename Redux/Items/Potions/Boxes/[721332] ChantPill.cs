using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [721332] ChantPill Pack
	/// </summary>
    public class Item_721332 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Inventory.Count >= 37)
                return;
            for(var amt = 0; amt < 3; amt++)
                _client.CreateItem(1002040);
            _client.DeleteItem(_item);
		}
	}
}
