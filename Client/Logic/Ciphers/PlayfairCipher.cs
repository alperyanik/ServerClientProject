using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class PlayfairCipher
    {
        private static char[,] GenerateKeySquare(string key)
        {
            char[,] keySquare = new char[5, 5];
            string defaultKey = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; 
            string tempKey = (key + defaultKey).ToUpper().Replace("J", "I");
            string builtKey = "";

           
            foreach (char c in tempKey)
            {
                if (char.IsLetter(c) && !builtKey.Contains(c))
                    builtKey += c;
            }

            
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    keySquare[i, j] = builtKey[index++];
                }
            }
            return keySquare;
        }

        private static void GetPosition(char[,] keySquare, char ch, out int row, out int col)
        {
            if (ch == 'J') ch = 'I';
            row = 0; col = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (keySquare[i, j] == ch)
                    {
                        row = i; col = j;
                        return;
                    }
                }
            }
        }

        private static string PrepareText(string input, bool encrypt)
        {
            string sb = input.ToUpper().Replace("J", "I");
            string clean = "";
            foreach (char c in sb) if (char.IsLetter(c)) clean += c;

            if (!encrypt) return clean; 

           
            string processed = "";
            for (int i = 0; i < clean.Length; i++)
            {
                processed += clean[i];
                if (i + 1 < clean.Length && clean[i] == clean[i + 1])
                    processed += 'X';
            }

            
            if (processed.Length % 2 != 0) processed += 'X';
            return processed;
        }

        public static string Cipher(string input, string key, bool encrypt)
        {
            char[,] keySquare = GenerateKeySquare(key);
            string text = PrepareText(input, encrypt);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i += 2)
            {
                char a = text[i];
                char b = text[i + 1];
                int r1, c1, r2, c2;
                GetPosition(keySquare, a, out r1, out c1);
                GetPosition(keySquare, b, out r2, out c2);

                if (r1 == r2) 
                {
                    c1 = (c1 + (encrypt ? 1 : 4)) % 5;
                    c2 = (c2 + (encrypt ? 1 : 4)) % 5;
                }
                else if (c1 == c2) 
                {
                    r1 = (r1 + (encrypt ? 1 : 4)) % 5;
                    r2 = (r2 + (encrypt ? 1 : 4)) % 5;
                }
                else 
                {
                    int temp = c1;
                    c1 = c2;
                    c2 = temp;
                }
                result.Append(keySquare[r1, c1]);
                result.Append(keySquare[r2, c2]);
            }
            return result.ToString();
        }
    }
}