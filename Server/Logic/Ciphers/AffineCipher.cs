using System;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class AffineCipher
    {
       
        private static bool IsCoprime(int a)
        {
            return GCD(a, 26) == 1;
        }

       
        private static int GCD(int a, int b)
        {
            while (b != 0) { int temp = b; b = a % b; a = temp; }
            return a;
        }

        
        private static int ModInverse(int a)
        {
            for (int x = 1; x < 26; x++)
            {
                if ((a * x) % 26 == 1) return x;
            }
            throw new Exception("Bu 'a' değerinin tersi yoktur (26 ile aralarında asal değil).");
        }

        public static string Encrypt(string input, int a, int b)
        {
            if (!IsCoprime(a)) return "[Hata: 'a' sayısı 26 ile aralarında asal olmalı (Örn: 1,3,5,7,9...)]";

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int x = c - offset;
                    
                    int processed = (a * x + b) % 26;
                    sb.Append((char)(processed + offset));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string Decrypt(string input, int a, int b)
        {
            if (!IsCoprime(a)) return "[Hata: Geçersiz Anahtar]";

            int aInverse = ModInverse(a); 
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int y = c - offset;

                    
                    int processed = (aInverse * (y - b)) % 26;
                    if (processed < 0) processed += 26;

                    sb.Append((char)(processed + offset));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}