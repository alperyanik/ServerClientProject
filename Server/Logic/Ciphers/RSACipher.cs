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
            // Sunucu her başladığında yeni bir 2048 bit RSA anahtarı üretir.
            _rsa = new RSACryptoServiceProvider(2048);
        }

        // İstemciye göndermek için SADECE Public Key'i al
        public string GetPublicKey()
        {
            return _rsa.ToXmlString(false); // false = Sadece Public Key
        }

        // İstemciden gelen şifreli AES/DES anahtarını çöz
        public byte[] Decrypt(byte[] encryptedData)
        {
            try
            {
                // OAEP padding false (PKCS#1 v1.5) uyumluluk için
                return _rsa.Decrypt(encryptedData, false);
            }
            catch (CryptographicException)
            {
                return null; // Şifre çözülemezse null döner
            }
        }
    }
}