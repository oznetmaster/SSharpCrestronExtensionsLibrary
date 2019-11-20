using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System
	{
	public static class StringExtensions
		{
		public static String[] Split (this string s, char[] separator, StringSplitOptions options)
			{
			return s.Split (separator, Int32.MaxValue, options);
			}

		public static String[] Split (this string s, char[] separator, int count)
			{
			return s.Split (separator, count, StringSplitOptions.None);
			}

		public static String[] Split (this string s, char[] separator, int count, StringSplitOptions options)
			{
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Count cannot be less than zero.");
			if ((options != StringSplitOptions.None) && (options != StringSplitOptions.RemoveEmptyEntries))
				throw new ArgumentException ("Illegal enum value: " + options + ".");

			if (s.Length == 0 && (options & StringSplitOptions.RemoveEmptyEntries) != 0)
				return ArrayEx.Empty<string> ();

			if (count <= 1)
				{
				return count == 0
					? ArrayEx.Empty<string> ()
					: new[] {s};
				}

			return s.SplitByCharacters (separator, count, options != 0);
			}

		public static String[] Split (this string s, string[] separator, StringSplitOptions options)
			{
			return s.Split (separator, Int32.MaxValue, options);
			}

		public static String[] Split (this string s, string[] separator, int count, StringSplitOptions options)
			{
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Count cannot be less than zero.");
			if ((options != StringSplitOptions.None) && (options != StringSplitOptions.RemoveEmptyEntries))
				throw new ArgumentException ("Illegal enum value: " + options + ".");

			if (count <= 1)
				{
				return count == 0
					? ArrayEx.Empty<string> ()
					: new[] {s};
				}

			bool removeEmpty = (options & StringSplitOptions.RemoveEmptyEntries) != 0;

			if (separator == null || separator.Length == 0)
				return s.SplitByCharacters (null, count, removeEmpty);

			if (s.Length == 0 && removeEmpty)
				return ArrayEx.Empty<string> ();

			var arr = new List<String> ();

			int pos = 0;
			int matchCount = 0;
			while (pos < s.Length)
				{
				int matchIndex = -1;
				int matchPos = Int32.MaxValue;

				// Find the first position where any of the separators matches
				for (int i = 0; i < separator.Length; ++i)
					{
					string sep = separator[i];
					if (string.IsNullOrEmpty (sep))
						continue;

					int match = s.IndexOfOrdinalUnchecked (sep, pos, s.Length - pos);
					if (match >= 0 && match < matchPos)
						{
						matchIndex = i;
						matchPos = match;
						}
					}

				if (matchIndex == -1)
					break;

				if (!(matchPos == pos && removeEmpty))
					{
					if (arr.Count == count - 1)
						break;
					arr.Add (s.Substring (pos, matchPos - pos));
					}

				pos = matchPos + separator[matchIndex].Length;

				matchCount++;
				}

			if (matchCount == 0)
				return new[] {s};

			// string contained only separators
			if (removeEmpty && matchCount != 0 && pos == s.Length && arr.Count == 0)
				return ArrayEx.Empty<string> ();

			if (!(removeEmpty && pos == s.Length))
				arr.Add (s.Substring (pos));

			return arr.ToArray ();
			}

		private static readonly char[] WhiteChars =
			{
			(char)0x9, (char)0xA, (char)0xB, (char)0xC, (char)0xD,
			(char)0x85, (char)0x1680, (char)0x2028, (char)0x2029,
			(char)0x20, (char)0xA0, (char)0x2000, (char)0x2001, (char)0x2002, (char)0x2003, (char)0x2004,
			(char)0x2005, (char)0x2006, (char)0x2007, (char)0x2008, (char)0x2009, (char)0x200A, (char)0x200B,
			(char)0x3000, (char)0xFEFF
			};

#if ALLOWUNSAFE
		unsafe static string[] SplitByCharacters (this string s, char[] sep, int count, bool removeEmpty)
			{
			if (sep == null || sep.Length == 0)
				sep = WhiteChars;

			int[] split_points = null;
			int total_points = 0;
			--count;

			if (sep == null || sep.Length == 0)
				{
				fixed (char* src = s)
					{
					char* src_ptr = src;
					int len = s.Length;

					while (len > 0)
						{
						if (char.IsWhiteSpace (*src_ptr++))
							{
							if (split_points == null)
								{
								split_points = new int[8];
								}
							else if (split_points.Length == total_points)
								{
								Array.Resize (ref split_points, split_points.Length * 2);
								}

							split_points[total_points++] = s.Length - len;
							if (total_points == count && !removeEmpty)
								break;
							}
						--len;
						}
					}
				}
			else
				{
				fixed (char* src = s)
					{
					fixed (char* sep_src = sep)
						{
						char* src_ptr = src;
						char* sep_ptr_end = sep_src + sep.Length;
						int len = s.Length;
						while (len > 0)
							{
							char* sep_ptr = sep_src;
							do
								{
								if (*sep_ptr++ == *src_ptr)
									{
									if (split_points == null)
										{
										split_points = new int[8];
										}
									else if (split_points.Length == total_points)
										{
										Array.Resize (ref split_points, split_points.Length * 2);
										}

									split_points[total_points++] = s.Length - len;
									if (total_points == count && !removeEmpty)
										len = 0;

									break;
									}
								} while (sep_ptr != sep_ptr_end);

							++src_ptr;
							--len;
							}
						}
					}
				}

			if (total_points == 0)
				return new string[] { s };

			var res = new string[Math.Min (total_points, count) + 1];
			int prev_index = 0;
			int i = 0;
			if (!removeEmpty)
				{
				for (; i < total_points; ++i)
					{
					var start = split_points[i];
					res[i] = s.SubstringUnchecked (prev_index, start - prev_index);
					prev_index = start + 1;
					}

				res[i] = s.SubstringUnchecked (prev_index, s.Length - prev_index);
				}
			else
				{
				int used = 0;
				int length;
				for (; i < total_points; ++i)
					{
					var start = split_points[i];
					length = start - prev_index;
					if (length != 0)
						{
						if (used == count)
							break;

						res[used++] = s.SubstringUnchecked (prev_index, length);
						}

					prev_index = start + 1;
					}

				length = s.Length - prev_index;
				if (length != 0)
					res[used++] = s.SubstringUnchecked (prev_index, length);

				if (used != res.Length)
					Array.Resize (ref res, used);
				}

			return res;
			}
#else
		private static string[] SplitByCharacters (this string s, char[] sep, int count, bool removeEmpty)
			{
			if (sep == null || sep.Length == 0)
				sep = WhiteChars;

			int[] split_points = null;
			int total_points = 0;
			--count;

			if (sep == null || sep.Length == 0)
				{
				const int srcIx = 0;
					{
					int src_ptrIx = srcIx;
					int len = s.Length;

					while (len > 0)
						{
						if (char.IsWhiteSpace (s[src_ptrIx++]))
							{
							if (split_points == null)
								{
								split_points = new int[8];
								}
							else if (split_points.Length == total_points)
								{
								Array.Resize (ref split_points, split_points.Length * 2);
								}

							split_points[total_points++] = s.Length - len;
							if (total_points == count && !removeEmpty)
								break;
							}
						--len;
						}
					}
				}
			else
				{
				const int srcIx = 0;
					{
					const int sep_srcIx = 0;
						{
						int src_ptrIx = srcIx;
						int sep_ptr_endIx = sep_srcIx + sep.Length;
						int len = s.Length;
						while (len > 0)
							{
							int sep_ptrIx = sep_srcIx;
							do
								{
								if (sep[sep_ptrIx++] == s[src_ptrIx])
									{
									if (split_points == null)
										{
										split_points = new int[8];
										}
									else if (split_points.Length == total_points)
										{
										Array.Resize (ref split_points, split_points.Length * 2);
										}

									split_points[total_points++] = s.Length - len;
									if (total_points == count && !removeEmpty)
										len = 0;

									break;
									}
								} while (sep_ptrIx != sep_ptr_endIx);

							++src_ptrIx;
							--len;
							}
						}
					}
				}

			if (total_points == 0)
				return new[] {s};

			var res = new string[Math.Min (total_points, count) + 1];
			int prev_index = 0;
			int i = 0;
			if (!removeEmpty)
				{
				for (; i < total_points; ++i)
					{
					var start = split_points[i];
					res[i] = s.SubstringUnchecked (prev_index, start - prev_index);
					prev_index = start + 1;
					}

				res[i] = s.SubstringUnchecked (prev_index, s.Length - prev_index);
				}
			else
				{
				int used = 0;
				int length;
				for (; i < total_points; ++i)
					{
					var start = split_points[i];
					length = start - prev_index;
					if (length != 0)
						{
						if (used == count)
							break;

						res[used++] = s.SubstringUnchecked (prev_index, length);
						}

					prev_index = start + 1;
					}

				length = s.Length - prev_index;
				if (length != 0)
					res[used++] = s.SubstringUnchecked (prev_index, length);

				if (used != res.Length)
					Array.Resize (ref res, used);
				}

			return res;
			}
#endif

		internal static String SubstringUnchecked (this string s, int startIndex, int length)
			{
			return s.Substring (startIndex, length);
			/*
			if (length == 0)
				return String.Empty;

			string tmp = InternalAllocateStr (length);
			fixed (char* dest = tmp, src = s)
				{
				CharCopy (dest, src + startIndex, length);
				}
			return tmp;
			*/
			}

		internal static int IndexOfOrdinalUnchecked (this string s, string value)
			{
			return s.IndexOfOrdinalUnchecked (value, 0, s.Length);
			}

#if ALLOWUNSAFE
		internal static unsafe int IndexOfOrdinalUnchecked (this string s, string value, int startIndex, int count)
			{
			int valueLen = value.Length;
			if (count < valueLen)
				return -1;

			if (valueLen <= 1)
				{
				if (valueLen == 1)
					return s.IndexOfUnchecked (value[0], startIndex, count);
				return startIndex;
				}

			fixed (char* thisptr = s, valueptr = value)
				{
				char* ap = thisptr + startIndex;
				char* thisEnd = ap + count - valueLen + 1;
				while (ap != thisEnd)
					{
					if (*ap == *valueptr)
						{
						for (int i = 1; i < valueLen; i++)
							{
							if (ap[i] != valueptr[i])
								goto NextVal;
							}
						return (int)(ap - thisptr);
						}
				NextVal:
					ap++;
					}
				}
			return -1;
			}

		internal static unsafe int IndexOfUnchecked (this string s, char value, int startIndex, int count)
			{
			// It helps JIT compiler to optimize comparison
			int value_32 = (int)value;

			fixed (char* start = s)
				{
				char* ptr = start + startIndex;
				char* end_ptr = ptr + (count >> 3 << 3);

				while (ptr != end_ptr)
					{
					if (*ptr == value_32)
						return (int)(ptr - start);
					if (ptr[1] == value_32)
						return (int)(ptr - start + 1);
					if (ptr[2] == value_32)
						return (int)(ptr - start + 2);
					if (ptr[3] == value_32)
						return (int)(ptr - start + 3);
					if (ptr[4] == value_32)
						return (int)(ptr - start + 4);
					if (ptr[5] == value_32)
						return (int)(ptr - start + 5);
					if (ptr[6] == value_32)
						return (int)(ptr - start + 6);
					if (ptr[7] == value_32)
						return (int)(ptr - start + 7);

					ptr += 8;
					}

				end_ptr += count & 0x07;
				while (ptr != end_ptr)
					{
					if (*ptr == value_32)
						return (int)(ptr - start);

					ptr++;
					}
				return -1;
				}
			}
#else
		internal static int IndexOfOrdinalUnchecked (this string s, string value, int startIndex, int count)
			{
			int valueLen = value.Length;
			if (count < valueLen)
				return -1;

			if (valueLen <= 1)
				{
				if (valueLen == 1)
					return s.IndexOfUnchecked (value[0], startIndex, count);
				return startIndex;
				}

			const int thisptrIx = 0;
			const int valueptrIx = 0;
				{
				int apIx = thisptrIx + startIndex;
				int thisEndIx = apIx + count - valueLen + 1;
				while (apIx != thisEndIx)
					{
					if (s[apIx] == value[valueptrIx])
						{
						for (int i = 1; i < valueLen; i++)
							{
							if (s[apIx + i] != value[valueptrIx + i])
								goto NextVal;
							}
						return apIx - thisptrIx;
						}
					NextVal:
					apIx++;
					}
				}
			return -1;
			}

		internal static int IndexOfUnchecked (this string s, char value, int startIndex, int count)
			{
			// It helps JIT compiler to optimize comparison
			int value_32 = value;

			const int startIx = 0;
				{
				int ptrIx = startIx + startIndex;
				int end_ptrIx = ptrIx + (count >> 3 << 3);

				while (ptrIx != end_ptrIx)
					{
					if (s[ptrIx] == value_32)
						return ptrIx - startIx;
					if (s[ptrIx + 1] == value_32)
						return ptrIx - startIx + 1;
					if (s[ptrIx + 2] == value_32)
						return ptrIx - startIx + 2;
					if (s[ptrIx + 3] == value_32)
						return ptrIx - startIx + 3;
					if (s[ptrIx + 4] == value_32)
						return ptrIx - startIx + 4;
					if (s[ptrIx + 5] == value_32)
						return ptrIx - startIx + 5;
					if (s[ptrIx + 6] == value_32)
						return ptrIx - startIx + 6;
					if (s[ptrIx + 7] == value_32)
						return ptrIx - startIx + 7;

					ptrIx += 8;
					}

				end_ptrIx += count & 0x07;
				while (ptrIx != end_ptrIx)
					{
					if (s[ptrIx] == value_32)
						return ptrIx - startIx;

					ptrIx++;
					}
				return -1;
				}
			}
#endif

		public static string ToLowerInvariant (this string str)
			{
			return str.ToLower (CultureInfo.InvariantCulture);
			}

		public static string ToUpperInvariant (this string str)
			{
			return str.ToUpper (CultureInfo.InvariantCulture);
			}

		public static string Remove (this string str, int startIndex)
			{
			if (startIndex >= str.Length || startIndex < 0)
				throw new ArgumentOutOfRangeException ("startIndex");

			return str.Remove (startIndex, str.Length - startIndex);
			}

		private static readonly byte[] LookupTable =
			{
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
			0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
			};

		private static byte Lookup (char c)
			{
			if (c > 255)
				throw new ArgumentOutOfRangeException ("c", "Not ANSII character");
			var b = LookupTable[c];
			if (b == 255)
				throw new ArgumentOutOfRangeException ("c", "Expected a hex character, got " + c);
			return b;
			}

		public static byte HexToByte (this char[] chars, int offset)
			{
			return (byte)(Lookup (chars[offset]) << 4 | Lookup (chars[offset + 1]));
			}

		public static byte[] HexToBytes (this string str)
			{
			if (str == null)
				throw new ArgumentNullException ("str");

			var ca = str.ToCharArray ();
			var cl = ca.Length;

			if ((cl & 1) != 0)
				throw new ArgumentException ("must be even length", "str");

			var ba = new byte[cl >> 1];

			for (var i = 0; i < cl; i += 2)
				ba[i >> 1] = (byte)(Lookup (ca[i]) << 4 | Lookup (ca[i + 1]));

			return ba;
			}
		}

	public static class StringEx
		{
		public static bool IsNullOrWhiteSpace (string str)
			{
			if (string.IsNullOrEmpty (str))
				return true;
			return str.All (Char.IsWhiteSpace);
			}

		public static string Join<T> (string separator, IEnumerable<T> values)
			{
			if (separator == null)
				return Concat (values);

			if (values == null)
				throw new ArgumentNullException ("values");

			var stringList = values as IList<T> ?? new List<T> (values);
			var strCopy = new string[stringList.Count];
			int i = 0;
			foreach (var v in stringList)
				strCopy[i++] = v.ToString ();

			return String.Join (separator, strCopy);
			}

		public static string Join (string separator, params object[] values)
			{
			if (separator == null)
				return Concat (values);

			if (values == null)
				throw new ArgumentNullException ("values");

			var strCopy = new string[values.Length];
			int i = 0;
			foreach (var v in values)
				strCopy[i++] = v.ToString ();

			return String.Join (separator, strCopy);
			}

		public static string Join (string separator, IEnumerable<string> values)
			{
			if (separator == null)
				return Concat (values);

			if (values == null)
				throw new ArgumentNullException ("values");

			var stringList = new List<string> (values);

			return String.Join (separator, stringList.ToArray ());
			}

		public static string Concat<T> (IEnumerable<T> values)
			{
			if (values == null)
				throw new ArgumentNullException ("values");

			var stringList = new List<string> ();
			int len = 0;
			foreach (var v in values)
				{
				string sr = v.ToString ();
				len += sr.Length;
				if (len < 0)
					throw new OutOfMemoryException ();
				stringList.Add (sr);
				}
			return String.Concat (stringList.ToArray ());
			}

		public static string Concat (IEnumerable<string> values)
			{
			if (values == null)
				throw new ArgumentNullException ("values");

			var stringList = new List<string> ();
			int len = 0;
			foreach (var v in values)
				{
				if (v == null)
					continue;
				len += v.Length;
				if (len < 0)
					throw new OutOfMemoryException ();
				stringList.Add (v);
				}
			return String.Concat (stringList.ToArray ());
			}
		}
	}