using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Game_Server;
using Redux.Structures;
namespace Redux.Items
{
    public class Manager
    {
        public static void ProcessItem(Player _client, ConquerItem _item)
        {
        	try
        	{
	            var type = Type.GetType("Redux.Items.Item_" + _item.StaticID);
	            IItem item = Activator.CreateInstance(type) as IItem;
	            item.Run(_client,_item);
            }
            catch { _client.SendMessage("Could not load script for item ID: " + _item.StaticID); }
        }
    }
}
