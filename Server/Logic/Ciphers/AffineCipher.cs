using System;
using System.Text;

namespace Server.Logic.Ciphers
{
    public static class AffineCipher
    {
        // 26 ile aralarında asal olup olmadığını kontrol eder
        private static bool IsCoprime(int a)
        {
            return GCD(a, 26) == 1;
        }

        // En büyük ortak bölen (EBOB) hesaplar
        private static int GCD(int a, int b)
        {
            while (b != 0) { int temp = b; b = a % b; a = temp; }
            return a;
        }

        // Modüler Tersini Bulur (Decryption için gerekli: a^-1 mod 26)
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
                    // Formül: E(x) = (ax + b) mod 26
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

            int aInverse = ModInverse(a); // a'nın tersini bul
            StringBuilder sb = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int y = c - offset;

                    // Formül: D(x) = a^-1 * (y - b) mod 26
                    // C#'ta negatif modül hatasını önlemek için +26 ekliyoruz
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