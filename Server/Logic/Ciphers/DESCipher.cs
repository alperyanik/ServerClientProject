using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class DESCipher
    {
        public static string Decrypt(string cipherText, byte[] key)
        {
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;

                try
                {
                    byte[] fullData = Convert.FromBase64String(cipherText);
                    
                    byte[] iv = new byte[8];
                    Array.Copy(fullData, 0, iv, 0, 8);
                    des.IV = iv;
                    
                    byte[] cipherBytes = new byte[fullData.Length - 8];
                    Array.Copy(fullData, 8, cipherBytes, 0, cipherBytes.Length);
                    
                    using (MemoryStream ms = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs)) { return sr.ReadToEnd(); }
                        }
                    }
                }
                catch { return "[Þifre Çözme Hatasý]"; }
            }
        }
    }
}