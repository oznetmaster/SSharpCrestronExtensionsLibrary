using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace System
	{
	public static class Volatile
		{
		public static T Read<T> (ref T value)
			{
			return value;
			}

		public static void Write<T> (ref T address, T value)
			{
			address = value;
			}
		}
	}