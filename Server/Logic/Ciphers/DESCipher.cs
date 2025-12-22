using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class DESCipher
    {
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("12345678");

        public static string Decrypt(string cipherText, byte[] key)
        {
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.IV = IV;
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                try
                {
                    byte[] buffer = Convert.FromBase64String(cipherText);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
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