using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace System.Collections.Generic
	{
	public static class DictionaryExtensions
		{
		public static bool TryAdd<TKey, TValue> (this IDictionary<TKey, TValue> dict, TKey key, TValue value)
			{
			if (dict.ContainsKey (key))
				return false;
			dict.Add (key, value);
			return true;
			}
		}
	}