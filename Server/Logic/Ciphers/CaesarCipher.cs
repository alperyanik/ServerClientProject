using System;
using System.Text;

namespace Server.Logic.Ciphers
{
	public static class CaesarCipher
	{
		public static string Encrypt(string input, int shift)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in input)
			{
				if (char.IsLetter(c))
				{
					char baseChar = char.IsUpper(c) ? 'A' : 'a';
					sb.Append((char)(((c - baseChar + shift) % 26) + baseChar));
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		public static string Decrypt(string input, int shift)
		{
			return Encrypt(input, 26 - (shift % 26));
		}
	}
}
