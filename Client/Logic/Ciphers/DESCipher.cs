using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class DESCipher
    {
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("12345678");

        public static string Encrypt(string plainText, byte[] key)
        {
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.IV = IV;
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs)) { sw.Write(plainText); }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static byte[] GenerateRandomKey()
        {
            using (DES des = DES.Create())
            {
                des.GenerateKey();
                return des.Key;
            }
        }
    }
}