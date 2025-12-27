using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class AESCipher
    {
        public static string Decrypt(string cipherText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                try
                {
                    byte[] fullData = Convert.FromBase64String(cipherText);
                    
                    byte[] iv = new byte[16];
                    Array.Copy(fullData, 0, iv, 0, 16);
                    aes.IV = iv;
                    
                    byte[] cipherBytes = new byte[fullData.Length - 16];
                    Array.Copy(fullData, 16, cipherBytes, 0, cipherBytes.Length);
                    
                    using (MemoryStream ms = new MemoryStream(cipherBytes))
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