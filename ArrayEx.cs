using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Collections;
using System.Collections.ObjectModel;

namespace System
	{
	public static class ArrayEx
		{
		public static T[] Empty<T> ()
			{
			return EmptyArray<T>.Value;
			}

		internal static class EmptyArray<T>
			{
			public static readonly T[] Value = new T[0];
			}

		public static bool TrueForAll<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (match == null)
				throw new ArgumentNullException ("match");

			return array.All (t => match (t));
			}

		public static void ForEach<T> (T[] array, Action<T> action)
			{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (action == null)
				throw new ArgumentNullException ("action");

			foreach (T t in array)
				action (t);
			}

		public static TOutput[] ConvertAll<TInput, TOutput> (TInput[] array, Converter<TInput, TOutput> converter)
			{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (converter == null)
				throw new ArgumentNullException ("converter");

			var output = new TOutput[array.Length];
			for (int i = 0; i < array.Length; i++)
				output[i] = converter (array[i]);

			return output;
			}

		public static int FindLastIndex<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			return GetLastIndex (array, 0, array.Length, match);
			}

		public static int FindLastIndex<T> (T[] array, int startIndex, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ();

			if (startIndex < 0 || (uint)startIndex > (uint)array.Length)
				throw new ArgumentOutOfRangeException ("startIndex");

			if (match == null)
				throw new ArgumentNullException ("match");

			return GetLastIndex (array, 0, startIndex + 1, match);
			}

		public static int FindLastIndex<T> (T[] array, int startIndex, int count, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			if (startIndex < 0 || (uint)startIndex > (uint)array.Length)
				throw new ArgumentOutOfRangeException ("startIndex");

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count");

			if (startIndex - count + 1 < 0)
				throw new ArgumentOutOfRangeException ("count must refer to a location within the array");

			return GetLastIndex (array, startIndex - count + 1, count, match);
			}

		internal static int GetLastIndex<T> (T[] array, int startIndex, int count, Predicate<T> match)
			{
			// unlike FindLastIndex, takes regular params for search range
			for (int i = startIndex + count; i != startIndex; )
				if (match (array[--i]))
					return i;

			return -1;
			}

		public static int FindIndex<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			return GetIndex (array, 0, array.Length, match);
			}

		public static int FindIndex<T> (T[] array, int startIndex, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (startIndex < 0 || (uint)startIndex > (uint)array.Length)
				throw new ArgumentOutOfRangeException ("startIndex");

			if (match == null)
				throw new ArgumentNullException ("match");

			return GetIndex (array, startIndex, array.Length - startIndex, match);
			}

		public static int FindIndex<T> (T[] array, int startIndex, int count, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (startIndex < 0)
				throw new ArgumentOutOfRangeException ("startIndex");

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count");

			if ((uint)startIndex + (uint)count > (uint)array.Length)
				throw new ArgumentOutOfRangeException ("index and count exceed length of list");

			return GetIndex (array, startIndex, count, match);
			}

		internal static int GetIndex<T> (T[] array, int startIndex, int count, Predicate<T> match)
			{
			int end = startIndex + count;
			for (int i = startIndex; i < end; i++)
				if (match (array[i]))
					return i;

			return -1;
			}

		public static int IndexOf (Array array, object value)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			return Array.IndexOf (array, value, 0, array.Length);
			}

		public static int IndexOf (Array array, object value, int startIndex)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			return Array.IndexOf (array, value, startIndex, array.Length - startIndex);
			}

		public static void Sort (Array keys, Array items)
			{
			Array.Sort (keys, items, (IComparer)null);
			}

		public static void Sort (Array array, int index, int length)
			{
			Array.Sort (array, index, length, (IComparer)null);
			}

		public static void Sort (Array keys, Array items, int index, int length)
			{
			Array.Sort (keys, items, index, length, (IComparer)null);
			}

		public static T[] FindAll<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			int pos = 0;
			T[] d = new T[array.Length];
			foreach (T t in array)
				if (match (t))
					d[pos++] = t;

			Array.Resize<T> (ref d, pos);
			return d;
			}

		public static bool Exists<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			foreach (T t in array)
				if (match (t))
					return true;
			return false;
			}

		public static ReadOnlyCollection<T> AsReadOnly<T> (T[] array)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			return new ReadOnlyCollection<T> (array);
			}

		public static T Find<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			foreach (T t in array)
				if (match (t))
					return t;

			return default (T);
			}

		public static T FindLast<T> (T[] array, Predicate<T> match)
			{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");

			for (int i = array.Length - 1; i >= 0; i--)
				if (match (array[i]))
					return array[i];

			return default (T);
			}

		}
	}