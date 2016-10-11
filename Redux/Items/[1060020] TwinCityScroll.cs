/*
 * User: cookc
 * Date: 7/20/2013
 * Time: 11:42 AM
 */
using System;
using Redux.Game_Server;
using Redux.Structures;

namespace Redux.Items
{
	/// <summary>
	/// Handles item usage for [1060020] Twin City Scroll
	/// </summary>
    public class Item_1060020: IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Map.IsNoScrollEnabled)
                return;
            _client.ChangeMap(1002);
            _client.DeleteItem(_item.UniqueID);
		}
	}
}
