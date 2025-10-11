using System;

namespace Client.Logic.Ciphers
{
    public static class VigenereCipher
    {
        public static string Encrypt(string text, string key)
        {
            string result = "";
            key = key.ToUpper();
            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    int shift = key[keyIndex % key.Length] - 'A';
                    result += (char)((((c - baseChar) + shift) % 26) + baseChar);
                    keyIndex++;
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }

        public static string Decrypt(string text, string key)
        {
            string result = "";
            key = key.ToUpper();
            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    int shift = key[keyIndex % key.Length] - 'A';
                    result += (char)((((c - baseChar) - shift + 26) % 26) + baseChar);
                    keyIndex++;
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }
    }
}
