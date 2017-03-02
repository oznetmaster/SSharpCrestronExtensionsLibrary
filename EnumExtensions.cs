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

			var ut = Enum.GetUnderlyingType (et);
			if (Marshal.SizeOf (ut) == 64)
				{
				var f64 = Convert.ToUInt64 (flag);
				return (Convert.ToUInt64 (en) & f64) == f64;
				}

			var f32 = Convert.ToUInt32 (flag);
			return (Convert.ToUInt32 (en) & f32) == f32;
			}
		}
	}