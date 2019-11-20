using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

namespace System
	{
	public static class EnumExtensions
		{
		public static bool HasFlag (this Enum en, Enum flag)
			{
			var et = en.GetType ();
			var ft = flag.GetType ();

			if (et != ft)
				throw new ArgumentException ("different enum types");

			if (Marshal.SizeOf (et) == 8)
				{
				var f64 = Convert.ToUInt64 (flag);
				return (Convert.ToUInt64 (en) & f64) == f64;
				}

			var f32 = Convert.ToUInt32 (flag);
			return (Convert.ToUInt32 (en) & f32) == f32;
			}

		public static bool HasFlag<TEnum> (this TEnum en, TEnum flag) where TEnum : struct, IConvertible, IFormattable, IComparable
			{
			var ten = typeof (TEnum);
			if (!ten.IsEnum)
				throw new ArgumentException ("value being tested must be an enum");

			if (Marshal.SizeOf (ten) == 8)
				{
				var f64 = Convert.ToUInt64 (flag);
				return (Convert.ToUInt64 (en) & f64) == f64;
				}

			var f32 = Convert.ToUInt32 (flag);
			return (Convert.ToUInt32 (en) & f32) == f32;
			}

		public static bool HasAllFlags<TEnum> (this TEnum en, params TEnum[] flags) where TEnum : struct, IConvertible, IFormattable, IComparable
			{
			var ten = typeof (TEnum);
			if (!ten.IsEnum)
				throw new ArgumentException ("value being tested must be an enum");

			if (Marshal.SizeOf (ten) == 8)
				{
				var f64 = flags.Aggregate ((ulong)0, (a, f) => a |= Convert.ToUInt64 (f));
				return (Convert.ToUInt64 (en) & f64) == f64;
				}

			var f32 = flags.Aggregate ((uint)0, (a, f) => a |= Convert.ToUInt32 (f));
			return (Convert.ToUInt32 (en) & f32) == f32;
			}

		public static bool HasAnyFlag<TEnum> (this TEnum en, params TEnum[] flags) where TEnum : struct, IConvertible, IFormattable, IComparable
			{
			var ten = typeof (TEnum);
			if (!ten.IsEnum)
				throw new ArgumentException ("value being tested must be an enum");

			if (Marshal.SizeOf (ten) == 8)
				{
				var f64 = flags.Aggregate ((ulong)0, (a, f) => a |= Convert.ToUInt64 (f));
				return (Convert.ToUInt64 (en) & f64) != 0;
				}

			var f32 = flags.Aggregate ((uint)0, (a, f) => a |= Convert.ToUInt32 (f));
			return (Convert.ToUInt32 (en) & f32) != 0;
			}
		}
	}