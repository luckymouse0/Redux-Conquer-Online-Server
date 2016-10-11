using System;
using System.IO;
using System.Text;

namespace Redux.Cryptography
{
    public unsafe class RC5
    {
        private readonly uint[] _bufKey;
        private readonly uint[] _bufSub;

        public RC5()
            : this(Constants.RC5_PASSWORDKEY)
        {
        }

        public static uint RotateLeft(uint data, int count)
        {
            count %= 32;

            var high = data >> (32 - count);
            return (data << count) | high;
        }

        public static uint RotateRight(uint data, int count)
        {
            count %= 32;

            var low = data << (32 - count);
            return (data >> count) | low;
        }
        public RC5(byte[] key)
        {
            _bufKey = new uint[Constants.RC5_KEY];
            _bufSub = new uint[Constants.RC5_SUB];

            fixed (byte* pKey = key)
            {
                fixed (uint* pKey2 = _bufKey)
                {
                    MSVCRT.memcpy(pKey2, pKey, Constants.RC5_16);
                }
            }

            _bufSub[0] = Constants.RC5_PW32;
            for (var i = 1; i < Constants.RC5_SUB; i++)
            {
                _bufSub[i] = _bufSub[i - 1] + Constants.RC5_QW32;
            }

            int ii, j;
            uint x, y;
            ii = j = 0;
            x = y = 0;
            for (var k = 0; k < 3 * Math.Max(Constants.RC5_KEY, Constants.RC5_SUB); k++)
            {
                _bufSub[ii] = RotateLeft(_bufSub[ii] + x + y, 3);
                x = _bufSub[ii];
                ii = (ii + 1) % Constants.RC5_SUB;

                _bufKey[j] = RotateLeft(_bufKey[j] + x + y, (int)(x + y));
                y = _bufKey[j];
                j = (j + 1) % Constants.RC5_KEY;
            }
        }

        public void Encrypt(void* buffer, int length)
        {
            if (length % 8 != 0) throw new ArgumentException("Length must be a multiple of 8!", "length");

            var length8 = (length / 8) * 8;
            if (length8 <= 0) return;

            var bufData = (uint*)buffer;
            for (var k = 0; k < length8 / 8; k++)
            {
                uint a = bufData[2 * k];
                uint b = bufData[2 * k + 1];

                uint le = a + _bufSub[0];
                uint re = b + _bufSub[1];
                for (var i = 1; i <= Constants.RC5_12; i++)
                {
                    le = RotateLeft(le ^ re, (int)re) + _bufSub[2 * i];
                    re = RotateLeft(re ^ le, (int)le) + _bufSub[2 * i + 1];
                }

                bufData[2 * k] = le;
                bufData[2 * k + 1] = re;
            }
        }

        public void Decrypt(void* buffer, int length)
        {
            if (length % 8 != 0) throw new ArgumentException("Length must be a multiple of 8!", "length");

            var length8 = (length / 8) * 8;
            if (length8 <= 0) return;

            var bufData = (uint*)buffer;
            for (var k = 0; k < length8 / 8; k++)
            {
                uint ld = bufData[2 * k];
                uint rd = bufData[2 * k + 1];
                for (var i = Constants.RC5_12; i >= 1; i--)
                {
                    rd = RotateRight(rd - _bufSub[2 * i + 1], (int)ld) ^ ld;
                    ld = RotateRight(ld - _bufSub[2 * i], (int)rd) ^ rd;
                }

                uint b = rd - _bufSub[1];
                uint a = ld - _bufSub[0];

                bufData[2 * k] = a;
                bufData[2 * k + 1] = b;
            }
        }
    }
}