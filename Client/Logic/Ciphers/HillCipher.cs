using System;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class HillCipher
    {

        private static int Mod(int a, int m)
        {
            return (a % m + m) % m;
        }


        private static int ModInverse(int a, int m)
        {
            a = Mod(a, m);
            for (int x = 1; x < m; x++)
                if (Mod(a * x, m) == 1)
                    return x;
            return -1;
        }


        private static int[,] GenerateKeyMatrix(string key, int n)
        {
            int[,] matrix = new int[n, n];
            int k = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = key[k++] - 'A';
            return matrix;
        }


        private static int GetDeterminant(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1) return matrix[0, 0];
            if (n == 2) return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);

            int det = 0;
            for (int c = 0; c < n; c++)
            {
                int sign = (c % 2 == 0) ? 1 : -1;
                det += sign * matrix[0, c] * GetDeterminant(GetMinor(matrix, 0, c));
            }
            return det;
        }


        private static int[,] GetMinor(int[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int[,] minor = new int[n - 1, n - 1];
            int r = 0, c = 0;

            for (int i = 0; i < n; i++)
            {
                if (i == row) continue;
                c = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == col) continue;
                    minor[r, c] = matrix[i, j];
                    c++;
                }
                r++;
            }
            return minor;
        }


        private static int[,] GetInverseMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int det = GetDeterminant(matrix);
            int detInv = ModInverse(det, 26);

            if (detInv == -1) throw new Exception("Anahtarın tersi yok (Determinant geçersiz).");

            int[,] adjugate = new int[n, n];


            if (n == 2)
            {
                adjugate[0, 0] = matrix[1, 1];
                adjugate[0, 1] = -matrix[0, 1];
                adjugate[1, 0] = -matrix[1, 0];
                adjugate[1, 1] = matrix[0, 0];
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        int sign = ((i + j) % 2 == 0) ? 1 : -1;

                        adjugate[j, i] = sign * GetDeterminant(GetMinor(matrix, i, j));
                    }
                }
            }


            int[,] inverse = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    inverse[i, j] = Mod(adjugate[i, j] * detInv, 26);

            return inverse;
        }

        public static string Encrypt(string input, string key)
        {
            input = input.ToUpper().Replace(" ", "");
            key = key.ToUpper();


            int n = (int)Math.Sqrt(key.Length);


            while (input.Length % n != 0) input += 'X';

            int[,] keyMatrix = GenerateKeyMatrix(key, n);
            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < input.Length; i += n)
            {
                int[] vector = new int[n];
                for (int j = 0; j < n; j++) vector[j] = input[i + j] - 'A';


                for (int r = 0; r < n; r++)
                {
                    int sum = 0;
                    for (int c = 0; c < n; c++)
                        sum += keyMatrix[r, c] * vector[c];

                    sb.Append((char)(Mod(sum, 26) + 'A'));
                }
            }
            return sb.ToString();
        }

        public static string Decrypt(string input, string key)
        {
            key = key.ToUpper();
            int n = (int)Math.Sqrt(key.Length);

            int[,] keyMatrix = GenerateKeyMatrix(key, n);
            int[,] invMatrix;

            try
            {
                invMatrix = GetInverseMatrix(keyMatrix);
            }
            catch
            {
                return "[Hata: Bu anahtar matematiksel olarak çözülemez!]";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < input.Length; i += n)
            {
                int[] vector = new int[n];
                for (int j = 0; j < n; j++) vector[j] = input[i + j] - 'A';


                for (int r = 0; r < n; r++)
                {
                    int sum = 0;
                    for (int c = 0; c < n; c++)
                        sum += invMatrix[r, c] * vector[c];

                    sb.Append((char)(Mod(sum, 26) + 'A'));
                }
            }
            return sb.ToString().TrimEnd('X');
        }


        public static bool IsKeyValid(string key)
        {
            double sqrt = Math.Sqrt(key.Length);
            if (sqrt % 1 != 0) return false;

            int n = (int)sqrt;
            try
            {
                int[,] m = GenerateKeyMatrix(key.ToUpper(), n);
                int det = GetDeterminant(m);
                return ModInverse(det, 26) != -1;
            }
            catch { return false; }
        }
    }
}