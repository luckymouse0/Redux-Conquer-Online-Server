using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [720011] Panacea Pack
	/// </summary>
    public class Item_720011 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Inventory.Count >= 37)
                return;
            for(var amt = 0; amt < 3; amt++)
                _client.CreateItem(1002000);
            _client.DeleteItem(_item);
		}
	}
}
