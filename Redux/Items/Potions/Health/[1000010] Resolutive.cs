using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [1000010] Resolutive
	/// </summary>
    public class Item_1000010 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Life >= _client.MaximumLife)
                return;
            _client.Life = Math.Min(_client.Life + _item.BaseItem.HealthAdd, _client.MaximumLife);
            _client.DeleteItem(_item);
		}
	}
}
