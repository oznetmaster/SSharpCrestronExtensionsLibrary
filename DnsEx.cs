#region License
/*
 * DnsEx.cs
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