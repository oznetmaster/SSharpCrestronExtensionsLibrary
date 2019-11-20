using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace System.Collections
	{
	using Environment = CrestronEnvironmentEx;

	public static class ArrayListEx
		{
		// Creates a ArrayList wrapper for a particular IList.  This does not
		// copy the contents of the IList, but only wraps the ILIst.  So any
		// changes to the underlying list will affect the ArrayList.  This would
		// be useful if you want to Reverse a subrange of an IList, or want to
		// use a generic BinarySearch or Sort method without implementing one yourself.
		// However, since these methods are generic, the performance may not be
		// nearly as good for some operations as they would be on the IList itself.
		//
		public static ArrayList Adapter (IList list)
			{
			if (list == null)
				throw new ArgumentNullException ("list");
			return new IListWrapper (list);
			}

		// This class wraps an IList, exposing it as a ArrayList
		// Note this requires reimplementing half of ArrayList...
		[Serializable]
		private class IListWrapper : ArrayList
			{
			private IList _list;
			private int _version;

			internal IListWrapper (IList list)
				{
				_list = list;
				_version = 0; // list doesn't not contain a version number
				}

			public override int Capacity
				{
				get
					{
					return _list.Count;
					}
				set
					{
					if (value < Count)
						throw new ArgumentOutOfRangeException ("value", Environment.GetResourceString ("ArgumentOutOfRange_SmallCapacity"));
					}
				}

			public override int Count
				{
				get
					{
					return _list.Count;
					}
				}

			public override bool IsReadOnly
				{
				get
					{
					return _list.IsReadOnly;
					}
				}

			public override bool IsFixedSize
				{
				get
					{
					return _list.IsFixedSize;
					}
				}


			public override bool IsSynchronized
				{
				get
					{
					return _list.IsSynchronized;
					}
				}

			public override Object this[int index]
				{
				get
					{
					return _list[index];
					}
				set
					{
					_list[index] = value;
					_version++;
					}
				}

			public override Object SyncRoot
				{
				get
					{
					return _list.SyncRoot;
					}
				}

			public override int Add (Object obj)
				{
				int i = _list.Add (obj);
				_version++;
				return i;
				}

			public override void AddRange (ICollection c)
				{
				InsertRange (Count, c);
				}

			// Other overloads with automatically work
			public override int BinarySearch (int index, int count, Object value, IComparer comparer)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (this.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));
				if (comparer == null)
					comparer = Comparer.Default;

				int lo = index;
				int hi = index + count - 1;
				int mid;
				while (lo <= hi)
					{
					mid = (lo + hi) / 2;
					int r = comparer.Compare (value, _list[mid]);
					if (r == 0)
						return mid;
					if (r < 0)
						hi = mid - 1;
					else
						lo = mid + 1;
					}
				// return bitwise complement of the first element greater than value.
				// Since hi is less than lo now, ~lo is the correct item.
				return ~lo;
				}

			public override void Clear ()
				{
				// If _list is an array, it will support Clear method.
				// We shouldn't allow clear operation on a FixedSized ArrayList
				if (_list.IsFixedSize)
					{
					throw new NotSupportedException (Environment.GetResourceString ("NotSupported_FixedSizeCollection"));
					}

				_list.Clear ();
				_version++;
				}

			public override Object Clone ()
				{
				// This does not do a shallow copy of _list into a ArrayList!
				// This clones the IListWrapper, creating another wrapper class!
				return new IListWrapper (_list);
				}

			public override bool Contains (Object obj)
				{
				return _list.Contains (obj);
				}

			public override void CopyTo (Array array, int index)
				{
				_list.CopyTo (array, index);
				}

			public override void CopyTo (int index, Array array, int arrayIndex, int count)
				{
				if (array == null)
					throw new ArgumentNullException ("array");
				if (index < 0 || arrayIndex < 0)
					throw new ArgumentOutOfRangeException ((index < 0) ? "index" : "arrayIndex", Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (count < 0)
					throw new ArgumentOutOfRangeException ("count", Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (array.Length - arrayIndex < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));
				if (array.Rank != 1)
					throw new ArgumentException (Environment.GetResourceString ("Arg_RankMultiDimNotSupported"));

				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				for (int i = index; i < index + count; i++)
					array.SetValue (_list[i], arrayIndex++);
				}

			public override IEnumerator GetEnumerator ()
				{
				return _list.GetEnumerator ();
				}

			public IEnumerator GetEnumerator (int index, int count)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				return new IListWrapperEnumWrapper (this, index, count);
				}

			public override int IndexOf (Object value)
				{
				return _list.IndexOf (value);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public int IndexOf (Object value, int startIndex)
				{
				return IndexOf (value, startIndex, _list.Count - startIndex);
				}

			public override int IndexOf (Object value, int startIndex, int count)
				{
				if (startIndex < 0 || startIndex > this.Count)
					throw new ArgumentOutOfRangeException ("startIndex", Environment.GetResourceString ("ArgumentOutOfRange_Index"));
				if (count < 0 || startIndex > this.Count - count)
					throw new ArgumentOutOfRangeException ("count", Environment.GetResourceString ("ArgumentOutOfRange_Count"));

				int endIndex = startIndex + count;
				if (value == null)
					{
					for (int i = startIndex; i < endIndex; i++)
						if (_list[i] == null)
							return i;
					return -1;
					}
				else
					{
					for (int i = startIndex; i < endIndex; i++)
						if (_list[i] != null && _list[i].Equals (value))
							return i;
					return -1;
					}
				}

			public override void Insert (int index, Object obj)
				{
				_list.Insert (index, obj);
				_version++;
				}

			public override void InsertRange (int index, ICollection c)
				{
				if (c == null)
					throw new ArgumentNullException ("c", Environment.GetResourceString ("ArgumentNull_Collection"));
				if (index < 0 || index > this.Count)
					throw new ArgumentOutOfRangeException ("index", Environment.GetResourceString ("ArgumentOutOfRange_Index"));

				if (c.Count > 0)
					{
					ArrayList al = _list as ArrayList;
					if (al != null)
						{
						// We need to special case ArrayList. 
						// When c is a range of _list, we need to handle this in a special way.
						// See ArrayList.InsertRange for details.
						al.InsertRange (index, c);
						}
					else
						{
						IEnumerator en = c.GetEnumerator ();
						while (en.MoveNext ())
							{
							_list.Insert (index++, en.Current);
							}
						}
					_version++;
					}
				}

			public int LastIndexOf (Object value)
				{
				return LastIndexOf (value, _list.Count - 1, _list.Count);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public int LastIndexOf (Object value, int startIndex)
				{
				return LastIndexOf (value, startIndex, startIndex + 1);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public int LastIndexOf (Object value, int startIndex, int count)
				{
				if (_list.Count == 0)
					return -1;

				if (startIndex < 0 || startIndex >= _list.Count)
					throw new ArgumentOutOfRangeException ("startIndex", Environment.GetResourceString ("ArgumentOutOfRange_Index"));
				if (count < 0 || count > startIndex + 1)
					throw new ArgumentOutOfRangeException ("count", Environment.GetResourceString ("ArgumentOutOfRange_Count"));

				int endIndex = startIndex - count + 1;
				if (value == null)
					{
					for (int i = startIndex; i >= endIndex; i--)
						if (_list[i] == null)
							return i;
					return -1;
					}
				else
					{
					for (int i = startIndex; i >= endIndex; i--)
						if (_list[i] != null && _list[i].Equals (value))
							return i;
					return -1;
					}
				}

			public override void Remove (Object value)
				{
				int index = IndexOf (value);
				if (index >= 0)
					RemoveAt (index);
				}

			public override void RemoveAt (int index)
				{
				_list.RemoveAt (index);
				_version++;
				}

			public override void RemoveRange (int index, int count)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				if (count > 0)    // be consistent with ArrayList
					_version++;

				while (count > 0)
					{
					_list.RemoveAt (index);
					count--;
					}
				}

			public override void Reverse (int index, int count)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				int i = index;
				int j = index + count - 1;
				while (i < j)
					{
					Object tmp = _list[i];
					_list[i++] = _list[j];
					_list[j--] = tmp;
					}
				_version++;
				}

			public void SetRange (int index, ICollection c)
				{
				if (c == null)
					{
					throw new ArgumentNullException ("c", Environment.GetResourceString ("ArgumentNull_Collection"));
					}

				if (index < 0 || index > _list.Count - c.Count)
					{
					throw new ArgumentOutOfRangeException ("index", Environment.GetResourceString ("ArgumentOutOfRange_Index"));
					}

				if (c.Count > 0)
					{
					IEnumerator en = c.GetEnumerator ();
					while (en.MoveNext ())
						{
						_list[index++] = en.Current;
						}
					_version++;
					}
				}

#if !NETCF
			public ArrayList GetRange (int index, int count)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));
				return new Range (this, index, count);
				}
#endif

			public override void Sort (int index, int count, IComparer comparer)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (_list.Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				Object[] array = new Object[count];
				CopyTo (index, array, 0, count);
				Array.Sort (array, 0, count, comparer);
				for (int i = 0; i < count; i++)
					_list[i + index] = array[i];

				_version++;
				}


			public override Object[] ToArray ()
				{
				Object[] array = new Object[Count];
				_list.CopyTo (array, 0);
				return array;
				}

			public override Array ToArray (Type type)
				{
				if (type == null)
					throw new ArgumentNullException ("type");
				Array array = Array.CreateInstance (type, _list.Count);// Array.UnsafeCreateInstance (type, _list.Count);
				_list.CopyTo (array, 0);
				return array;
				}

			public override void TrimToSize ()
				{
				// Can't really do much here...
				}

			// This is the enumerator for an IList that's been wrapped in another
			// class that implements all of ArrayList's methods.
			[Serializable]
			private sealed class IListWrapperEnumWrapper : IEnumerator, ICloneable
				{
				private IEnumerator _en;
				private int _remaining;
				private int _initialStartIndex;   // for reset
				private int _initialCount;        // for reset
				private bool _firstCall;       // firstCall to MoveNext

				private IListWrapperEnumWrapper ()
					{
					}

				internal IListWrapperEnumWrapper (IListWrapper listWrapper, int startIndex, int count)
					{
					_en = listWrapper.GetEnumerator ();
					_initialStartIndex = startIndex;
					_initialCount = count;
					while (startIndex-- > 0 && _en.MoveNext ())
						;
					_remaining = count;
					_firstCall = true;
					}

				public Object Clone ()
					{
					// We must clone the underlying enumerator, I think.
					IListWrapperEnumWrapper clone = new IListWrapperEnumWrapper ();
					clone._en = (IEnumerator)((ICloneable)_en).Clone ();
					clone._initialStartIndex = _initialStartIndex;
					clone._initialCount = _initialCount;
					clone._remaining = _remaining;
					clone._firstCall = _firstCall;
					return clone;
					}

				public bool MoveNext ()
					{
					if (_firstCall)
						{
						_firstCall = false;
						return _remaining-- > 0 && _en.MoveNext ();
						}
					if (_remaining < 0)
						return false;
					bool r = _en.MoveNext ();
					return r && _remaining-- > 0;
					}

				public Object Current
					{
					get
						{
						if (_firstCall)
							throw new InvalidOperationException (Environment.GetResourceString ("InvalidOperation_EnumNotStarted"));
						if (_remaining < 0)
							throw new InvalidOperationException (Environment.GetResourceString ("InvalidOperation_EnumEnded"));
						return _en.Current;
						}
					}

				public void Reset ()
					{
					_en.Reset ();
					int startIndex = _initialStartIndex;
					while (startIndex-- > 0 && _en.MoveNext ())
						;
					_remaining = _initialCount;
					_firstCall = true;
					}
				}
			}

		public static IList ReadOnly (IList list)
			{
			if (list == null)
				throw new ArgumentNullException ("list");
			return new ReadOnlyList (list);
			}

		// Returns a read-only ArrayList wrapper for the given ArrayList.
		//
		public static ArrayList ReadOnly (ArrayList list)
			{
			if (list == null)
				throw new ArgumentNullException ("list");
			return new ReadOnlyArrayList (list);
			}

		[Serializable]
		private class ReadOnlyArrayList : ArrayList
			{
			private ArrayList _list;

			internal ReadOnlyArrayList (ArrayList l)
				{
				_list = l;
				}

			public override int Count
				{
				get
					{
					return _list.Count;
					}
				}

			public override bool IsReadOnly
				{
				get
					{
					return true;
					}
				}

			public override bool IsFixedSize
				{
				get
					{
					return true;
					}
				}

			public override bool IsSynchronized
				{
				get
					{
					return _list.IsSynchronized;
					}
				}

			public override Object this[int index]
				{
				get
					{
					return _list[index];
					}
				set
					{
					throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
					}
				}

			public override Object SyncRoot
				{
				get
					{
					return _list.SyncRoot;
					}
				}

			public override int Add (Object obj)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public override void AddRange (ICollection c)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override int BinarySearch (int index, int count, Object value, IComparer comparer)
				{
				return _list.BinarySearch (index, count, value, comparer);
				}


			public override int Capacity
				{
				get
					{
					return _list.Capacity;
					}
				[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
				set
					{
					throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
					}
				}

			public override void Clear ()
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public override Object Clone ()
				{
				ReadOnlyArrayList arrayList = new ReadOnlyArrayList (_list);
				arrayList._list = (ArrayList)_list.Clone ();
				return arrayList;
				}

			public override bool Contains (Object obj)
				{
				return _list.Contains (obj);
				}

			public override void CopyTo (Array array, int index)
				{
				_list.CopyTo (array, index);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void CopyTo (int index, Array array, int arrayIndex, int count)
				{
				_list.CopyTo (index, array, arrayIndex, count);
				}

			public override IEnumerator GetEnumerator ()
				{
				return _list.GetEnumerator ();
				}

#if !NETCF
			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override IEnumerator GetEnumerator (int index, int count)
				{
				return _list.GetEnumerator (index, count);
				}
#endif

			public override int IndexOf (Object value)
				{
				return _list.IndexOf (value);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public int IndexOf (Object value, int startIndex)
				{
				return _list.IndexOf (value, startIndex, _list.Count - startIndex);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override int IndexOf (Object value, int startIndex, int count)
				{
				return _list.IndexOf (value, startIndex, count);
				}

			public override void Insert (int index, Object obj)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void InsertRange (int index, ICollection c)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

#if !NETCF
			public override int LastIndexOf (Object value)
				{
				return _list.LastIndexOf (value);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override int LastIndexOf (Object value, int startIndex)
				{
				return _list.LastIndexOf (value, startIndex);
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override int LastIndexOf (Object value, int startIndex, int count)
				{
				return _list.LastIndexOf (value, startIndex, count);
				}
#endif

			public override void Remove (Object value)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public override void RemoveAt (int index)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void RemoveRange (int index, int count)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

#if !NETCF
			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void SetRange (int index, ICollection c)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public override ArrayList GetRange (int index, int count)
				{
				if (index < 0 || count < 0)
					throw new ArgumentOutOfRangeException ((index < 0 ? "index" : "count"), Environment.GetResourceString ("ArgumentOutOfRange_NeedNonNegNum"));
				if (Count - index < count)
					throw new ArgumentException (Environment.GetResourceString ("Argument_InvalidOffLen"));

				return new Range (this, index, count);
				}
#endif

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void Reverse (int index, int count)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override void Sort (int index, int count, IComparer comparer)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public override Object[] ToArray ()
				{
				return _list.ToArray ();
				}

			[SuppressMessage ("Microsoft.Contracts", "CC1055")]  // Skip extra error checking to avoid *potential* AppCompat problems.
			public override Array ToArray (Type type)
				{
				return _list.ToArray (type);
				}

			public override void TrimToSize ()
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}
			}

		[Serializable]
		private class ReadOnlyList : IList
			{
			private IList _list;

			internal ReadOnlyList (IList l)
				{
				_list = l;
				}

			public virtual int Count
				{
				get
					{
					return _list.Count;
					}
				}

			public virtual bool IsReadOnly
				{
				get
					{
					return true;
					}
				}

			public virtual bool IsFixedSize
				{
				get
					{
					return true;
					}
				}

			public virtual bool IsSynchronized
				{
				get
					{
					return _list.IsSynchronized;
					}
				}

			public virtual Object this[int index]
				{
				get
					{
					return _list[index];
					}
				set
					{
					throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
					}
				}

			public virtual Object SyncRoot
				{
				get
					{
					return _list.SyncRoot;
					}
				}

			public virtual int Add (Object obj)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public virtual void Clear ()
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public virtual bool Contains (Object obj)
				{
				return _list.Contains (obj);
				}

			public virtual void CopyTo (Array array, int index)
				{
				_list.CopyTo (array, index);
				}

			public virtual IEnumerator GetEnumerator ()
				{
				return _list.GetEnumerator ();
				}

			public virtual int IndexOf (Object value)
				{
				return _list.IndexOf (value);
				}

			public virtual void Insert (int index, Object obj)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public virtual void Remove (Object value)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}

			public virtual void RemoveAt (int index)
				{
				throw new NotSupportedException (Environment.GetResourceString ("NotSupported_ReadOnlyCollection"));
				}
			}
		}
	}