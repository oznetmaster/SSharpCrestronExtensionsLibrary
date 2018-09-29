using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

namespace Crestron.SimplSharp
	{
	public static class CrestronConsoleEx
		{
		private static readonly CType _ctypeThread;
		private static readonly PropertyInfo _propCurrentThread;
		private static readonly PropertyInfo _propName;
		private delegate object DelGetCurrentThread ();
		private static DelGetCurrentThread _delGetCurrentThread;

		static CrestronConsoleEx ()
			{
			try
				{
				_ctypeThread = Type.GetType ("Crestron.SimplSharpPro.CrestronThread.Thread, SimplSharpPro");
				}
			catch
				{
				}

			if (_ctypeThread == null)
				return;

			_propCurrentThread = _ctypeThread.GetProperty ("CurrentThread");
			_propName = _ctypeThread.GetProperty ("Name");
			}

		public static bool IsCommandThread
			{
			get
				{
				if (_ctypeThread == null)
					return false;

				if (_delGetCurrentThread == null)
					_delGetCurrentThread = (DelGetCurrentThread)CDelegate.CreateDelegate (typeof (DelGetCurrentThread), null, _propCurrentThread.GetGetMethod ());

				return (string)_propName.GetValue (_delGetCurrentThread (), null) == "SimplSharpProCommandProcessorThread";
				}
			}

		/// <summary>
		/// Flag to determine if the console has been registered. Once registered adding
		///    new console commands is not allowed.
		/// </summary>
		public static bool ConsoleRegistered
			{
			get { return CrestronConsole.ConsoleRegistered; }
			}

		/// <summary>
		/// Function for the user to add console commands to the system.
		/// </summary>
		/// <param name="userFunction">Callback function the command will call.</param>
		/// <param name="userCmdName">Name of the console command. Spaces are not permitted.</param>
		/// <param name="userCmdHelp">Information on the console command.</param>
		/// <param name="userAccess">Authentication level of the command.</param>
		/// <returns>true if the operation succeeds; otherwise, false.</returns>
		/// <exception cref="System.ArgumentException">Verify the Command name does not contain spaces.</exception>
		/// <exception cref="System.NotSupportedException">This feature is only available in SIMPL# Pro</exception>
		public static bool AddNewConsoleCommand (SimplSharpProConsoleCmdFunction userFunction, string userCmdName, string userCmdHelp,
			ConsoleAccessLevelEnum userAccess)
			{
			return CrestronConsole.AddNewConsoleCommand (userFunction, userCmdName, userCmdHelp, userAccess);
			}

		/// <summary>
		/// Function for the user to add console commands to the system.
		/// </summary>
		/// <param name="userFunction">Callback function the command will call.</param>
		/// <param name="userCmdName">Name of the console command. Spaces are not permitted.</param>
		/// <param name="userCmdHelp">Information on the console command.</param>
		/// <param name="userAccess">Authentication level of the command.</param>
		/// <param name="bIsCommandHidden">If true the command will be hidden and will not show up in help user.</param>
		/// <returns>true if the operation succeeds; otherwise, false.</returns>
		/// <exception cref="System.ArgumentException">Verify the Command name does not contain spaces.</exception>
		/// <exception cref="System.NotSupportedException">This feature is only available in SIMPL# Pro</exception>
		public static bool AddNewConsoleCommand (SimplSharpProConsoleCmdFunction userFunction, string userCmdName, string userCmdHelp,
			ConsoleAccessLevelEnum userAccess, bool bIsCommandHidden)
			{
			return CrestronConsole.AddNewConsoleCommand (userFunction, userCmdName, userCmdHelp, userAccess, bIsCommandHidden);
			}

		/// <summary>
		/// Function to print a message to the console, from the context of a console
		/// command function.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void ConsoleCommandResponse (string message)
			{
			CrestronConsole.ConsoleCommandResponse (message);
			}

		/// <summary>
		/// Function to print a message to the console including the end line characters,
		/// from the context of a console command function.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void ConsoleCommandResponseLine (string message)
			{
			CrestronConsole.ConsoleCommandResponse (message + CrestronEnvironment.NewLine);
			}

		/// <summary>
		/// Function to print an empty line on the text console,
		/// from the context of a console command function.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void ConsoleCommandResponseLine ()
			{
			CrestronConsole.ConsoleCommandResponse (CrestronEnvironment.NewLine);
			}

		/// <summary>
		/// Function to print a formatted message to the console, from the context of a console
		/// command function.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <param name="args">Array containing objects to format.</param>
		/// <exception cref="System.ArgumentNullException">Arguments message or args are null.</exception>
		/// <exception cref="System.FormatException">
		/// Format is invalid or the number of arguments does not match the argument
		/// number specified in the message.
		/// </exception>
		public static void ConsoleCommandResponse (string message, params object[] args)
			{
			CrestronConsole.ConsoleCommandResponse (message, args);
			}

		/// <summary>
		/// Function to print a formatted message to the console including the end line characters,
		/// from the context of a console command function.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <param name="args">Array containing objects to format.</param>
		/// <exception cref="System.ArgumentNullException">Arguments message or args are null.</exception>
		/// <exception cref="System.FormatException">
		/// Format is invalid or the number of arguments does not match the argument
		/// number specified in the message.
		/// </exception>
		public static void ConsoleCommandResponseLine (string message, params object[] args)
			{
			CrestronConsole.ConsoleCommandResponse (message + CrestronEnvironment.NewLine, args);
			}

		/// <summary>
		///  Function to print a message to the text console.
		/// </summary>
		/// <param name="message">Message to print to the console.</param>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void Print (string message)
			{
			if (IsCommandThread)
				CrestronConsole.ConsoleCommandResponse (message);
			else
				CrestronConsole.Print (message);
			}

		/// <summary>
		/// Function to print a formatted message to the text console.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <param name="args">Array containing objects to format.</param>
		/// <exception cref="System.ArgumentNullException">Arguments message or args are null.</exception>
		/// <exception cref="System.FormatException">
		/// Format is invalid or the number of arguments does not match the argument
		/// number specified in the message.
		/// </exception>
		public static void Print (string message, params object[] args)
			{
			if (IsCommandThread)
				CrestronConsole.ConsoleCommandResponse (message, args);
			else
				CrestronConsole.Print (message, args);
			}

		/// <summary>
		///  Function to print a message to the text console including the end line characters.
		/// </summary>
		/// <param name="message">Message to print to the console.</param>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void PrintLine (string message)
			{
			if (IsCommandThread)
				CrestronConsole.ConsoleCommandResponse (message + CrestronEnvironment.NewLine);
			else
				CrestronConsole.PrintLine (message);
			}

		/// <summary>
		///  Function to print an empty line to the text console.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Argument message is null.</exception>
		public static void PrintLine ()
			{
			if (IsCommandThread)
				CrestronConsole.ConsoleCommandResponse (CrestronEnvironment.NewLine);
			else
				CrestronConsole.PrintLine ("");
			}

		/// <summary>
		/// Function to print a formatted message to the text console including the end line characters.
		/// </summary>
		/// <param name="message"> Message to print.</param>
		/// <param name="args">Array containing objects to format.</param>
		/// <exception cref="System.ArgumentNullException">Arguments message or args are null.</exception>
		/// <exception cref="System.FormatException">
		/// Format is invalid or the number of arguments does not match the argument
		/// number specified in the message.
		/// </exception>
		public static void PrintLine (string message, params object[] args)
			{
			if (IsCommandThread)
				CrestronConsole.ConsoleCommandResponse (message + CrestronEnvironment.NewLine, args);
			else
				CrestronConsole.PrintLine (message, args);
			}

		/// <summary>
		/// Function for the user to remove console commands from the system. Only commands
		/// which have been registered by the user can be removed.
		/// </summary>
		/// <param name="userCmdName">Name of the console command to remove.</param>
		/// <returns>true if the operation succeeds; otherwise false</returns>
		/// <exception cref="System.ArgumentException">Command name passed in was not registered by the user or command name contains spaces.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Specified parameter is out of range.</exception>
		public static bool RemoveConsoleCommand (string userCmdName)
			{
			return CrestronConsole.RemoveConsoleCommand (userCmdName);
			}

		/// <summary>
		/// Function to allow the user to send a console command and receive a response
		/// from the control system.
		/// </summary>
		/// <param name="commandToSend">The console command to send; end line not needed.</param>
		/// <param name="pResponse">Reference to a string where the controller's response will be stored.</param>
		/// <returns>true if the command was sent correctly; false if it was not sent correctly.</returns>
		public static bool SendControlSystemCommand (string commandToSend, ref string pResponse)
			{
			return CrestronConsole.SendControlSystemCommand (commandToSend, ref pResponse);
			}

		/// <summary>
		/// Function to allow the user to send a console command and receive a response
		/// from the control system.
		/// </summary>
		/// <param name="commandToSend">The console command to send; end line not needed.</param>
		/// <param name="pResponse">Reference to a string where the controller's response will be stored.</param>
		/// <param name="timeOutInMsec">The time out in msec.</param>
		/// <returns>true if the command was sent correctly; false if it was not sent correctly.</returns>
		public static bool SendControlSystemCommand (string commandToSend, ref string pResponse, uint timeOutInMsec)
			{
			return CrestronConsole.SendControlSystemCommand (commandToSend, ref pResponse, timeOutInMsec);
			}
		}
	}