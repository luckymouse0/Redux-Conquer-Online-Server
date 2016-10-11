/*
 * User: pro4never
 * Date: 9/21/2013
 * Time: 11:11 AM
 */
using System;
using Redux.Game_Server;
using Redux.Structures;

namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [1060023] Castle City Scroll
	/// </summary>
    public class Item_1060023 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Map.IsNoScrollEnabled)
                return;
            _client.ChangeMap(1011);
            _client.DeleteItem(_item.UniqueID);
		}
	}
}
