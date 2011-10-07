using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Framework.Web
{
	public static class HttpStatusClient
	{
		public static int? GetStatusCode(string url, Dictionary<string, string> headers)
		{
			try
			{
				string host = String.Empty;
				string path = String.Empty;
				int pos = 0;

				  // parse up the request string
				if (url.StartsWith ("http://"))
					host = url.Substring (7);  
				else
					host = url;
				
				// split at the first path delimiter
				if (-1 != (pos = host.IndexOf ('/')))
				{ 
					path = host.Substring (pos);
					host = host.Substring(0, pos);
				}
				
				// make sure path is correct
				if (String.IsNullOrEmpty (path) || !path.StartsWith ("/"))
					path = String.Concat ("/", (path ?? ""));
				
				// connect to server 
				var sock = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				sock.Blocking = true;

				// Build headers string
				StringBuilder sb = new StringBuilder();				
				if (headers.Count > 0)
				{
					sb.AppendLine();

					foreach (var key in headers.Keys)
						sb.AppendFormat("{0}: {1}{2}", key, headers[key], Environment.NewLine);
				}

				// build request
				string request = String.Format (
@"GET {0} HTTP/1.0
HOST: {1}{2}

", path, host, sb);

				// Convert the string data to byte data using ASCII encoding
				sock.Connect(host, 80);
				sock.Send(Encoding.ASCII.GetBytes (request));

				byte[] receiveBuffer = new byte[1024];
				int received = sock.Receive(receiveBuffer);
				string response = Encoding.ASCII.GetString(receiveBuffer, 0, received);

				// read the first line from the response and read the statuscode
				string[] lines = response.Split('\n');
				string[] parts = lines.First().Split(' ');

				return Int32.Parse(parts[1]);
			}
			catch (Exception ex)
			{				
				Logger.Error("An error has occured while checking the http status code. Url = '{0}', Exception = {1}", LogSource.Channel, url, ex);

				return null;				
			}
		}
	}
}
