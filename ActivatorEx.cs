#region License
/*
 * ActivatorEx.cs
 *
 * The MIT License
 *
 * Copyright © 2017 Nivloc Enterprises Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using System.Globalization;

namespace System
	{
	public static class ActivatorEx
		{
		private const BindingFlags BfNonPublic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private const BindingFlags BfPublic = BindingFlags.Public | BindingFlags.Instance;

		public static object CreateInstance (Type type)
			{
			return Activator.CreateInstance (type);
			}

		public static T CreateInstance<T> () where T : new()
			{
			return Activator.CreateInstance<T> ();
			}

		public static object CreateInstance (Type type, bool nonPublic)
			{
			CType ct = type;

			if (!nonPublic)
				return Activator.CreateInstance (ct);

			var ci = ct.GetConstructor (BfNonPublic, null, ArrayEx.Empty<CType> (), null);
			if (ci == null)
				throw new MissingMethodException ();

			return ci.Invoke (null);
			}

		public static object CreateInstance (Type type, params object[] args)
			{
			CType ct = type;

			if (args == null || args.Length == 0)
				return Activator.CreateInstance (ct);

			var ci = ct.GetConstructor (args.Cast<CType>().ToArray ());
			if (ci == null)
				{
				if (Array.IndexOf (args, null) != -1)
					{
					var cis = ct.GetConstructors ();
					if (cis.Length == 1)
						{
						var parms = cis[0].GetParameters ();
						if (parms.Length == args.Length)
							{
							for (int ix = 0; ix < parms.Length; ++ix)
								{
								if (args[ix] != null)
									continue;

								if (parms[ix].ParameterType.IsPrimitive)
									break;
								}

							ci = cis[0];
							}
						}
					}

				if (ci == null)
					throw new MissingMethodException ();
				}

			return ci.Invoke (args);
			}

		public static object CreateInstance (Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
			{
			var ci = type.GetCType ().GetConstructor (bindingAttr, binder, args.Cast<CType>().ToArray (), null);
			if (ci == null)
				{
				if (Array.IndexOf (args, null) != -1)
					{
					var cis = type.GetCType ().GetConstructors (bindingAttr);
					if (cis.Length == 1)
						{
						var parms = cis[0].GetParameters ();
						if (parms.Length == args.Length)
							{
							for (int ix = 0; ix < parms.Length; ++ix)
								{
								if (args[ix] != null)
									continue;

								if (parms[ix].ParameterType.IsPrimitive)
									break;
								}

							ci = cis[0];
							}
						}
					}

				if (ci == null)
					throw new MissingMethodException ();
				}

			return ci.Invoke (bindingAttr, binder, args, culture);
			}
		}
	}