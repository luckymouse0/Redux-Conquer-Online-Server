/*
 * User: pro4never
 * Date: 25/10/2014
 * Time: 7:33 PM 
 */
using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Packets.Game;
namespace Redux.Items
{
    /// <summary>
    /// Handles item usage for [720030] Firework
    /// </summary>
    public class Item_720030 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720031] EndlessLove
    /// </summary>
    public class Item_720031 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true); 
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720032] MyWish
    /// </summary>
    public class Item_720032 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720033] FineRocketWrap
    /// </summary>
    public class Item_720033 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720034] GreatRocketWrap
    /// </summary>
    public class Item_720034 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720035] SuperRocketWrap
    /// </summary>
    public class Item_720035 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720036] FineSkyrocket
    /// </summary>
    public class Item_720036 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720037] GreatSkyrocket
    /// </summary>
    public class Item_720037 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720038] SuperSkyrocket
    /// </summary>
    public class Item_720038 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [720094] EasterFireworks
    /// </summary>
    public class Item_720094 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [721862] AutumnFirework
    /// </summary>
    public class Item_721862 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.SendToScreen(StringsPacket.Create(_client.UID, Enum.StringAction.Fireworks, _item.BaseItem.Name), true);
            _client.DeleteItem(_item);
        }
    }
}
