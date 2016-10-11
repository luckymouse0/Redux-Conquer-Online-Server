using System;
using System.Runtime.InteropServices;
using OpenSSL;


namespace Redux.Cryptography
{
    public class GameCryptography
    {
        OpenSSL.Blowfish _blowfish;
        public GameCryptography(byte[] key)
        {
            _blowfish = new Blowfish(BlowfishAlgorithm.CFB64);
            _blowfish.SetKey(key);
        }

        public void Decrypt(byte[] packet)
        {
            byte[] buffer = _blowfish.Decrypt(packet);
            System.Buffer.BlockCopy(buffer, 0, packet, 0, buffer.Length);
        }

        public void Encrypt(byte[] packet)
        {
            byte[] buffer = _blowfish.Encrypt(packet);
            System.Buffer.BlockCopy(buffer, 0, packet, 0, buffer.Length);
        }

        public Blowfish Blowfish
        {
            get { return _blowfish; }
        }
        public void SetKey(byte[] k)
        {
            _blowfish.SetKey(k);
        }
        public void SetIvs(byte[] i1, byte[] i2)
        {
            _blowfish.EncryptIV = i1;
            _blowfish.DecryptIV = i2;
        }
    }

}
