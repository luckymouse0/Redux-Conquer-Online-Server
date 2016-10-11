using System;
using Redux.Game_Server;
using Redux.Structures;

namespace Redux.Items
{
    #region Black Dye
    /// <summary>
    /// Handles item usage for [1060030] BlackDye
	/// </summary>
    public class Item_1060030 : IItem
	{		
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 3;
            client.DeleteItem(item);
		}
	}
    #endregion
    #region Violet Dye
    /// <summary>
    /// Handles item usage for [1060040] VioletDye
    /// </summary>
    public class Item_1060040 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 9;
            client.DeleteItem(item);
        }
    }
    #endregion    
    #region Blue Dye
    /// <summary>
    /// Handles item usage for [1060050] BlueDye
    /// </summary>
    public class Item_1060050 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 8;
            client.DeleteItem(item);
        }
    }
    #endregion    
    #region Green Dye
    /// <summary>
    /// Handles item usage for [1060060] GreenDye
    /// </summary>
    public class Item_1060060 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 7;
            client.DeleteItem(item);
        }
    }
    #endregion    
    #region Brown Dye
    /// <summary>
    /// Handles item usage for [1060070] BrownDye
    /// </summary>
    public class Item_1060070 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 6;
            client.DeleteItem(item);
        }
    }
    #endregion    
    #region Red Dye
    /// <summary>
    /// Handles item usage for [1060080] RedDye
    /// </summary>
    public class Item_1060080 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 5;
            client.DeleteItem(item);
        }
    }
    #endregion
    #region White Dye
    /// <summary>
    /// Handles item usage for [1060090] WhiteDye
    /// </summary>
    public class Item_1060090 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            client.HairColour = 4;
            client.DeleteItem(item);
        }
    }
    #endregion
}
