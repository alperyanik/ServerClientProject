using System;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class PolybiusCipher
    {
        
        private static char[,] GenerateGrid(string key)
        {
            char[,] grid = new char[5, 5];
            string defaultKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            
            string tempKey = (key + defaultKey).ToUpper().Replace("J", "I");
            string builtKey = "";

            
            foreach (char c in tempKey)
            {
                if (char.IsLetter(c) && !builtKey.Contains(c.ToString()))
                {
                    builtKey += c;
                }
            }

            
            int index = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (index < builtKey.Length)
                        grid[row, col] = builtKey[index++];
                }
            }
            return grid;
        }

        public static string Encrypt(string input, string key)
        {
            char[,] grid = GenerateGrid(key);
            input = input.ToUpper().Replace("J", "I");
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    
                    for (int row = 0; row < 5; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (grid[row, col] == c)
                            {
                                
                                sb.Append((row + 1).ToString());
                                sb.Append((col + 1).ToString());
                            }
                        }
                    }
                }
               
            }
            return sb.ToString();
        }

        public static string Decrypt(string input, string key)
        {
            char[,] grid = GenerateGrid(key);
            StringBuilder sb = new StringBuilder();

           
            string numbers = "";
            foreach (char c in input)
            {
                if (char.IsDigit(c)) numbers += c;
            }


            for (int i = 0; i < numbers.Length; i += 2)
            {
                if (i + 1 < numbers.Length)
                {
                    int row = numbers[i] - '0';     
                    int col = numbers[i + 1] - '0'; 

                    
                    if (row >= 1 && row <= 5 && col >= 1 && col <= 5)
                    {
                        sb.Append(grid[row - 1, col - 1]);
                    }
                }
            }
            return sb.ToString();
        }
    }
}