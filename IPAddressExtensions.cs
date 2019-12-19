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

		internal static ushort[] GetAddressWords (this IPAddress address)
			{
			if (address.AddressFamily == AddressFamily.InterNetwork)
				throw new InvalidOperationException("Must be IPv^ address");

			var bytes = address.GetAddressBytes ();

			var numbers = new ushort[8];
			for (int ix = 0; ix < 8; ix += 2)
				numbers[ix / 2] = (ushort)((bytes[ix + 1] << 8) | bytes[ix]);

			return numbers;
			}

		public static bool IsIPv6LinkLocal (this IPAddress address)
			{
			if (address.AddressFamily == AddressFamily.InterNetwork)
				return false;
			int v = IPAddress.NetworkToHostOrder ((short)address.GetAddressWords()[0]) & 0xFFF0;
			return 0xFE80 <= v && v < 0xFEC0;
			}

		public static bool IsIPv6Multicast (this IPAddress address)
			{
			return address.AddressFamily != AddressFamily.InterNetwork &&
				((ushort)IPAddress.NetworkToHostOrder ((short)address.GetAddressWords()[0]) & 0xFF00) == 0xFF00;
			}
		}
	}