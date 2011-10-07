using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inbox2.Platform.Framework.OAuth
{
	public class OAuthBase
	{
		static readonly Random random = new Random();

		/// <summary>
		/// Provides a predefined set of algorithms that are supported officially by the protocol
		/// </summary>
		public enum SignatureTypes
		{
			HMACSHA1,
			PLAINTEXT,
			RSASHA1
		}

		/// <summary>
		/// Provides an internal structure to sort the query parameter
		/// </summary>
		protected class QueryParameter
		{
			private string name = null;
			private string value = null;

			public QueryParameter(string name, string value)
			{
				this.name = name;
				this.value = value;
			}

			public string Name
			{
				get { return name; }
			}

			public string Value
			{
				get { return value; }
			}
		}

		/// <summary>
		/// Comparer class used to perform the sorting of the query parameters
		/// </summary>
		protected class QueryParameterComparer : IComparer<QueryParameter>
		{

			#region IComparer<QueryParameter> Members

			public int Compare(QueryParameter x, QueryParameter y)
			{
				if (x.Name == y.Name)
				{
					return string.Compare(x.Value, y.Value);
				}
				else
				{
					return string.Compare(x.Name, y.Name);
				}
			}

			#endregion
		}

		protected const string OAuthVersion = "1.0";
		protected const string OAuthParameterPrefix = "oauth_";

		//
		// List of know and used oauth parameters' names
		//        
		protected const string OAuthConsumerKeyKey = "oauth_consumer_key";
		protected const string OAuthCallbackKey = "oauth_callback";
		protected const string OAuthVersionKey = "oauth_version";
		protected const string OAuthSignatureMethodKey = "oauth_signature_method";
		protected const string OAuthSignatureKey = "oauth_signature";
		protected const string OAuthTimestampKey = "oauth_timestamp";
		protected const string OAuthNonceKey = "oauth_nonce";
		protected const string OAuthTokenKey = "oauth_token";
		protected const string OAuthTokenSecretKey = "oauth_token_secret";

		protected const string HMACSHA1SignatureType = "HMAC-SHA1";
		protected const string PlainTextSignatureType = "PLAINTEXT";
		protected const string RSASHA1SignatureType = "RSA-SHA1";

		protected const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

		/// <summary>
		/// Helper function to compute a hash value
		/// </summary>
		/// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
		/// <param name="data">The data to hash</param>
		/// <returns>a Base64 string of the hash value</returns>
		private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
		{
			if (hashAlgorithm == null)
			{
				throw new ArgumentNullException("hashAlgorithm");
			}

			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("data");
			}

			byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
			byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

			return Convert.ToBase64String(hashBytes);
		}

		/// <summary>
		/// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
		/// </summary>
		/// <param name="parameters">The query string part of the Url</param>
		/// <returns>A list of QueryParameter each containing the parameter name and value</returns>
		private List<QueryParameter> GetQueryParameters(string parameters)
		{
			if (parameters.StartsWith("?"))
			{
				parameters = parameters.Remove(0, 1);
			}

			List<QueryParameter> result = new List<QueryParameter>();

			if (!string.IsNullOrEmpty(parameters))
			{
				string[] p = parameters.Split('&');
				foreach (string s in p)
				{
					if (!string.IsNullOrEmpty(s) && !s.StartsWith(OAuthParameterPrefix))
					{
						if (s.IndexOf('=') > -1)
						{
							string[] temp = s.Split('=');
							result.Add(new QueryParameter(temp[0], temp[1]));
						}
						else
						{
							result.Add(new QueryParameter(s, string.Empty));
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
		/// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
		/// </summary>
		/// <param name="value">The value to Url encode</param>
		/// <returns>Returns a Url encoded string</returns>
		public static string UrlEncode(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			StringBuilder result = new StringBuilder();

			foreach (char symbol in value)
			{
				if (UnreservedChars.IndexOf(symbol) != -1)
				{
					result.Append(symbol);
				}
				else
				{
					result.Append('%' + String.Format("{0:X2}", (int)symbol));
				}
			}

			return result.ToString();
		}

		/// <summary>
		/// Normalizes the request parameters accoriding to the spec
		/// </summary>
		/// <param name="parameters">The list of parameters already sorted</param>
		/// <returns>a string representing the normalized parameters</returns>
		protected string NormalizeRequestParameters(IList<QueryParameter> parameters)
		{
			StringBuilder sb = new StringBuilder();
			QueryParameter p = null;
			for (int i = 0; i < parameters.Count; i++)
			{
				p = parameters[i];
				sb.AppendFormat("{0}={1}", UrlEncode(p.Name), UrlEncode(p.Value));

				if (i < parameters.Count - 1)
				{
					sb.Append("&");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Generate the signature base that is used to produce the signature
		/// </summary>
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="parameters"></param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="consumerSecret">The consumer seceret</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param>
		/// <returns>The signature base</returns>
		public string GenerateSignatureBase(Uri url, NameValueCollection parameters, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, string signatureType)
		{
			if (token == null)
			{
				token = string.Empty;
			}

			if (tokenSecret == null)
			{
				tokenSecret = string.Empty;
			}

			if (string.IsNullOrEmpty(consumerKey))
			{
				throw new ArgumentNullException("consumerKey");
			}

			if (string.IsNullOrEmpty(consumerSecret))
			{
				throw new ArgumentNullException("consumerSecret");
			}

			if (string.IsNullOrEmpty(httpMethod))
			{
				throw new ArgumentNullException("httpMethod");
			}

			if (string.IsNullOrEmpty(signatureType))
			{
				throw new ArgumentNullException("signatureType");
			}

			List<QueryParameter> queryParameters = new List<QueryParameter>();
			/*List<QueryParameter> parameters = GetQueryParameters(url.Query);
			parameters.Add(new QueryParameter(OAuthVersionKey, UrlEncode(OAuthVersion)));
			parameters.Add(new QueryParameter(OAuthNonceKey, nonce));
			parameters.Add(new QueryParameter(OAuthTimestampKey, timeStamp));
			parameters.Add(new QueryParameter(OAuthSignatureMethodKey, UrlEncode(signatureType)));
			parameters.Add(new QueryParameter(OAuthConsumerKeyKey, UrlEncode(consumerKey)));

			if (!string.IsNullOrEmpty(token))
			{
				parameters.Add(new QueryParameter(OAuthTokenKey, UrlEncode(token)));
			}*/

			

			foreach (string key in parameters.AllKeys)
			{
				queryParameters.Add(new QueryParameter(key, parameters[key]));
			}


			queryParameters.Sort(new QueryParameterComparer());

			string normalizedRequestParameters = NormalizeRequestParameters(queryParameters);

			StringBuilder signatureBase = new StringBuilder();
			signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
			signatureBase.AppendFormat("{0}&", UrlEncode(string.Format("{0}://{1}{2}", url.Scheme, url.Host, url.AbsolutePath)));
			signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));
			//signatureBase.AppendFormat("{0}&", UrlEncode(consumerSecret));
			//signatureBase.AppendFormat("{0}", UrlEncode(tokenSecret));

			return signatureBase.ToString();
		}

		/// <summary>
		/// Generate the signature value based on the given signature base and hash algorithm
		/// </summary>
		/// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param>
		/// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param>
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
		{
			return ComputeHash(hash, signatureBase);
		}

		/// <summary>
		/// Generates a signature using the HMAC-SHA1 algorithm
		/// </summary>
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="consumerSecret">The consumer seceret</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignature(Uri url, NameValueCollection parameters, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce)
		{
			return GenerateSignature(url, parameters, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, nonce, SignatureTypes.HMACSHA1);
		}

		/// <summary>
		/// Generates a signature using the specified signatureType 
		/// </summary>
		/// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
		/// <param name="consumerKey">The consumer key</param>
		/// <param name="consumerSecret">The consumer seceret</param>
		/// <param name="token">The token, if available. If not available pass null or an empty string</param>
		/// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
		/// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
		/// <param name="signatureType">The type of signature to use</param>
		/// <returns>A base64 string of the hash value</returns>
		public string GenerateSignature(Uri url, NameValueCollection parameters, string consumerKey, string consumerSecret, string token, string tokenSecret, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType)
		{
			switch (signatureType)
			{
				case SignatureTypes.PLAINTEXT:
					return UrlEncode(string.Format("{0}&{1}", consumerSecret, tokenSecret));

				case SignatureTypes.HMACSHA1:
					string signatureBase = GenerateSignatureBase(url, parameters, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, nonce, HMACSHA1SignatureType);

					HMACSHA1 hmacsha1 = new HMACSHA1();
					hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), UrlEncode(tokenSecret)));

					return GenerateSignatureUsingHash(signatureBase, hmacsha1);

				case SignatureTypes.RSASHA1:
					throw new NotImplementedException();
				default:
					throw new ArgumentException("Unknown signature type", "signatureType");
			}
		}

		/// <summary>
		/// Generate the timestamp for the signature        
		/// </summary>
		/// <returns></returns>
		public static string GenerateTimeStamp()
		{
			TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

			return ((int)t.TotalSeconds).ToString();
		}

		/// <summary>
		/// Generate a nonce
		/// </summary>
		/// <returns></returns>
		public static string GenerateNonce()
		{		
			// Just a simple implementation of a random number between 123400 and 9999999
			return random.Next(123400, 9999999).ToString();
		}
	}
}