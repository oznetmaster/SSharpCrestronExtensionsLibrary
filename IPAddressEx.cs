using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp
	{
	public static class IPAddressEx
		{
		public static bool TryParse (string ipString, out IPAddress address)
			{
			try
				{
				address = IPAddress.Parse (ipString);
				return true;
				}
			catch (FormatException)
				{
				address = IPAddress.None;
				return false;
				}
			}

		public static bool IsLoopback (IPAddress address)
			{
			if (address.AddressFamily == AddressFamily.InterNetwork)
				return (address.GetAddressBytes ()[0] & 0xFF) == 127;

			var words = address.GetAddressWords ();

			for (int i = 0; i < 6; i++)
				{
				if (words[i] != 0)
					return false;
				}

			return IPAddress.NetworkToHostOrder ((short)words[7]) == 1;
			}

		}
	}