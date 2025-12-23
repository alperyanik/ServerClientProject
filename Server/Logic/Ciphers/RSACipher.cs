using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic.Ciphers
{
    public class RSACipher
    {
        private RSACryptoServiceProvider _rsa;

        public RSACipher()
        {
            _rsa = new RSACryptoServiceProvider(2048);
        }

        public string GetPublicKey()
        {
            return _rsa.ToXmlString(false);
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            try
            {
                return _rsa.Decrypt(encryptedData, false);
            }
            catch (CryptographicException)
            {
                return null;
            }
        }
    }
}