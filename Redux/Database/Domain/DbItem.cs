using Redux.Enum;
using Redux.Structures;

namespace Redux.Database.Domain
{
    public class DbItem
    {        
    	public DbItem(uint _owner, ConquerItem _item)
    	{
    		Owner = _owner;
    		UniqueID = _item.UniqueID;
    		StaticID = _item.StaticID;
    		Durability = _item.Durability;
    		Location = _item.Location;
    		Plus = _item.Plus;
    		Bless = _item.Bless;
    		Enchant = _item.Enchant;
    		Gem1 = _item.Gem1;
    		Gem2 = _item.Gem2;
    		Effect = _item.Effect;
    		Locked = _item.Locked;    		
    	}
        public DbItem()
        {
        }
        public virtual uint UniqueID { get; set; }
        public virtual uint StaticID { get; set; }
        public virtual uint Owner { get; set; }
        public virtual ushort Durability { get; set; }
        public virtual ItemLocation Location { get; set; }
        public virtual byte Color { get; set; }
        public virtual byte Plus { get; set; }
        public virtual byte Bless { get; set; }
        public virtual byte Enchant { get; set; }
        public virtual byte Gem1 { get; set; }
        public virtual byte Gem2 { get; set; }
        public virtual byte Effect { get; set; }
        public virtual bool Locked { get; set; }
    }
}

