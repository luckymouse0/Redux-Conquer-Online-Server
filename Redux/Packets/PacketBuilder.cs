using System.Text;


namespace Redux.Packets
{
    public unsafe partial class PacketBuilder
    {
        /// <summary>
        /// Appends size and type information to a packet pointer.
        /// </summary>
        /// <param name="ptr">Pointer to the packet</param>
        /// <param name="size">Size of the packet</param>
        /// <param name="type">Type of the packet</param>
        public static void AppendHeader(byte* ptr, int size, ushort type)
        {
            *((ushort*)ptr) = (ushort)(size - 8);
            *((ushort*)(ptr + 2)) = type;
        }
        public static string GetProperString(sbyte* ptr, int length)
        {
            return new string(ptr, 0, length, Encoding.Default).Split('\0')[0];
        }

        public static void AppendNetStringPacker(byte* buffer, NetStringPacker packer)
        {
            var bufPacker = packer.ToArray();
            fixed (byte* ptr = bufPacker)
            {
                MSVCRT.memcpy(buffer, ptr, bufPacker.Length);
            }
        }
    }
}

