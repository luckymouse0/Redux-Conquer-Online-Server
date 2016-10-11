/*
 * User: cookc
 * Date: 27/11/2013
 * Time: 4:10 PM 
 */
using System;
using Redux.Game_Server;
using Redux.Structures;

namespace Redux.Items
{
	/// <summary>
    /// Handles item usage for [723583] Ninja Amulet
	/// </summary>
    public class Item_723583: IItem
	{		
        public override void Run(Player _client, ConquerItem _item) 
        {
            if (_client.Lookface % 2 == 0)            
                _client.Lookface--;            
            else
                _client.Lookface++;
            _client.Character.Lookface = _client.Lookface;
            _client.Save();
            _client.DeleteItem(_item);
		}
	}
}
