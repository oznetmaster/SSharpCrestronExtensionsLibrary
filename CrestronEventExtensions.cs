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