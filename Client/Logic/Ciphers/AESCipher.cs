using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class AESCipher
    {
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456");

        public static string Encrypt(string plainText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs)) { sw.Write(plainText); }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static byte[] GenerateRandomKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128; // 128 bit
                aes.GenerateKey();
                return aes.Key;
            }
        }
    }
}