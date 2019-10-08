#region License

/*
 * CrestronEnvironmentExtensions.cs
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
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Reflection;

namespace Crestron.SimplSharp
	{
	public static class CrestronEnvironmentEx
		{
		static CrestronEnvironmentEx ()
			{
			CrestronEnvironment.ProgramStatusEventHandler += CrestronEnvironment_ProgramStatusEventHandler;
			CrestronEnvironment.SystemEventHandler += CrestronEnvironment_SystemEventHandler;

			s_haveSystemResources = File.Exists (Path.Combine (InitialParametersClass.ProgramDirectory.ToString (), "SSRefMscorlibResources.dll"))
				&& File.Exists (Path.Combine (InitialParametersClass.ProgramDirectory.ToString (), "SSCoreResourceLibrary.dll"));
			}

		private static void CrestronEnvironment_SystemEventHandler (eSystemEventType systemEventType)
			{
			switch (systemEventType)
				{
				case eSystemEventType.Rebooting:
					HasRebootStarted = true;
					break;
				}
			}

		private static void CrestronEnvironment_ProgramStatusEventHandler (eProgramStatusEventType programEventType)
			{
			switch (programEventType)
				{
				case eProgramStatusEventType.Stopping:
					HasShutdownStarted = true;
					break;
				}
			}

		public static bool HasShutdownStarted { get; private set; }

		public static bool HasRebootStarted { get; private set; }

		public enum SpecialFolder
			{
			Programs = 0x02,
			ApplicationData = 0x1a,
			CommonApplicationData = 0x23,
			}

		internal enum SpecialFolderOption
			{
			None = 0,
			DoNotVerify = 0x4000,
			Create = 0x8000
			}

		/// <summary>
		/// Gets or sets the current directory. Actually this is supposed to get
		/// and/or set the process start directory acording to the documentation
		/// but actually test revealed at beta2 it is just Getting/Setting the CurrentDirectory
		/// </summary>
		public static string CurrentDirectory
			{
			get
				{
				if (currentDirectory != null)
					return currentDirectory;

				return InitialParametersClass.ProgramDirectory.ToString ();
				}

			set
				{
				if (value == null)
					throw new ArgumentNullException ("path");
				if (value.Trim ().Length == 0)
					throw new ArgumentException ("path string must not be an empty string or whitespace string");
				if (value.Length >= 260)
					throw new ArgumentOutOfRangeException ("path", "maximum length of path is 260 characters");
				if (value.IndexOfAny (Path.GetInvalidPathChars ()) != -1)
					throw new ArgumentException ("path", "path contains invalid characters");
				var tempPath = Path.IsPathRooted (value) ? value : Path.Combine (CurrentDirectory, value);
				if (!Directory.Exists (tempPath))
					throw new DirectoryNotFoundException ();
				currentDirectory = tempPath;
				}
			}

		private static string currentDirectory;

		/// <summary>
		/// Gets the name of the local computer
		/// </summary>
		public static string MachineName
			{
			get { return machineName ?? (machineName = CrestronEthernetHelper.GetEthernetParameter (CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_HOSTNAME, 0)); }
			}

		private static string machineName;

		/// <summary>
		/// Gets the standard new line value
		/// </summary>
		public static string NewLine
			{
			get { return CrestronEnvironment.NewLine; }
			}

		//
		// Support methods and fields for OSVersion property
		//
		private static object os;

		private static PlatformID Platform
			{
			get { return PlatformID.WinCE; }
			}

		/// <summary>
		/// Gets the current OS version information
		/// </summary>
		public static OperatingSystem OSVersion
			{
			get
				{
				if (os == null)
					os = new OperatingSystem (PlatformID.WinCE, CrestronEnvironment.OSVersion.Version);
				return os as OperatingSystem;
				}
			}

		/// <summary>
		/// Get StackTrace
		/// </summary>
		public static string StackTrace
			{
			get
				{
				try
					{
					throw new Exception ();
					}
				catch (Exception ex)
					{
					return ex.StackTrace;
					}
				}
			}

		/// <summary>
		/// Get the number of milliseconds that have elapsed since the system was booted
		/// </summary>
		public static int TickCount
			{
			get { return CrestronEnvironment.TickCount; }
			}

		/// <summary>
		/// Get the version of the common language runtime 
		/// </summary>
		public static Version Version
			{
			get { return CrestronEnvironment.OSVersion.Version; }
			}

		/// <summary>
		/// Returns the fully qualified path of the
		/// folder specified by the "folder" parameter
		/// </summary>
		public static string GetFolderPath (SpecialFolder folder)
			{
			return GetFolderPath (folder, SpecialFolderOption.None);
			}

		private static string GetFolderPath (SpecialFolder folder, SpecialFolderOption option)
			{
			string dir = null;
			switch (folder)
				{
				case SpecialFolder.ApplicationData:
					dir = String.Format ("\\Nvram\\App{0:D2}\\", InitialParametersClass.ApplicationNumber);
					if (option == SpecialFolderOption.Create)
						Directory.Create (dir);
					break;
				case SpecialFolder.Programs:
					dir = InitialParametersClass.ProgramDirectory.ToString ();
					break;
				case SpecialFolder.CommonApplicationData:
					dir = "\\Nvram\\";
					break;
				}

			return dir;
			}

		public static void FailFast (string message)
			{
			ErrorLog.Error (String.Format ("FailFast: {0}\r\n{1}", message, StackTrace));

			if (Debugger.IsAttached)
				{
				Debugger.WriteLine (message);
				Debugger.Break ();
				}

			string response = String.Empty;
			CrestronConsole.SendControlSystemCommand ("stopprog -p:" + InitialParametersClass.ApplicationNumber, ref response);
			}


		public static bool IsRunningOnWindows
			{
			get { return true; }
			}

		public static bool IsUnix
			{
			get { return false; }
			}

		public static bool IsMacOS
			{
			get { return false; }
			}

		public static int ProcessorCount
			{
			get { return 1; }
			}

		public static void Sleep (int timeoutInMsec)
			{
			CrestronEnvironment.Sleep (timeoutInMsec);
			}

		public static void AllowOtherAppsToRun ()
			{
			CrestronEnvironment.AllowOtherAppsToRun ();
			}

		private static bool s_haveSystemResources;
		private static object s_internalSyncObject;
		private static GetStringDelegate s_getString;

		private delegate string GetStringDelegate (string name);

		internal static object InternalSyncObject
			{
			get
				{
				if (ReferenceEquals (s_internalSyncObject, null))
					{
					Interlocked.CompareExchange (ref s_internalSyncObject, new object (), null);
					}
				return s_internalSyncObject;
				}
			}

		private static GetStringDelegate GetString
			{
			get
				{
				if (ReferenceEquals (s_getString, null))
					{
					CMonitor.Enter (InternalSyncObject);
					try
						{
						if (ReferenceEquals (s_getString, null))
							{
							var managerAssembly = Assembly.LoadFrom (Path.Combine (InitialParametersClass.ProgramDirectory.ToString (), "SSCoreResourceLibrary.dll"));
							if (managerAssembly == null)
								{
								s_haveSystemResources = false;
								return null;
								}

							var resourceManagerType = managerAssembly.GetType ("SSCore.Resources.ResourceManager");
							if (resourceManagerType == null)
								{
								s_haveSystemResources = false;
								return null;
								}

							var getString = resourceManagerType.GetMethod ("GetString", new CType[] { typeof (string) });
							if (getString == null)
								{
								s_haveSystemResources = false;
								return null;
								}

							var sr = Assembly.LoadFrom (Path.Combine (InitialParametersClass.ProgramDirectory.ToString (), "SSRefMscorlibResources.dll"));
							if (sr == null)
								{
								s_haveSystemResources = false;
								return null;
								}

							var ctorResourceManager = resourceManagerType.GetConstructor (new CType[] {typeof (string), typeof (Assembly)});
							var resourceManager = ctorResourceManager.Invoke (new object[] {"SSRefMscorlibResources.SR", sr});
							if (resourceManager == null)
								{
								s_haveSystemResources = false;
								return null;
								}

							s_getString = (GetStringDelegate)CDelegate.CreateDelegate (typeof (GetStringDelegate), resourceManager, getString);
							}
						}
					finally
						{
						CMonitor.Exit (InternalSyncObject);
						}
					}

				return s_getString;
				}
			}


		public static string GetResourceString (string key)
			{
			if (!s_haveSystemResources || GetString == null)
				return key;

			return GetString (key);
			}

		public static string GetResourceString (string fmt, params Object[] args)
			{
			if (!s_haveSystemResources || GetString == null)
				return fmt;

			return String.Format (GetString (fmt), args);
			}
		}
	}