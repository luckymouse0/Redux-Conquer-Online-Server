/*
 * User: cookc
 * Date: 27/11/2013
 * Time: 4:10 PM 
 */
using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Packets.Game;
namespace Redux.Items
{
    /// <summary>
    /// Handles item usage for [1200000] PrayingStone(s)
    /// </summary>
    public class Item_1200000 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.Character.HeavenBlessExpires = DateTime.Now.AddDays(3);
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.HeavenBlessing, Common.SecondsFromNow(_client.Character.HeavenBlessExpires)));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.SizeAdd, 2));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.OnlineTraining, 0));
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [1200001] PrayingStone(m)
    /// </summary>
    public class Item_1200001 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.Character.HeavenBlessExpires = DateTime.Now.AddDays(7);
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.HeavenBlessing, Common.SecondsFromNow(_client.Character.HeavenBlessExpires)));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.SizeAdd, 2));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.OnlineTraining, 0));
            _client.DeleteItem(_item);
        }
    }
    /// <summary>
    /// Handles item usage for [1200002] PrayingStone(l)
    /// </summary>
    public class Item_1200002 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            _client.Character.HeavenBlessExpires = DateTime.Now.AddDays(30);
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.HeavenBlessing, Common.SecondsFromNow(_client.Character.HeavenBlessExpires)));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.SizeAdd, 2));
            _client.Send(UpdatePacket.Create(_client.UID, Enum.UpdateType.OnlineTraining, 0));
            _client.DeleteItem(_item);
        }
    }
}
