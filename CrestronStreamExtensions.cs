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