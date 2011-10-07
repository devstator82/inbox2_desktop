using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Framework.OAuth;

namespace Inbox2.Channels.Yammer
{
	public static class YammerWebRequest
	{
		public static string PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "inbox2";
			request.Headers.Add("Authorization", BuildOAuthParams(consumerKey, consumerSecret, null, null, null));

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var responseStream = response.GetResponseStream())
				return responseStream.ReadString();
		}

		public static string PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "inbox2";
			request.Headers.Add("Authorization", BuildOAuthParams(consumerKey, consumerSecret, token, tokenSecret, verifier));

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var responseStream = response.GetResponseStream())
				return responseStream.ReadString();
		}

		public static XDocument PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
			request.Method = "GET";
			request.UserAgent = "inbox2";
			request.Headers.Add("Authorization", BuildOAuthParams(consumerKey, consumerSecret, token, tokenSecret, null));

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var responseStream = response.GetResponseStream())
				return XDocument.Parse(responseStream.ReadString());
		}

		public static XDocument PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret, string token, string tokenSecret, Dictionary<string, object> parameters)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "inbox2";
			request.Headers.Add("Authorization", BuildOAuthParams(consumerKey, consumerSecret, token, tokenSecret, null));

			StringBuilder data = new StringBuilder();
			foreach (var parameter in parameters)
			{
				if (data.Length > 0)
					data.Append("&");

				data.AppendFormat("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value.ToString()));
			}

			byte[] bytes = Encoding.UTF8.GetBytes(data.ToString());
			request.ContentLength = bytes.Length;

			// Write post data to request stream
			using (Stream requestStream = request.GetRequestStream())
				requestStream.Write(bytes, 0, bytes.Length);

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var responseStream = response.GetResponseStream())
				return XDocument.Parse(responseStream.ReadString());
		}		

		static string BuildOAuthParams(string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier)
		{
			string timeStamp = OAuthBase.GenerateTimeStamp();
			string nonce = OAuthBase.GenerateNonce();

			StringBuilder sb = new StringBuilder();

			sb.Append(@"OAuth realm="", ");
			sb.AppendFormat(@"oauth_consumer_key=""{0}"", ", consumerKey);

			if (token != null)
				sb.AppendFormat(@"oauth_token=""{0}"", ", token);

			sb.AppendFormat(@"oauth_signature_method=""PLAINTEXT"", ");
			sb.AppendFormat(@"oauth_signature=""{0}%26{1}"", ", consumerSecret, tokenSecret);
			sb.AppendFormat(@"oauth_timestamp=""{0}"", ", timeStamp);
			sb.AppendFormat(@"oauth_nonce=""{0}"", ", nonce);

			if (verifier != null)
				sb.AppendFormat(@"oauth_verifier=""{0}"", ", verifier);

			sb.AppendFormat(@"oauth_version=""1.0"", ");

			return sb.ToString();
		}
	}
}
