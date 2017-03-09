#region License
/*
 * CrestronEventExtensions.cs
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

namespace Crestron.SimplSharp
	{
	public static class CrestronEventExtensions
		{
		public static bool WaitOne (this CEventHandle ceh)
			{
			return ceh.Wait ();
			}

		public static bool WaitOne (this CEventHandle ceh, int timeout)
			{
			return ceh.Wait (timeout);
			}

		public static bool WaitOne (this CEventHandle ceh, TimeSpan timeout)
			{
			return ceh.Wait ((int)timeout.TotalMilliseconds);
			}

		public static bool WaitOne (this CEventHandle ceh, int timeout, bool exitContext)
			{
			return ceh.Wait (timeout);
			}

		public static bool WaitOne (this CEventHandle ceh, TimeSpan timeout, bool exitContext)
			{
			return ceh.Wait ((int)timeout.TotalMilliseconds);
			}
		}

	public class AutoResetEvent : CEvent
		{
		public AutoResetEvent (bool initalState)
			: base (true, initalState)
			{
			
			}
		}

	public class ManualResetEvent : CEvent
		{
		public ManualResetEvent (bool initialState)
			: base (false, initialState)
			{
			
			}
		}
	}