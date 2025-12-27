using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class DESCipher
    {
        public static string Encrypt(string plainText, byte[] key)
        {
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.GenerateIV();
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(des.IV, 0, des.IV.Length);
                    
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