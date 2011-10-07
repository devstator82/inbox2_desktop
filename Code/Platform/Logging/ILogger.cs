using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Logging
{
	public interface ILogger
	{
		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		void Warn(string message, LogSource source, params object[] args);

		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		void Debug(string message, LogSource source, params object[] args);

		/// <summary>
		/// Logs the specified message with an error severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		void Error(string message, LogSource source, params object[] args);

		/// <summary>
		/// Logs the specified message with a fatal severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		void Fatal(string message, LogSource source, params object[] args);
	}
}