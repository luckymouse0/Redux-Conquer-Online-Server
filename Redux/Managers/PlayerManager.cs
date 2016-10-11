using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Redux.Game_Server;
using Redux.Enum;

namespace Redux.Managers
{
    public static class PlayerManager
    {
        #region Variables
        public static ConcurrentDictionary<uint, Player> Players = new ConcurrentDictionary<uint, Player>();
        public static List<Packets.Game.TalkPacket> Broadcasts = new List<Packets.Game.TalkPacket>();
        public static DateTime NextBroadcast = DateTime.Now;
        #endregion
        #region Functions

        public static Player GetUser(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            name = name.ToLower();
            return Players.Values.FirstOrDefault(user => user.Character.Name.ToLower() == name);
        }

        public static Player GetUser(uint id)
        {
            if (id == 0) return null;
            if (!Players.ContainsKey(id)) return null;
            return Players[id];
        }

        public static void SendToServer(byte[] packet)
        {
            foreach (var player in Players.Values)
                player.Send(packet);
        }

        public static bool AddPlayer(Player client)
        {
            bool success = Players.TryAdd(client.UID, client);
            ControlForm.GUI.SetOnlineCount(Players.Count);
            return success;
        }

        public static bool RemovePlayer(Player client)
        {
            Player _out;
            Players.TryRemove(client.UID, out _out);
            ControlForm.GUI.SetOnlineCount(Players.Count);
            if (client.CombatManager != null)
                client.CombatManager.Save();          
            return _out != null;
        }

        public static void PlayerManager_Tick()
        {
            foreach (var player in Players.Values)
            {
                player.On_Player_Timer();
                player.On_Entity_Timer();
            }

            if (Broadcasts.Count > 0 && DateTime.Now > NextBroadcast)
            {
                SendToServer(Broadcasts[0]);
                Broadcasts.RemoveAt(0);
                NextBroadcast = DateTime.Now.AddMinutes(1);
            }
        }

        public static void CombatManager_Tick()
        {
            foreach (var player in Players.Values)
            {
                if (player.CombatManager != null && player.CombatManager.nextTrigger != 0)
                    if (Common.Clock > player.CombatManager.nextTrigger)
                        player.CombatManager.OnTimer();
            }
        }

        public static void PacketManager_Tick()
        {
            foreach (var player in Players.Values)            
                player.On_Packet_Timer();            
        }

        public static IEnumerable<Player> Select(Func<Player, bool> predicate)
        {
            return Players.Values.Where(predicate);
        }

        #endregion
    }
}
