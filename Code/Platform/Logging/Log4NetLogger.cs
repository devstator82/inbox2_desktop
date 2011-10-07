using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;
using log4net;
using log4net.Config;

namespace Inbox2.Platform.Logging
{
	public class Log4NetLogger : ILogger
	{
	    public Log4NetLogger()
		{
			// Use application startup path
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");

			XmlConfigurator.ConfigureAndWatch(new FileInfo(filename));
		}

		public Log4NetLogger(Stream log4netConfigStream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(log4netConfigStream);

			XmlConfigurator.Configure(doc.DocumentElement);
		}

		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public void Warn(string message, LogSource source, params object[] args)
		{
			ILog log = LogManager.GetLogger(source.ToString());
			log.WarnFormat(message, args);
		}

		/// <summary>
		/// Logs the specified message with a debug severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public void Debug(string message, LogSource source, params object[] args)
		{
			ILog log = LogManager.GetLogger(source.ToString());
			log.DebugFormat(message, args);
		}

		/// <summary>
		/// Logs the specified message with an error severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public void Error(string message, LogSource source, params object[] args)
		{
            ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			log.ErrorFormat(message, args);
		}

		/// <summary>
		/// Logs the specified message with a fatal severity.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="source">The source.</param>
		/// <param name="args">The args.</param>
		public void Fatal(string message, LogSource source, params object[] args)
		{
			ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			log.FatalFormat(message, args);
		}
	}
}
