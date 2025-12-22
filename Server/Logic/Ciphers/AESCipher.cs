using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class AESCipher
    {
        // Basitlik için sabit IV. Gerçekte dinamik olmalı.
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456");

        public static string Decrypt(string cipherText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                try
                {
                    byte[] buffer = Convert.FromBase64String(cipherText);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs)) { return sr.ReadToEnd(); }
                        }
                    }
                }
                catch { return "[Şifre Çözme Hatası]"; }
            }
        }
    }
}