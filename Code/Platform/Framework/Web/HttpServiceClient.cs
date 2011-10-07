using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.Web.Upload;
using Inbox2.Platform.Logging;

namespace Inbox2.Platform.Framework.Web
{
	public static class HttpServiceRequest
	{
		public static string FormUrlEncoded = "application/x-www-form-urlencoded";

		public static string Get(string url)
		{
			return Get(url, false);
		}

		public static string Get(string url, bool throwOnError)
		{
			int statusCode;

			return Get(url, throwOnError, out statusCode);
		}

		public static string Get(string url, bool throwOnError, out int statusCode)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);

				AttachCertificate(request);

				var response = (HttpWebResponse)request.GetResponse();
				statusCode = (int)response.StatusCode;
				
				using (var result = response.GetResponseStream())
					return result.ReadString();
			}
			catch (Exception ex)
			{
				statusCode = -1;

				Logger.Error("An error has occured while calling url. url = {0} Exception = {1}", LogSource.ServiceCall, url, ex);

				if (ex is WebException)
				{
					var wex = (WebException) ex;
					var response = (HttpWebResponse) wex.Response;

					if (response != null)
						statusCode = (int)response.StatusCode;
				}				

				if (throwOnError)
					throw;

				return null;
			}
		}

		public static Stream GetStream(string url)
		{
			return GetStream(url, false);
		}

		public static Stream GetStream(string url, bool throwOnError)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);

				AttachCertificate(request);

				var response = request.GetResponse();

				return response.GetResponseStream();
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while calling url. url = {0} Exception = {1}", LogSource.ServiceCall, url, ex);

				if (throwOnError)
					throw;

				return null;
			}
		}

		public static string Post(string url, string data)
		{
			return Post(url, data, FormUrlEncoded);
		}

		public static string Post(string url, string data, bool throwOnError)
		{
			return Post(url, data, FormUrlEncoded, throwOnError);
		}

		public static string Post(string url, string data, string contentType)
		{
			return Post(url, data, contentType, false);
		}

		public static string Post(string url, string data, string contentType, bool throwOnError)
		{
			return Send("POST", url, data, contentType, throwOnError);
		}

		public static string Put(string url, string data)
		{
			return Put(url, data, FormUrlEncoded);
		}

		public static string Put(string url, string data, bool throwOnError)
		{
			return Put(url, data, FormUrlEncoded, throwOnError);
		}

		public static string Put(string url, string data, string contentType)
		{
			return Put(url, data, contentType, false);
		}

		public static string Put(string url, string data, string contentType, bool throwOnError)
		{
			return Send("PUT", url, data, contentType, throwOnError);
		}

		internal static string Send(string method, string url, string data, string contentType, bool throwOnError)
		{
			try
			{
				var uri = new Uri(url);
				var request = (HttpWebRequest)WebRequest.Create(
					String.Format("{0}://{1}:{2}{3}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath));

				AttachCertificate(request);

				request.Method = method;
				request.ContentType = contentType;

				byte[] bytes = Encoding.UTF8.GetBytes(data);
				request.ContentLength = bytes.Length;

				// Write post data to request stream
				using (Stream requestStream = request.GetRequestStream())
					requestStream.Write(bytes, 0, bytes.Length);

				using (var response = request.GetResponse())
				using (var result = response.GetResponseStream())
					return result.ReadString();
			}
			catch (Exception ex)
			{
				string message = String.Empty;

				if (ex is WebException)
				{
					var wex = (WebException)ex;
					var response = (HttpWebResponse)wex.Response;

					if (response != null)
					{
						using (var stream = response.GetResponseStream())
							message = stream.ReadString();
					}
				}

				Logger.Error("An error has occured while calling url. Url = {0}, Data = {1}, Exception = {2}, Message = {3}", LogSource.ServiceCall, url, data, ex, message);

				if (throwOnError)
					throw;

				return null;
			}
		}

		public static string Post(string url, string data, List<UploadFile> files)
		{
			return Post(url, data, files, false);
		}

		public static string Post(string url, string data, List<UploadFile> files, bool throwOnError)
		{
			try
			{
				var uri = new Uri(url);
				var request = (HttpWebRequest)WebRequest.Create(
					String.Format("{0}://{1}:{2}{3}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath));

				AttachCertificate(request);

				using (var response = HttpUploadHelper.Upload(request, files.ToArray(), NameValueParser.GetCollection(data, "&")))
				using (var result = response.GetResponseStream())
					return result.ReadString();
			}
			catch (Exception ex)
			{
				string message = String.Empty;

				if (ex is WebException)
				{
					var wex = (WebException)ex;
					var response = (HttpWebResponse)wex.Response;

					if (response != null)
					{
						using (var stream = response.GetResponseStream())
							message = stream.ReadString();
					}						
				}

				Logger.Error("An error has occured while calling url. Url = {0}, Data = {1}, Exception = {2}, Message = {3}", LogSource.ServiceCall, url, data, ex, message);

				if (throwOnError)
					throw;

				return null;
			}			
		}

		public static string GetServiceUrl()
		{
			var env = "/Settings/Application/Environment".AsKey(String.Empty);
			var scheme = String.IsNullOrEmpty(env) ? "https://" : "http://";

			// When request signing is disabled, always use http
			if ("/Settings/Application/DisableServiceRequestSigning".AsKey(false))
				scheme = "http://";

			if (String.IsNullOrEmpty(env))
				return scheme + "services.inbox2.com";
			else
				return String.Format(scheme + "services.{0}.inbox2.com", env);
		}

		static void AttachCertificate(HttpWebRequest request)
		{
			var env = "/Settings/Application/Environment".AsKey(String.Empty);
			var disable = "/Settings/Application/DisableServiceRequestSigning".AsKey(false);

			if (env == String.Empty && disable == false)
			{
				X509Certificate2 cert = GetCertificate();

				request.ClientCertificates.Add(cert);
			}
		}

		public static X509Certificate2 GetCertificate()
		{
			X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

			X509Certificate2 cert = certStore.Certificates.Find(X509FindType.FindByIssuerName, "Inbox2.com CA", false)[0];
			certStore.Close();
			return cert;
		}
	}
}
