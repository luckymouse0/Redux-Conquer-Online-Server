using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Game_Server;
using Redux.Structures;
namespace Redux.Items
{
    public class IItem
    {    	
        public virtual void Run(Player _client, ConquerItem _item) { }
    }
}
