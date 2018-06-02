using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp
	{
	public static class IPAddressExtensions
		{
		public static IPAddress Clone (this IPAddress ipAddress)
			{
			return new IPAddress (ipAddress.GetAddressBytes ());
			}
		}
	}