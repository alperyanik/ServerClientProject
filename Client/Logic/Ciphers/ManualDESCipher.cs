using System;
using System.Security.Cryptography;
using System.Text;

namespace Client.Logic.Ciphers
{
    public static class ManualDESCipher
    {

        private static readonly int[] IP = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9,  1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };


        private static readonly int[] FP = {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9,  49, 17, 57, 25
        };


        private static readonly int[] E = {
            32, 1,  2,  3,  4,  5,
            4,  5,  6,  7,  8,  9,
            8,  9,  10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };


        private static readonly int[] P = {
            16, 7,  20, 21, 29, 12, 28, 17,
            1,  15, 23, 26, 5,  18, 31, 10,
            2,  8,  24, 14, 32, 27, 3,  9,
            19, 13, 30, 6,  22, 11, 4,  25
        };


        private static readonly int[] PC1 = {
            57, 49, 41, 33, 25, 17, 9,
            1,  58, 50, 42, 34, 26, 18,
            10, 2,  59, 51, 43, 35, 27,
            19, 11, 3,  60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7,  62, 54, 46, 38, 30, 22,
            14, 6,  61, 53, 45, 37, 29,
            21, 13, 5,  28, 20, 12, 4
        };


        private static readonly int[] PC2 = {
            14, 17, 11, 24, 1,  5,
            3,  28, 15, 6,  21, 10,
            23, 19, 12, 4,  26, 8,
            16, 7,  27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };


        private static readonly int[] SHIFTS = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };


        private static readonly int[,,] SBOX = {
            {
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },
            {
                {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },
            {
                {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },
            {
                {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },
            {
                {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },
            {
                {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },
            {
                {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },
            {
                {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };


        public static byte[] EncryptBlock(byte[] block, byte[] key)
        {
            ulong data = BytesToULong(block);
            ulong keyBits = BytesToULong(key);


            ulong permuted = Permute(data, IP, 64);


            uint L = (uint)(permuted >> 32);
            uint R = (uint)(permuted & 0xFFFFFFFF);


            ulong[] subKeys = GenerateSubKeys(keyBits);


            for (int i = 0; i < 16; i++)
            {
                uint temp = R;
                R = L ^ FeistelFunction(R, subKeys[i]);
                L = temp;
            }


            ulong combined = ((ulong)R << 32) | L;


            ulong result = Permute(combined, FP, 64);

            return ULongToBytes(result);
        }


        private static uint FeistelFunction(uint R, ulong subKey)
        {
            ulong expanded = Permute(R, E, 48);


            ulong xored = expanded ^ subKey;


            uint sboxOutput = 0;
            for (int i = 0; i < 8; i++)
            {
                int bits6 = (int)((xored >> (42 - i * 6)) & 0x3F);
                int row = ((bits6 & 0x20) >> 4) | (bits6 & 0x01);
                int col = (bits6 >> 1) & 0x0F;
                int sVal = SBOX[i, row, col];
                sboxOutput |= (uint)(sVal << (28 - i * 4));
            }


            uint pOutput = (uint)Permute(sboxOutput, P, 32);

            return pOutput;
        }


        private static ulong[] GenerateSubKeys(ulong key)
        {
            ulong[] subKeys = new ulong[16];


            ulong permutedKey = Permute(key, PC1, 56);


            uint C = (uint)(permutedKey >> 28);
            uint D = (uint)(permutedKey & 0x0FFFFFFF);

            for (int i = 0; i < 16; i++)
            {

                C = LeftShift28(C, SHIFTS[i]);
                D = LeftShift28(D, SHIFTS[i]);


                ulong CD = ((ulong)C << 28) | D;


                subKeys[i] = Permute(CD, PC2, 48);
            }

            return subKeys;
        }





        private static ulong Permute(ulong input, int[] table, int outputBits)
        {
            ulong output = 0;
            for (int i = 0; i < table.Length; i++)
            {
                int bitPos = table[i];
                ulong bit = (input >> (64 - bitPos)) & 1;
                output |= bit << (outputBits - 1 - i);
            }
            return output;
        }

        private static uint LeftShift28(uint val, int shift)
        {
            return ((val << shift) | (val >> (28 - shift))) & 0x0FFFFFFF;
        }

        private static ulong BytesToULong(byte[] bytes)
        {
            ulong result = 0;
            for (int i = 0; i < 8; i++)
            {
                result |= (ulong)bytes[i] << (56 - i * 8);
            }
            return result;
        }

        private static byte[] ULongToBytes(ulong val)
        {
            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = (byte)(val >> (56 - i * 8));
            }
            return bytes;
        }

        #region Public API

        /// <summary>
        /// Metni DES ile ÅŸifreler (CBC modu, PKCS7 padding, dinamik IV)
        /// </summary>
        public static string Encrypt(string plainText, byte[] key)
        {
            if (key.Length != 8)
                throw new ArgumentException("DES anahtarÄ± 8 byte olmalÄ±dÄ±r.");

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] iv = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }

            int paddingLen = 8 - (plainBytes.Length % 8);
            byte[] padded = new byte[plainBytes.Length + paddingLen];
            Array.Copy(plainBytes, padded, plainBytes.Length);
            for (int i = plainBytes.Length; i < padded.Length; i++)
                padded[i] = (byte)paddingLen;

            byte[] encrypted = new byte[padded.Length];
            byte[] previousBlock = iv;
            
            for (int i = 0; i < padded.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(padded, i, block, 0, 8);
                
                for (int j = 0; j < 8; j++)
                    block[j] ^= previousBlock[j];
                
                byte[] encBlock = EncryptBlock(block, key);
                Array.Copy(encBlock, 0, encrypted, i, 8);
                previousBlock = encBlock;
            }

            byte[] result = new byte[iv.Length + encrypted.Length];
            Array.Copy(iv, 0, result, 0, iv.Length);
            Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);
            
            return Convert.ToBase64String(result);
        }


        public static byte[] GenerateRandomKey()
        {
            byte[] key = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        #endregion
    }
}
