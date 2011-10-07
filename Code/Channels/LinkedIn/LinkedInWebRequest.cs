using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Inbox2.Platform.Framework.OAuth;
using System.Web;
using Inbox2.Platform.Channels.Text;
using System.Xml.Linq;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.LinkedIn
{
    public class LinkedInWebRequest
    {
		public static string Put(Uri sourceUri, string data, string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
			request.Method = "PUT";
			//request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "inbox2";
			string authParams = BuildOAuthParams(sourceUri, consumerKey, consumerSecret, token, tokenSecret, null, "PUT");
			request.Headers.Add("Authorization", authParams);

			byte[] bytes = Encoding.UTF8.GetBytes(data);
			request.ContentLength = bytes.Length;

			// Write post data to request stream
			using (Stream requestStream = request.GetRequestStream())
				requestStream.Write(bytes, 0, bytes.Length);

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			using (var responseStream = response.GetResponseStream())
				return responseStream.ReadString();
		}

        public static string PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "inbox2";
            string authParams = BuildOAuthParams(sourceUri, consumerKey, consumerSecret, null, null, null);
            request.Headers.Add("Authorization", authParams);

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
            string authParams = BuildOAuthParams(sourceUri, consumerKey, consumerSecret, token, tokenSecret, verifier);
            request.Headers.Add("Authorization", authParams);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (var responseStream = response.GetResponseStream())
                return responseStream.ReadString();
        }

        public static XDocument PerformRequest(Uri sourceUri, string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sourceUri);
            request.Method = "GET";
            request.UserAgent = "inbox2";
            string authParams = BuildOAuthParams(sourceUri, consumerKey, consumerSecret, token, tokenSecret, null, "GET");
            request.Headers.Add("Authorization", authParams);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string responseString;
            using (var responseStream = response.GetResponseStream())
                responseString = responseStream.ReadString();

            return XDocument.Parse(responseString);
        }

        static string BuildOAuthParams(Uri source, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier)
        {
            return BuildOAuthParams(source, consumerKey, consumerSecret, token, tokenSecret, verifier, "POST");
        }

        static string BuildOAuthParams(Uri source, string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string httpMethod)
        {
            OAuthBase oauth = new OAuthBase();

            string timeStamp = OAuthBase.GenerateTimeStamp();
            string nonce = OAuthBase.GenerateNonce();

            // Calling source.Query returns an urlencoded string, but we don't want that since we will use
            // oauth.UrlEncode ourselves
            var query = HttpUtility.UrlDecode(source.Query.Contains("?") ? source.Query.Remove(0, 1) : source.Query);
            var parameters = NameValueParser.GetCollection(query, "&");

            parameters.Add("oauth_nonce", nonce);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");

            parameters.Add("oauth_timestamp", timeStamp);
            parameters.Add("oauth_consumer_key", consumerKey);

            if (!String.IsNullOrEmpty(token))
                parameters.Add("oauth_token", token);

            if (!String.IsNullOrEmpty(verifier))
                parameters.Add("oauth_verifier", verifier);

            parameters.Add("oauth_version", "1.0");

            StringBuilder requestBuilder = new StringBuilder();

            string signature = oauth.GenerateSignature(source, parameters, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1);

            parameters.Add("oauth_signature", signature);

            requestBuilder.Append("OAuth ");

            foreach (string key in parameters)
            {
                requestBuilder.Append(key);
                requestBuilder.Append("=");
				requestBuilder.Append(@"""" + OAuthBase.UrlEncode(parameters[key]) + @"""");

                requestBuilder.Append(", ");
            }

            return requestBuilder.ToString();
        }
    }
}
