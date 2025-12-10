using System;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class RailFenceCipher
    {
        public static string Encrypt(string input, int rails)
        {
            if (string.IsNullOrEmpty(input) || rails <= 1) return input;

            char[,] fence = new char[rails, input.Length];
           
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < input.Length; j++)
                    fence[i, j] = '\n';

            bool down = false;
            int row = 0, col = 0;

            
            for (int i = 0; i < input.Length; i++)
            {
                if (row == 0 || row == rails - 1) down = !down;
                fence[row, col++] = input[i];
                row += down ? 1 : -1;
            }

            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < input.Length; j++)
                    if (fence[i, j] != '\n') sb.Append(fence[i, j]);

            return sb.ToString();
        }

        public static string Decrypt(string input, int rails)
        {
            if (string.IsNullOrEmpty(input) || rails <= 1) return input;

            char[,] fence = new char[rails, input.Length];
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < input.Length; j++)
                    fence[i, j] = '\n';

            bool down = false;
            int row = 0, col = 0;

            
            for (int i = 0; i < input.Length; i++)
            {
                if (row == 0 || row == rails - 1) down = !down;
                fence[row, col++] = '*';
                row += down ? 1 : -1;
            }

            
            int index = 0;
            for (int i = 0; i < rails; i++)
                for (int j = 0; j < input.Length; j++)
                    if (fence[i, j] == '*' && index < input.Length)
                        fence[i, j] = input[index++];

            
            StringBuilder sb = new StringBuilder();
            row = 0; col = 0;
            down = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (row == 0 || row == rails - 1) down = !down;
                if (fence[row, col] != '\n') sb.Append(fence[row, col]);
                col++;
                row += down ? 1 : -1;
            }
            return sb.ToString();
        }
    }
}