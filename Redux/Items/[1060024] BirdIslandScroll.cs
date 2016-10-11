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
    /// Handles item usage for [1060024] Bird Island Scroll
	/// </summary>
    public class Item_1060024 : IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Map.IsNoScrollEnabled)
                return;
            _client.ChangeMap(1015);
            _client.DeleteItem(_item.UniqueID);
		}
	}
}
