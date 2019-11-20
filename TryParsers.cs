//
// <class>Ex.TryParse
//
// Author:
//	Neil Colvin
//
// (C) 2017 Nivloc Enterprises Ltd.
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;
using Crestron.SimplSharp.Reflection;

namespace System
	{
	public static class TryParser
		{
		private static readonly CType[] parseParamTypes = new CType[] { typeof (string), typeof (NumberStyles), typeof (IFormatProvider) };

		public static bool TryParse<T> (string s, out T result) where T : struct, IComparable, IComparable<T>, IFormattable, IConvertible, IEquatable<T> 
			{
			return TryParse<T> (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse<T> (string s, NumberStyles style, IFormatProvider provider, out T result) where T : struct, IComparable, IComparable<T>, IFormattable, IConvertible, IEquatable<T> 
			{
			MethodInfo parseMethod = typeof (T).GetCType ().GetMethod ("Parse", parseParamTypes);

			if (s != null)
				{
				try
					{
					if (parseMethod != null)
						{
						result = (T)parseMethod.Invoke (null, new object[] {s, style, provider});
						return true;
						}
					}
				catch (Exception)
					{
					}
				}
			result = default (T);
			return false;
			}

		public static bool IsParsable<T> (string s) where T : struct, IComparable, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
			{
			T result;

			return TryParse<T> (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable<T> (string s, NumberStyles style, IFormatProvider provider) where T : struct, IComparable, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
			{
			T result;

			return TryParse<T> (s, style, provider, out result);
			}
		}

	public static class ByteEx
		{
		public static bool TryParse (string s, out byte result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out byte result)
			{
			if (s != null)
				{
				try
					{
					result = Byte.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(byte);
			return false;
			}

		public static bool IsParsable (string s)
			{
			byte result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			byte result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class SByteEx
		{
		public static bool TryParse (string s, out sbyte result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out sbyte result)
			{
			if (s != null)
				{
				try
					{
					result = SByte.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(sbyte);
			return false;
			}

		public static bool IsParsable (string s)
			{
			sbyte result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			sbyte result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class Int16Ex
		{
		public static bool TryParse (string s, out short result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out short result)
			{
			if (s != null)
				{
				try
					{
					result = Int16.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(short);
			return false;
			}

		public static bool IsParsable (string s)
			{
			short result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			short result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class Int32Ex
		{
		public static bool TryParse (string s, out int result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out int result)
			{
			if (s != null)
				{
				try
					{
					result = Int32.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(int);
			return false;
			}

		public static bool IsParsable (string s)
			{
			int result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			int result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class Int64Ex
		{
		public static bool TryParse (string s, out long result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out long result)
			{
			if (s != null)
				{
				try
					{
					result = Int64.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(long);
			return false;
			}

		public static bool IsParsable (string s)
			{
			long result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			long result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class UInt16Ex
		{
		public static bool TryParse (string s, out ushort result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out ushort result)
			{
			if (s != null)
				{
				try
					{
					result = UInt16.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(ushort);
			return false;
			}

		public static bool IsParsable (string s)
			{
			ushort result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			ushort result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class UInt32Ex
		{
		public static bool TryParse (string s, out uint result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out uint result)
			{
			if (s != null)
				{
				try
					{
					result = UInt32.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(uint);
			return false;
			}

		public static bool IsParsable (string s)
			{
			uint result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			uint result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class UInt64Ex
		{
		public static bool TryParse (string s, out ulong result)
			{
			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out ulong result)
			{
			if (s != null)
				{
				try
					{
					result = UInt64.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(ulong);
			return false;
			}

		public static bool IsParsable (string s)
			{
			ulong result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			ulong result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class BooleanEx
		{

		public static bool TryParse (string s, out bool result)
			{
			if (s != null)
				{
				try
					{
					result = Boolean.Parse (s);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (bool);
			return false;
			}

		public static bool IsParsable (string s)
			{
			bool result;

			return TryParse (s, out result);
			}
		}

	public static class DateTimeEx
		{
		public static bool TryParse (string s, out DateTime result)
			{
			if (s != null)
				{
				try
					{
					result = DateTime.Parse (s);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(DateTime);
			return false;
			}

		public static bool TryParse (string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (s != null)
				{
				try
					{
					result = DateTime.Parse (s, provider, styles);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (DateTime);
			return false;
			}

		public static bool TryParseExact (string s, string format, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (String.IsNullOrEmpty (s) || String.IsNullOrEmpty (format))
				{
				result = default (DateTime);
				return false;
				}

			try
				{
				result = DateTime.ParseExact (s, format, provider, styles);
				return true;
				}
			catch (Exception)
				{
				}

			result = default (DateTime);
			return false;
			}

		public static bool TryParseExact (string s, string[] formats, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (String.IsNullOrEmpty (s) || formats == null)
				{
				result = default (DateTime);
				return false;
				}

			foreach (var f in formats)
				{
				try
					{
					result = DateTime.ParseExact (s, f, provider, styles);
					return true;
					}
				catch (Exception)
					{
					}
				}

			result = default (DateTime);
			return false;
			}

		public static bool IsParsable (string s)
			{
			DateTime result;

			return TryParse (s, out result);
			}

		public static bool IsParsable (string s, IFormatProvider provider, DateTimeStyles styles)
			{
			DateTime result;

			return TryParse (s, provider, styles, out result);
			}
		}

	public static class DoubleEx
		{
		public static bool TryParse (string s, out double result)
			{
			return TryParse (s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out double result)
			{
			if (s != null)
				{
				try
					{
					result = Double.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(Double);
			return false;
			}

		public static bool IsParsable (string s)
			{
			double result;

			return TryParse (s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			double result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class SingleEx
		{
		public static bool TryParse (string s, out float result)
			{
			return TryParse (s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out float result)
			{
			if (s != null)
				{
				try
					{
					result = Single.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(Single);
			return false;
			}

		public static bool IsParsable (string s)
			{
			float result;

			return TryParse (s, NumberStyles.Float, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			float result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class DecimalEx
		{
		public static bool TryParse (string s, out decimal result)
			{
			return TryParse (s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out decimal result)
			{
			if (s != null)
				{
				try
					{
					result = Decimal.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (Decimal);
			return false;
			}

		public static bool IsParsable (string s)
			{
			decimal result;

			return TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool IsParsable (string s, NumberStyles style, IFormatProvider provider)
			{
			decimal result;

			return TryParse (s, style, provider, out result);
			}
		}

	public static class TimeSpanEx
		{
		public static bool TryParse (string s, out TimeSpan result)
			{
			if (s != null)
				{
				try
					{
					result = TimeSpan.Parse (s);
					return true;
					}
				catch (Exception)
					{
					}
				}

			result = default (TimeSpan);
			return false;
			}

		public static bool IsParsable (string s)
			{
			TimeSpan result;

			return TryParse (s, out result);
			}
		}
	}