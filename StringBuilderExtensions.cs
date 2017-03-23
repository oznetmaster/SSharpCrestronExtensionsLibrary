using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace System.Text
	{
	public static class StringBuilderExtensions
		{
		public static StringBuilder Insert (this StringBuilder sb, int index, char value)
			{
			return sb.Insert (index, new char[] { value });
			}
		}
	}