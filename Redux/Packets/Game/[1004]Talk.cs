using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Utility;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public struct TalkPacket
    {
        public uint Color;
        public ChatType Type;
        public ushort Unknown0;
        public uint Time;
        public uint HearerLookface;
        public uint SpeakerLookface;
        public NetStringPacker StringPacker;

        public int Size
        {
            get { return 24 + StringPacker.Length; }
        }

        public string Speaker
        {
            get
            {
                string value;
                return StringPacker.GetString(0, out value) ? value : null;
            }
            set { StringPacker.SetString(0, value); }
        }

        public string Hearer
        {
            get
            {
                string value;
                return StringPacker.GetString(1, out value) ? value : null;
            }
            set { StringPacker.SetString(1, value); }
        }

        public string Emotion
        {
            get
            {
                string value;
                return StringPacker.GetString(2, out value) ? value : null;
            }
            set { StringPacker.SetString(2, value); }
        }

        public string Words
        {
            get
            {
                string value;
                return StringPacker.GetString(3, out value) ? value : null;
            }
            set { StringPacker.SetString(3, value); }
        }

        public TalkPacket(string words)
            : this(ChatType.Talk2, words)
        {
        }

        public TalkPacket(ChatType type, string words, ChatColour colour)
            : this(Constants.SYSTEM_NAME, Constants.ALLUSERS_NAME, words, string.Empty, (uint) colour, type)
        {
        }


        public TalkPacket(ChatType type, string words)
            : this(Constants.SYSTEM_NAME, Constants.ALLUSERS_NAME, words, string.Empty, 0x00ffffffu, type)
        {
        }

        public TalkPacket(string speaker, string hearer, string words, string emotion, uint color, ChatType type)
        {
            StringPacker = new NetStringPacker(4);
            Color = color;
            Type = type;
            Unknown0 = 0;
            Time = 0;
            HearerLookface = SpeakerLookface = 0;
            StringPacker.SetString(0, speaker);
            StringPacker.SetString(1, hearer);
            StringPacker.SetString(2, emotion);
            StringPacker.SetString(3, words);
            StringPacker.SetString(4, string.Empty);
            StringPacker.SetString(5, string.Empty);
        }

        public void FormatWords(string format, params object[] args)
        {
            Words = string.Format(format, args);
        }

        public unsafe static implicit operator byte[](TalkPacket packet)
        {
            var buffer = new byte[24 + 3 + packet.StringPacker.Length + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_TALK);
                *((uint*)(ptr + 4)) = packet.Color;
                *((ChatType*)(ptr + 8)) = packet.Type;
                *((ushort*)(ptr + 10)) = packet.Unknown0;
                *((uint*)(ptr + 12)) = packet.Time;
                *((uint*)(ptr + 16)) = packet.HearerLookface;
                *((uint*)(ptr + 20)) = packet.SpeakerLookface;
                PacketBuilder.AppendNetStringPacker(ptr + 24, packet.StringPacker);
            }
            return buffer;
        }

        public unsafe static implicit operator TalkPacket(byte* ptr)
        {
            var packet = new TalkPacket();
            packet.Color = *((uint*)(ptr + 4));
            packet.Type = *((ChatType*)(ptr + 8));
            packet.Unknown0 = *((ushort*)(ptr + 10));
            packet.Time = *((uint*)(ptr + 12));
            packet.HearerLookface = *((uint*)(ptr + 16));
            packet.SpeakerLookface = *((uint*)(ptr + 20));
            packet.StringPacker = new NetStringPacker(ptr + 24);
            return packet;
        }
    }
}
