using System;
using System.Linq;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class ColumnarTranspositionCipher
    {
        private static int[] GetColumnOrder(string key)
        {
            
            return key.Select((c, i) => new { Char = char.ToUpperInvariant(c), Index = i })
                      .OrderBy(x => x.Char)
                      .ThenBy(x => x.Index)
                      .Select(x => x.Index)
                      .ToArray();
        }

        public static string Encrypt(string input, string key)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(key)) return input;

            int columns = key.Length;
            
            int rows = (int)Math.Ceiling((double)input.Length / columns);
            int totalLen = rows * columns;
            
            input = input.PadRight(totalLen, 'X');

            int[] order = GetColumnOrder(key);
            StringBuilder sb = new StringBuilder();

            foreach (int colIndex in order)
            {
                for (int r = 0; r < rows; r++)
                {
                    int index = r * columns + colIndex;
                    sb.Append(input[index]);
                }
            }

            return sb.ToString();
        }

        public static string Decrypt(string input, string key)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(key)) return input;

            int columns = key.Length;
            int rows = input.Length / columns;
            if (input.Length % columns != 0) rows++;

            char[,] grid = new char[rows, columns];

            int[] order = GetColumnOrder(key);
            int index = 0;

            foreach (int colIndex in order)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (index < input.Length)
                        grid[r, colIndex] = input[index++];
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
