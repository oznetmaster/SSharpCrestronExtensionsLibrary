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
		}
	}