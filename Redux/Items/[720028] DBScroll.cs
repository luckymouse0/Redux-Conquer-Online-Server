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
	/// Handles item usage for [720028] DBScroll
	/// </summary>
	public class Item_720028:IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
        	if(_client.Inventory.Count > 30)
        		return;
            _client.DeleteItem(_item);
        	for(var i = 0; i < 10; i++)
        		_client.CreateItem(Constants.DRAGONBALL_ID);
		}
	}
}
