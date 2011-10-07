using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Inbox2.Platform.Logging
{
	public static class Logger
	{
		private static ILogger current { get; set; }

		public static void Initialize(ILogger logger)
		{
			current = logger;
		}

		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public static void Warn(string message, LogSource source, params object[] args)
		{
			current.Warn(message, source, args);
		}

		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public static void Debug(string message, LogSource source, params object[] args)
		{
			current.Debug(message, source, args);
		}

		/// <summary>
		/// Logs the specified message with an error severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public static void Error(string message, LogSource source, params object[] args)
		{
			current.Error(message, source, args);
		}

		/// <summary>
		/// Logs the specified message with a fatal severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public static void Fatal(string message, LogSource source, params object[] args)
		{
			current.Fatal(message, source, args);
		}
	}
}