using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Redux.Packets
{
    public unsafe class NetStringPacker
    {
        private readonly List<string> _values;

        public NetStringPacker()
        {
            _values = new List<string>();
        }

        public NetStringPacker(int stringCount)
        {
            _values = new List<string>(stringCount);
            for (var i = 0; i < stringCount; i++) _values.Add(null);
        }

        /// <summary>
        /// Use this constructor for variable-length strings.
        /// </summary>
        /// <param name="ptr"></param>
        public NetStringPacker(byte* ptr)
        {
            int offset = 0;
            var stringCount = *(ptr + offset++);
            _values = new List<string>(stringCount);

            for (var i = 0; i < stringCount; i++)
            {
                var length = *(ptr + offset++);
                _values.Add(new string((sbyte*)(ptr + offset), 0, length, Encoding.Default));
                offset += length;
            }
        }

        public bool AddString(string pszString)
        {
            //if (_values.Count >= _values.Capacity) return false;

            //if (pszString.Length != 0)
            {
                int length = pszString.Length;
                if (length > 255) return false;

                _values.Add(pszString);
            }

            return true;
        }

        public bool GetString(int index, out string value)
        {
            value = string.Empty;

            if (index < 0 || index >= _values.Count)
                return false;

            value = _values[index];
            return true;
        }

        public bool SetString(int index, string value)
        {
            if (index < 0 || index >= _values.Count)
                return false;

            _values[index] = value;
            return true;
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(int index)
        {
            return Count > index;
        }

        public int Capacity
        {
            get { return _values.Capacity; }
            set { _values.Capacity = value; }
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public int Length
        {
            get
            {
                return 1 + Count + _values.Sum(str => str.Length);
            }
        }

        public byte[] ToArray()
        {
            var buffer = new byte[Length];
            var offset = 1;

            fixed (byte* ptr = buffer)
            {
                *ptr = (byte)Count;
                for (var i = 0; i < Count; i++)
                {
                    string value;
                    if (GetString(i, out value))
                    {
                        *(ptr + offset++) = (byte)value.Length;
                        value.CopyTo(ptr + offset);
                        offset += value.Length;
                    }
                    else
                    {
                        offset++; // compensate for length byte
                    }
                }
            }

            return buffer;
        }

        public static implicit operator byte[](NetStringPacker packer)
        {
            return packer.ToArray();
        }
    }
}
