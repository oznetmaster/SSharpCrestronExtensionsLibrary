#region License
/*
 * CrestronStreamExtensions.cs
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

namespace Crestron.SimplSharp.CrestronIO
	{
	public static class CrestronStreamExtensions
		{
		public static void CopyTo (this Stream stream, Stream destination)
			{
			stream.CopyTo (destination, 16 * 1024);
			}

		public static void CopyTo (this Stream stream, Stream destination, int bufferSize)
			{
			if (destination == null)
				throw new ArgumentNullException ("destination");
			if (!stream.CanRead)
				throw new NotSupportedException ("This stream does not support reading");
			if (!destination.CanWrite)
				throw new NotSupportedException ("This destination stream does not support writing");
			if (bufferSize <= 0)
				throw new ArgumentOutOfRangeException ("bufferSize");

			var buffer = new byte[bufferSize];
			int nread;
			while ((nread = stream.Read (buffer, 0, bufferSize)) != 0)
				destination.Write (buffer, 0, nread);
			}
		}
	}