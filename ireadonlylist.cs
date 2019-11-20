// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Interface:  IReadOnlyList<T>
** 
** <OWNER>matell</OWNER>
**
** Purpose: Base interface for read-only generic lists.
** 
===========================================================*/

using System;

namespace System.Collections.Generic
	{
	// Provides a read-only, covariant view of a generic list.

	// Note that T[] : IReadOnlyList<T>, and we want to ensure that if you use
	// IList<YourValueType>, we ensure a YourValueType[] can be used 
	// without jitting.  Hence the TypeDependencyAttribute on SZArrayHelper.
	// This is a special hack internally though - see VM\compile.cpp.
	// The same attribute is on IList<T>, IEnumerable<T>, ICollection<T> and IReadOnlyCollection<T>.
	// If we ever implement more interfaces on IReadOnlyList, we should also update RuntimeTypeCache.PopulateInterfaces() in rttype.cs
	public interface IReadOnlyList<T> : IReadOnlyCollection<T>
		{
		T this [int index] { get; }
		}
	}