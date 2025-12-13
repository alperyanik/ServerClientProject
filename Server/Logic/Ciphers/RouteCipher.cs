using System;
using System.Text;

namespace Server.Logic.Ciphers 
{
    public static class RouteCipher
    {
        public static string Encrypt(string input, int columns)
        {
            if (string.IsNullOrEmpty(input) || columns <= 0) return input;

            
            input = input.Replace(" ", "");

            
            int rows = (int)Math.Ceiling((double)input.Length / columns);

            
            int totalLen = rows * columns;
            input = input.PadRight(totalLen, 'X');

            char[,] grid = new char[rows, columns];
            int index = 0;

            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    grid[r, c] = input[index++];
                }
            }

            
            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    sb.Append(grid[r, c]);
                }
            }

            return sb.ToString();
        }

        public static string Decrypt(string input, int columns)
        {
            if (string.IsNullOrEmpty(input) || columns <= 0) return input;

            int rows = input.Length / columns;
            char[,] grid = new char[rows, columns];
            int index = 0;

           
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (index < input.Length)
                        grid[r, c] = input[index++];
                }
            }

            
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    sb.Append(grid[r, c]);
                }
            }

            
            return sb.ToString().TrimEnd('X');
        }
    }
}