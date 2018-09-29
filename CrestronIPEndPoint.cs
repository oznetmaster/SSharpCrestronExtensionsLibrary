using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp
	{
	public class CrestronIPEndPoint : IPEndPoint
		{
		public EthernetAdapterType AdapterType { get; set; }

		public CrestronIPEndPoint (IPAddress address, int port)
			: base (address, port)
			{
			AdapterType = EthernetAdapterType.EthernetUnknownAdapter;
			}

		public CrestronIPEndPoint (long address, int port)
			: base (address, port)
			{
			AdapterType = EthernetAdapterType.EthernetUnknownAdapter;
			}

		public CrestronIPEndPoint (IPAddress address, int port, EthernetAdapterType adapterType)
			: base (address, port)
			{
			AdapterType = adapterType;
			}

		public CrestronIPEndPoint (long address, int port, EthernetAdapterType adapterType)
			: base (address, port)
			{
			AdapterType = adapterType;
			}

		public CrestronIPEndPoint (IPEndPoint endpoint)
			: this (endpoint.Address, endpoint.Port)
			{
			}
		}
	}