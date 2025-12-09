using System;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class SubstitutionCipher
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string Encrypt(string text, string key)
        {
            if (key.Length != 26) return text;

            key = key.ToUpper();
            StringBuilder result = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    bool isUpper = char.IsUpper(c);
                    char baseChar = char.ToUpper(c);
                    int index = Alphabet.IndexOf(baseChar);

                    if (index != -1)
                    {
                        char newChar = key[index];
                        result.Append(isUpper ? newChar : char.ToLower(newChar));
                    }
                    else
                    {
                        result.Append(c);
                    }
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
            if (key.Length != 26) return text;

            key = key.ToUpper();
            StringBuilder result = new StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    bool isUpper = char.IsUpper(c);
                    char baseChar = char.ToUpper(c);
                    int index = key.IndexOf(baseChar);

                    if (index != -1)
                    {
                        char originalChar = Alphabet[index];
                        result.Append(isUpper ? originalChar : char.ToLower(originalChar));
                    }
                    else
                    {
                        result.Append(c);
                    }
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