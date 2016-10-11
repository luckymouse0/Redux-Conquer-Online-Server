using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Enum;
namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [721333] ChantPill
	/// </summary>
    public class Item_721333 : IItem
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
