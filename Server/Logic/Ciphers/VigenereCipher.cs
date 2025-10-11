using System;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class VigenereCipher
    {
        public static string Encrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(key)) return text;
            StringBuilder result = new StringBuilder();
            key = key.ToUpper();
            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    int shift = key[keyIndex % key.Length] - 'A';
                    result.Append((char)(((c - baseChar + shift) % 26) + baseChar));
                    keyIndex++;
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        public static string Decrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(key)) return text;
            StringBuilder result = new StringBuilder();
            key = key.ToUpper();
            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    int shift = key[keyIndex % key.Length] - 'A';
                    result.Append((char)(((c - baseChar - shift + 26) % 26) + baseChar));
                    keyIndex++;
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
