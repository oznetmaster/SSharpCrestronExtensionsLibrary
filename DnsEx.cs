using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using System.Text.RegularExpressions;

namespace Crestron.SimplSharp
	{
	public static class DnsEx
		{
		private static readonly Regex regexIPAddress = new Regex (@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

		public static IPAddress[] GetHostAddresses (string hostNameOrAddress)
			{
			return GetHostAddresses (hostNameOrAddress, false);
			}

		public static IPAddress[] GetHostAddresses (string hostNameOrAddress, bool allowIPV6)
			{
			hostNameOrAddress = hostNameOrAddress.Trim ();

			if (regexIPAddress.IsMatch (hostNameOrAddress))
				try
					{
					var ip = IPAddress.Parse (hostNameOrAddress);

					if (!allowIPV6 && ip.AddressFamily == AddressFamily.InterNetworkV6)
						return new IPAddress[0];

					return new[] {ip};
					}
				catch
					{
					}

			var hosts = new FileInfo (@"\nvram\etc\hosts");
			if (hosts.Exists)
				{
				using (var sr = hosts.OpenText ())
					{
					string line;
					while ((line = sr.ReadLine ()) != null)
						{
						line = line.Trim ();
						if (line.Length == 0 || line[0] == '#')
							continue;

						var parts = line.Split (new [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
						if (parts.Length < 2)
							continue;

						if (!parts.Skip (1).Any (p => String.Equals (p, hostNameOrAddress, StringComparison.InvariantCultureIgnoreCase)))
							continue;

						IPAddress ip;
						try
							{
							ip = IPAddress.Parse (parts[0].Trim ());
							}
						catch
							{
							continue;
							}

						if (!allowIPV6 && ip.AddressFamily == AddressFamily.InterNetworkV6)
							return new IPAddress[0];

						return new[] { ip };
						}
					}
				}

			if (allowIPV6)
				return Dns.GetHostEntry (hostNameOrAddress).AddressList;

			return Dns.GetHostEntry (hostNameOrAddress).AddressList.Where (a => a.AddressFamily == AddressFamily.InterNetwork).ToArray ();
			}

		/*
		public static IPHostEntry GetHostEntry (string hostname)
			{
			try
				{
				var ip = IPAddress.Parse (hostname);

				}
			catch
				{
				}

			var hosts = new FileInfo (@"\nvram\etc\hosts");
			if (hosts.Exists)
				{
				using (var sr = hosts.OpenText ())
					{
					string line;
					while ((line = sr.ReadLine ()) != null)
						{
						line = line.Trim ();
						if (line.Length == 0 || line[0] == '#')
							continue;

						var parts = line.Split (new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
						if (parts.Length < 2)
							continue;

						if (!parts.Skip (1).Any (p => String.Equals (p, hostname, StringComparison.InvariantCultureIgnoreCase)))
							continue;

						IPAddress ip;
						try
							{
							ip = IPAddress.Parse (parts[0].Trim ());
							}
						catch
							{
							continue;
							}

						return new IPHostEntry () {AddressList = new [] {ip}, Aliases = parts.Skip(1).ToArray ()}
						}
					}
				}

			return Dns.GetHostEntry (hostname);
			
			}
		 */

		}
	}