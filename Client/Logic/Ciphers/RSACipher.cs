using System;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public class RSACipher
    {
        // İstemci, AES/DES anahtarını Sunucunun Public Key'i ile şifreler
        public static byte[] Encrypt(byte[] data, string publicKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKeyXml);
                // Sunucu ile aynı padding ayarı (false = PKCS#1 v1.5)
                return rsa.Encrypt(data, false);
            }
        }
    }
}