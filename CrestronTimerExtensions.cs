using System;

namespace Crestron.SimplSharp
	{
	public static class CrestronTimerExtensions
		{
		public static void Change (this CTimer ctimer, int dueTime, int period)
			{
			ctimer.Reset (dueTime, period);
			}

		public static void Change (this CTimer ctimer, long dueTime, long period)
			{
			ctimer.Reset (dueTime, period);
			}

		public static void Change (this CTimer ctimer, uint dueTime, uint period)
			{
			ctimer.Reset (dueTime, period);
			}

		public static void Change (this CTimer ctimer, TimeSpan dueTime, TimeSpan period)
			{
			ctimer.Reset ((long)dueTime.TotalMilliseconds, (long)period.TotalMilliseconds);
			}
		}
	}