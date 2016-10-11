using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [1001020] PearlOintment
	/// </summary>
    public class Item_1001020 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Mana >= _client.MaximumMana)
                return;
            _client.Mana = (ushort)Math.Min(_client.Mana + _item.BaseItem.ManaAdd, _client.MaximumMana);
            _client.DeleteItem(_item);
		}
	}
}
