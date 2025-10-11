using System;

namespace Client.Logic.Ciphers
{
    public static class CaesarCipher
    {
        public static string Encrypt(string text, int key)
        {
            string result = "";
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    result += (char)((((c - baseChar) + key) % 26) + baseChar);
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }

        public static string Decrypt(string text, int key)
        {
            return Encrypt(text, 26 - key % 26);
        }
    }
}
