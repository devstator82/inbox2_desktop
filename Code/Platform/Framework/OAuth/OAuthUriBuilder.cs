using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Inbox2.Platform.Channels.Text;

namespace Inbox2.Platform.Framework.OAuth
{
	public static class OAuthUriBuilder
	{
		public static string Build(Uri source, string consumerKey, string consumerSecret)
		{
			return Build(source, consumerKey, consumerSecret, null, null);
		}

		public static string Build(Uri source, string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			OAuthBase oauth = new OAuthBase();

			string timeStamp = OAuthBase.GenerateTimeStamp();
			string nonce = OAuthBase.GenerateNonce();

			// Calling source.Query returns an urlencoded string, but we don't want that since we will use
			// oauth.UrlEncode ourselves
			var query = HttpUtility.UrlDecode(source.Query.Contains("?") ? source.Query.Remove(0, 1) : source.Query);
			var parameters = NameValueParser.GetCollection(query, "&");

			parameters.Add("oauth_consumer_key", consumerKey);
			parameters.Add("oauth_timestamp", timeStamp);
			parameters.Add("oauth_nonce", nonce);
			parameters.Add("oauth_version", "1.0");
			parameters.Add("oauth_signature_method", "HMAC-SHA1");

			if (!String.IsNullOrEmpty(token))
				parameters.Add("oauth_token", token);

			string signature = oauth.GenerateSignature(source, parameters, consumerKey, consumerSecret, token, tokenSecret, "POST", timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1);

			parameters.Add("oauth_signature", signature);

			StringBuilder requestBuilder = new StringBuilder(512);
			foreach (string key in parameters)
			{
				if (requestBuilder.Length != 0)
					requestBuilder.Append("&");

				requestBuilder.Append(key);
				requestBuilder.Append("=");
				requestBuilder.Append(OAuthBase.UrlEncode(parameters[key]));
			}

			return requestBuilder.ToString();
		}
	}
}