using Redux.Enum;
using Redux.Game_Server;


namespace Redux.Packets.Game
{
    public unsafe struct TeamInteractPacket
    {
        public TeamInteractType Type;
        public uint UID;

        public static TeamInteractPacket Create(uint uid, TeamInteractType type)
        {
            TeamInteractPacket packet = new TeamInteractPacket();
            packet.UID = uid;
            packet.Type = type;
            return packet;
        }

        public static implicit operator TeamInteractPacket(byte* ptr)
        {
            var packet = new TeamInteractPacket();      
            packet.Type = *((TeamInteractType*)(ptr + 4));
            packet.UID = *((uint*)(ptr + 8));
            return packet;
        }

        public static implicit operator byte[](TeamInteractPacket packet)
        {
            var buffer = new byte[20];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_TEAM_INTERACT);
                *((TeamInteractType*)(ptr + 4)) = packet.Type;
                *((uint*)(ptr + 8)) = packet.UID;
            }
            return buffer;
        }
    }
}
