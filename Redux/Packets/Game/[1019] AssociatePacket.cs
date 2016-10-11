using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct AssociatePacket
    {
        public uint FriendID;
        public AssociateAction Action;
        public bool Online;
        private fixed sbyte _name[Constants.MAX_NAMESIZE];

        public string Name
        {
            get
            {
                fixed (sbyte* ptr = _name)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _name)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public static AssociatePacket Create(Associate assoc)
        {
            AssociatePacket packet = new AssociatePacket();
            packet.FriendID = assoc.UID;
            packet.Name = assoc.Name;
            packet.Online = assoc.IsOnline;
            return packet;
        }

        public static AssociatePacket Create(Associate assoc, AssociateAction action)
        {
            AssociatePacket packet = Create(assoc);
            packet.Action = action;
            return packet;
        }

        public static AssociatePacket Create(uint uid, AssociateAction action, bool online, string name)
        {
            AssociatePacket packet = new AssociatePacket();
            packet.FriendID = uid;
            packet.Action = action;
            packet.Online = online;
            packet.Name = name;
            return packet;
        }

        public static implicit operator AssociatePacket(byte* ptr)
        {
            var packet = new AssociatePacket();
            packet.FriendID = *(uint*)(ptr + 4);
            packet.Action = (AssociateAction)(*(byte*)(ptr + 8));
            packet.Online = *(bool*)(ptr + 9);            
            //TODO: packet.Name = 
            return packet;
        }

        public static implicit operator byte[](AssociatePacket packet)
        {
            var buffer = new byte[48];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_ASSOCIATE);
                *((uint*)(ptr + 4)) = packet.FriendID;
                *((byte*)(ptr + 8)) = (byte)packet.Action;
                *((bool*)(ptr + 9)) = packet.Online;
                MSVCRT.memcpy(ptr + 20, packet._name, Constants.MAX_NAMESIZE);
            }
            return buffer;
        }
    }
}
