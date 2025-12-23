using System;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public class RSACipher
    {
        public static byte[] Encrypt(byte[] data, string publicKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKeyXml);
                return rsa.Encrypt(data, false);
            }
        }
    }
}