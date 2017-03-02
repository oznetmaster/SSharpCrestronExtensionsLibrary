using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Globalization;

namespace System
	{
	public static class CharExtensions
		{
		public static char ToUpperInvariant (this char c)
			{
			return Char.ToUpper (c, CultureInfo.InvariantCulture);
			}

		public static char ToLowerInvariant (this char c)
			{
			return Char.ToLower (c, CultureInfo.InvariantCulture);
			}

		/// <summary>
		/// Determines if a character is a digit
		/// </summary>
		/// <param name="c">character to test</param>
		/// <returns><b>true</b> if the character is a digit, otherwise <b>false</b></returns>
		public static bool IsDigit (this char c)
			{
			return char.IsDigit (c);
			}
		}

	public static class CharEx
		{
		public static char ToUpperInvariant (char c)
			{
			return Char.ToUpper (c, CultureInfo.InvariantCulture);
			}

		public static string ConvertFromUtf32 (int utf32)
			{
			if (utf32 < 0 || utf32 > 0x10FFFF)
				throw new ArgumentOutOfRangeException ("utf32", "The argument must be from 0 to 0x10FFFF.");
			if (0xD800 <= utf32 && utf32 <= 0xDFFF)
				throw new ArgumentOutOfRangeException ("utf32", "The argument must not be in surrogate pair range.");
			if (utf32 < 0x10000)
				return new string ((char)utf32, 1);
			utf32 -= 0x10000;
			return new string (
				new char[] {(char) ((utf32 >> 10) + 0xD800),
				(char) (utf32 % 0x0400 + 0xDC00)});
			}

		public static int ConvertToUtf32 (char highSurrogate, char lowSurrogate)
			{
			if (highSurrogate < 0xD800 || 0xDBFF < highSurrogate)
				throw new ArgumentOutOfRangeException ("highSurrogate");
			if (lowSurrogate < 0xDC00 || 0xDFFF < lowSurrogate)
				throw new ArgumentOutOfRangeException ("lowSurrogate");

			return 0x10000 + ((highSurrogate - 0xD800) << 10) + (lowSurrogate - 0xDC00);
			}

		public static int ConvertToUtf32 (string s, int index)
			{
			CheckParameter (s, index);

			if (!Char.IsSurrogate (s[index]))
				return s[index];
			if (!CharEx.IsHighSurrogate (s[index])
				 || index == s.Length - 1
				 || !CharEx.IsLowSurrogate (s[index + 1]))
				throw new ArgumentException (String.Format ("The string contains invalid surrogate pair character at {0}", index));
			return ConvertToUtf32 (s[index], s[index + 1]);
			}

		public static bool IsHighSurrogate (char c)
			{
			return c >= '\uD800' && c <= '\uDBFF';
			}

		public static bool IsHighSurrogate (string s, int index)
			{
			CheckParameter (s, index);

			return IsHighSurrogate (s[index]);
			}

		public static bool IsLowSurrogate (char c)
			{
			return c >= '\uDC00' && c <= '\uDFFF';
			}

		public static bool IsLowSurrogate (string s, int index)
			{
			CheckParameter (s, index);

			return IsLowSurrogate (s[index]);
			}

		private static void CheckParameter (string s, int index)
			{
			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException ("index");
			}

		}
	}