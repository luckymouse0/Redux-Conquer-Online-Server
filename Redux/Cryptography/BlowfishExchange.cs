using OpenSSL;

namespace Redux.Cryptography
{
    public class ServerKeyExchange
    {
        private const string P = "E7A69EBDF105F2A6BBDEAD7E798F76A209AD73FB466431E2E7352ED262F8C558F10BEFEA977DE9E21DCEE9B04D245F300ECCBBA03E72630556D011023F9E857F";
        private const string G = "05";
        private const int PAD_LENGTH = 11;
        private const int JUNK_LENGTH = 12;
        private const string TQSERVER = "TQServer";

        private DH _keyExchange;
        private byte[] _clientIV;
        private byte[] _serverIV;

        public byte[] CreateServerKeyPacket()
        {
            _clientIV = new byte[8];
            _serverIV = new byte[8];

            _keyExchange = new DH(BigNumber.FromHexString(P), BigNumber.FromHexString(G));
            _keyExchange.GenerateKeys();

            return GeneratePacket();
        }

        public void HandleClientKeyPacket(string publicKey, ref GameCryptography crypto)
        {
            var key = _keyExchange.ComputeKey(BigNumber.FromHexString(publicKey));
            crypto.SetKey(key);
            crypto.SetIvs(_clientIV, _serverIV);
        }

        public unsafe byte[] GeneratePacket()
        {
            var pad = new byte[PAD_LENGTH];
            var junk = new byte[JUNK_LENGTH];

            Common.Random.NextBytes(pad);
            Common.Random.NextBytes(junk);

            var publicKey = _keyExchange.PublicKey.ToHexString();

            var size = 28 + PAD_LENGTH + JUNK_LENGTH + _clientIV.Length + _serverIV.Length + P.Length + G.Length + publicKey.Length + 8;

            var buffer = new byte[size];
            fixed (byte* ptr = buffer, pPad = pad, pJunk = junk, pClientIV = _clientIV, pServerIV = _serverIV)
            {
                var offset = 0;

                MSVCRT.memcpy(ptr + offset, pPad, PAD_LENGTH);
                offset += PAD_LENGTH;

                *((int*)(ptr + offset)) = size - PAD_LENGTH;
                offset += 4;

                *((int*)(ptr + offset)) = JUNK_LENGTH;
                offset += 4;

                MSVCRT.memcpy(ptr + offset, pJunk, JUNK_LENGTH);
                offset += JUNK_LENGTH;

                *((int*)(ptr + offset)) = _clientIV.Length;
                offset += 4;

                MSVCRT.memcpy(ptr + offset, pClientIV, _clientIV.Length);
                offset += _clientIV.Length;

                *((int*)(ptr + offset)) = _serverIV.Length;
                offset += 4;

                MSVCRT.memcpy(ptr + offset, pServerIV, _serverIV.Length);
                offset += _serverIV.Length;

                *((int*)(ptr + offset)) = P.Length;
                offset += 4;

                P.CopyTo(ptr + offset);
                offset += P.Length;

                *((int*)(ptr + offset)) = G.Length;
                offset += 4;

                G.CopyTo(ptr + offset);
                offset += G.Length;

                *((int*)(ptr + offset)) = publicKey.Length;
                offset += 4;

                publicKey.CopyTo(ptr + offset);
                offset += publicKey.Length;

                TQSERVER.CopyTo(ptr + offset);
            }

            return buffer;
        }
    }
}
