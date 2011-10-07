using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Inbox2.Channels.Facebook.REST.DataContracts;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Logging;

namespace Inbox2.Channels.Facebook.REST
{
	public class FacebookRESTClient : IDisposable
	{
		protected IFacebookClient channel;
		protected readonly string apiKey;
		protected readonly string apiSecret;

		protected string sessionKey;
		protected string sessionSecret;
		protected string uid;

		public FacebookRESTClient(string apiKey, string apiSecret)
			: this(apiKey, apiSecret, null, null)
		{
		}

		public FacebookRESTClient(string apiKey, string apiSecret, string sessionKey, string sessionSecret)
		{
			// Note: use ApiSecret as the sessionSecret with web apps, desktop apps would use the SessionSecret

			channel = ChannelHelper.BuildChannel();

			this.apiKey = apiKey;
			this.apiSecret = apiSecret;
			this.sessionKey = sessionKey;
			this.sessionSecret = sessionSecret;
		}

		public FbAuth Authenticate()
		{
			// Allready logged in
			if (!String.IsNullOrEmpty(sessionKey))
				return FbAuth.Success;

			string authToken = ChannelContext.Current.ClientContext.GetSetting("/Channels/Facebook/AuthToken").ToString();

			if (String.IsNullOrEmpty(authToken))
				return FbAuth.NoAuthKey;

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "auth.getSession");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("v", "1.0");
			requestParams.Add("auth_token", authToken);

			var result = channel.GetSession(apiKey, GenerateSignature(requestParams, apiSecret), authToken);

			foreach (XElement resultElement in result.Elements())
			{
				if (resultElement.Name.LocalName.Equals("error_code"))
					return FbAuth.Error;

				if (resultElement.Name.LocalName.Equals("session_key"))
					sessionKey = resultElement.Value;
				else if (resultElement.Name.LocalName.Equals("uid"))
					uid = resultElement.Value;
				else if (resultElement.Name.LocalName.Equals("secret"))
					sessionSecret = resultElement.Value;
			}

			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Facebook/SessionKey", sessionKey);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Facebook/SessionSecret", sessionSecret);
			ChannelContext.Current.ClientContext.DeleteSetting("/Channels/Facebook/AuthToken");

			return FbAuth.Success;
		}

		public FbContact GetLoggedInUser()
		{
			Authenticate();

			string call_id = GetNextCallNr();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "users.getloggedinuser");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");

			var result = channel.GetLoggedInUser(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret));
			var userId = result.Value;

			return GetUsersInfo(userId).First();
		}

		public IEnumerable<FbContact> GetContacts()
		{
			Authenticate();

			string call_id = GetNextCallNr();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "friends.get");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");

			var result = channel.GetContacts(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret));

			XNamespace ns = result.GetDefaultNamespace();

			List<string> friendsList = result.Elements().Select(e => e.Value).ToList();

			for (int i = 0; i < friendsList.Count; i += 10)
			{
				// Creates lists like id1,id2,id3, etc
				var ids = String.Join(",", friendsList.Skip(i).Take(10).ToArray());

				// Now get the desired data for user's contact to define the attributes of ChannelContact object
				foreach (var info in GetUsersInfo(ids))
				{
					yield return info;
				}
			}
		}

		public IEnumerable<FbContact> GetUsersInfo(string ids)
		{
			Authenticate();

			string call_id = GetNextCallNr();

			Dictionary<string, string> requestParamsUserInfo = new Dictionary<string, string>();
			requestParamsUserInfo.Add("method", "users.getinfo");
			requestParamsUserInfo.Add("api_key", apiKey);
			requestParamsUserInfo.Add("session_key", sessionKey);
			requestParamsUserInfo.Add("call_id", call_id);
			requestParamsUserInfo.Add("v", "1.0");
			requestParamsUserInfo.Add("uids", ids);
			requestParamsUserInfo.Add("fields", "name,last_name,first_name,pic_square");

			XElement result = channel.GetUserInfo(apiKey, sessionKey, call_id, GenerateSignature(requestParamsUserInfo, sessionSecret), ids, "name,last_name,first_name,pic_square");
			XNamespace ns = result.GetDefaultNamespace();

			if (!IsError(result))
			{
				foreach (XElement resultElement in result.Elements())
				{
					FbContact contact = new FbContact();

					try
					{
						contact.UserId = resultElement.Element(ns + "uid").Value;
						contact.Name = resultElement.Element(ns + "name").Value;
						contact.Lastname = resultElement.Element(ns + "last_name").Value;
						contact.Firstname = resultElement.Element(ns + "first_name").Value;
						contact.AvatarSquareUrl = resultElement.Element(ns + "pic_square").Value;

					}
					catch (NullReferenceException e)
					{
						Logger.Error("Unable to retreive contact details for contact. Uid = {0}", LogSource.Channel, resultElement.Value);

						continue;
					}

					yield return contact;
				}
			}
		}

		public IEnumerable<FbMessage> GetMessages(FbMessageFolder folder)
		{
			Authenticate();

			string call_id = GetNextCallNr();
			var query = new StringBuilder();
			query.Append("{");
			query.Append("\"threads\":\"SELECT thread_id, subject, recipients, updated_time, parent_message_id, parent_thread_id, unread FROM thread WHERE folder_id = 0\",");
			query.Append("\"messages\":\"SELECT thread_id, body, author_id, created_time, attachment FROM message WHERE thread_id IN (SELECT thread_id FROM #threads)\",");
			query.Append("\"users\":\"SELECT uid, name FROM user WHERE uid IN (SELECT author_id FROM #messages)\"");
			query.Append("}");

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "fql.multiquery");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");
			requestParams.Add("queries", query.ToString());

			var result = channel.ExecuteMultiQueries(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), query.ToString());
			XNamespace ns = result.GetDefaultNamespace();

			// Fire one call to retreive names for all recipients in messages
			var uids = result.Descendants(ns + "recipients").Elements(ns + "uid").Select(n => n.Value).ToArray();
			var addresses = GetAddresses(String.Join(", ", uids)).ToList();

			foreach (XElement messageElement in result.Descendants(ns + "message"))
			{
				FbMessage message = new FbMessage();

				try
				{
					var threadid = messageElement.Element(ns + "thread_id").Value;
					var from = messageElement.Element(ns + "author_id").Value;

					//if (String.IsNullOrEmpty(from))
					//	System.Diagnostics.Debugger.Break();					

					// Find the associated thread which contains the subject and readstate
					var threadElement = result.Descendants(ns + "thread").FirstOrDefault(t => t.Element(ns + "thread_id").Value == threadid);
					var senderElement = result.Descendants(ns + "user").FirstOrDefault(u => u.Element(ns + "uid").Value == from);

					if (threadElement == null || senderElement == null)
					{
						Logger.Error("Unable to determine sender for Facebook message, ignoring", LogSource.Channel);

						continue;
					}

					message.ThreadId = threadid;
					message.Subject = threadElement.Element(ns + "subject").Value;
					message.Body = messageElement.Element(ns + "body").Value;
					message.From = new SourceAddress(senderElement.Element(ns + "uid").Value, senderElement.Element(ns + "name").Value);
					message.To = new SourceAddressCollection();
					message.Read = threadElement.Element(ns + "unread").Value == "0";
					message.DateCreated = Int64.Parse(threadElement.Element(ns + "updated_time").Value).ToUnixTime();

					foreach (XElement recipientElement in threadElement.Element(ns + "recipients").Elements())
						message.To.Add(addresses.FirstOrDefault(a => a.Address == recipientElement.Value));

					message.MessageId = message.ThreadId + message.From.Address;
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to retreive mesaage. Result = {0}. Exception = {1}", LogSource.Channel, messageElement.Value, ex);

					continue;
				}

				yield return message;
			}
		}

		public IEnumerable<SourceAddress> GetAddresses(string uids)
		{
			Authenticate();

			string call_id = GetNextCallNr();
			string query = String.Format("SELECT uid, name FROM user WHERE uid IN ({0})", uids);

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "fql.query");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");
			requestParams.Add("query", query);

			var result = channel.ExecuteQuery(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), query);

			XNamespace ns = result.GetDefaultNamespace();

			foreach (XElement element in result.Descendants(ns + "user"))
			{
				SourceAddress address;

				try
				{
					address = new SourceAddress(element.Element(ns + "uid").Value, element.Element(ns + "name").Value);
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to retreive user source address. Result = {0}. Exception = {1}", LogSource.Channel, element.Value, ex);

					continue;
				}

				yield return address;
			}
		}

		public IEnumerable<FbStatus> GetStatusses(int pageSize)
		{
			Authenticate();

			string call_id = GetNextCallNr();
			string limit = pageSize.ToString();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "stream.get");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("source_ids", "");
			requestParams.Add("v", "1.0");
			requestParams.Add("limit", limit);

			var result = channel.GetStream(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), "", limit);

			XNamespace ns = result.GetDefaultNamespace();

			foreach (XElement element in result.Descendants(ns + "stream_post"))
			{
				var status = new FbStatus();

				try
				{
					var id = element.Element(ns + "actor_id").Value;
					var userElement = result.Descendants(ns + "profile").First(p => p.Element(ns + "id").Value == id);

					status.From = new SourceAddress(id, userElement.Element(ns + "name").Value,
						userElement.Element(ns + "pic_square").Value);

					if (element.Element(ns + "target_id") != null && !String.IsNullOrEmpty(element.Element(ns + "target_id").Value))
					{
						var toid = element.Element(ns + "target_id").Value;

						if (!String.IsNullOrEmpty(toid))
						{
							var toUserElement = result.Descendants(ns + "profile").First(p => p.Element(ns + "id").Value == toid);

							status.To = new SourceAddress(toid, toUserElement.Element(ns + "name").Value,
								toUserElement.Element(ns + "pic_square").Value);
						}
					}

					status.Uid = Int64.Parse(element.Element(ns + "actor_id").Value);
					status.StatusId = element.Element(ns + "post_id").Value;
					status.Message = element.Element(ns + "message").Value;
					status.DateCreated = Int64.Parse(element.Element(ns + "created_time").Value).ToUnixTime();

					foreach (var commentElement in element.Descendants(ns + "comment"))
					{
						var comment = new FbStatus();
						var commentid = commentElement.Element(ns + "fromid").Value;
						var commentUserElement = result.Descendants(ns + "profile").First(p => p.Element(ns + "id").Value == commentid);

						comment.From = new SourceAddress(commentid, commentUserElement.Element(ns + "name").Value,
							commentUserElement.Element(ns + "pic_square").Value);

						comment.Uid = Int64.Parse(commentElement.Element(ns + "fromid").Value);
						comment.StatusId = commentElement.Element(ns + "id").Value;
						comment.Message = commentElement.Element(ns + "text").Value;
						comment.DateCreated = Int64.Parse(commentElement.Element(ns + "time").Value).ToUnixTime();

						status.Comments.Add(comment);
					}

					foreach (var attachmentElement in element.Descendants(ns + "stream_media"))
					{
						var attachment = new FbAttachment();
						attachment.MediaType = (FbMediaType)Enum.Parse(typeof(FbMediaType), attachmentElement.Element(ns + "type").Value, true);

						switch (attachment.MediaType)
						{
							case FbMediaType.Link:
								{
									attachment.TargetUrl = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "href").Value);
									attachment.PreviewAltText = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "alt").Value);
									attachment.PreviewImageUrl = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "src").Value);

									break;
								}
							case FbMediaType.Photo:
								{
									attachment.TargetUrl = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "href").Value);
									attachment.PreviewAltText = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "alt").Value);
									attachment.PreviewImageUrl = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "src").Value);

									break;
								}
							case FbMediaType.Video:
								{
									var src = new Uri(attachmentElement.Element(ns + "src").Value);
									var uriParams = NameValueParser.GetCollection(src.Query, "&");

									attachment.TargetUrl = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "video").Element(ns + "display_url").Value);
									attachment.PreviewAltText = HttpUtility.HtmlDecode(attachmentElement.Element(ns + "alt").Value);
									attachment.PreviewImageUrl = HttpUtility.UrlDecode(uriParams["url"]);

									break;
								}
						}

						status.Attachments.Add(attachment);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to retreive user source address. Result = {0}. Exception = {1}", LogSource.Channel, element.Value, ex);

					continue;
				}

				yield return status;
			}
		}

		public IEnumerable<FbStatus> GetStatusses(string userid, int pageSize)
		{
			Authenticate();

			string call_id = GetNextCallNr();
			string limit = pageSize.ToString();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "stream.get");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("source_ids", userid);
			requestParams.Add("v", "1.0");
			requestParams.Add("limit", limit);

			var result = channel.GetStream(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), userid, limit);

			XNamespace ns = result.GetDefaultNamespace();

			foreach (XElement element in result.Descendants(ns + "stream_post"))
			{
				var status = new FbStatus();
				string id;

				try
				{
					id = element.Element(ns + "actor_id").Value;

					var userElement = result.Descendants(ns + "profile").First(p => p.Element(ns + "id").Value == id);

					status.From = new SourceAddress(id, userElement.Element(ns + "name").Value,
						userElement.Element(ns + "pic_square").Value);

					if (element.Element(ns + "target_id") != null && !String.IsNullOrEmpty(element.Element(ns + "target_id").Value))
					{
						var toid = element.Element(ns + "target_id").Value;

						if (!String.IsNullOrEmpty(toid))
						{
							var toUserElement = result.Descendants(ns + "profile").First(p => p.Element(ns + "id").Value == toid);

							status.To = new SourceAddress(toid, toUserElement.Element(ns + "name").Value,
								toUserElement.Element(ns + "pic_square").Value);
						}
					}

					status.Uid = Int64.Parse(element.Element(ns + "actor_id").Value);
					status.StatusId = element.Element(ns + "post_id").Value;
					status.Message = element.Element(ns + "message").Value;
					status.DateCreated = Int64.Parse(element.Element(ns + "created_time").Value).ToUnixTime();
				}
				catch (Exception ex)
				{
					Logger.Error("Unable to retreive user source address. Result = {0}. Exception = {1}", LogSource.Channel, element.Value, ex);

					continue;
				}

				// If id and actorid are not equal then this is a message directed at user we are
				// looking at by someone else, in that case skip it as we are only interested in updates
				// by this specific user.
				if (userid == id)
					yield return status;
			}
		}

		public void SendNotification(string to, string notification)
		{
			Authenticate();

			string call_id = GetNextCallNr();

			// Now get the desired data for user's contact to define the attributes of ChannelContact object
			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "notifications.send");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");
			requestParams.Add("to_ids", to);
			requestParams.Add("notification", notification);

			//var result = channel.SendNotification(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), to, notification);

			var queryString =
				String.Format(
					"?method=notifications.send&api_key={0}&session_key={1}&call_id={2}&sig={3}&v=1.0&to_ids={4}&notification={5}",
					apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), to, notification);

			WebClient wc = new WebClient();
			wc.DownloadString("http://api.facebook.com/restserver.php" + queryString);
		}

		public void SetStatus(string status)
		{
			Authenticate();

			string call_id = GetNextCallNr();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "status.set");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");
			requestParams.Add("status", status);

			var result = channel.SetStatus(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), status);

			if (IsError(result))
			{
				Logger.Error("Unable to update status. Error = {0}", LogSource.Channel, result);
			}
		}

		public void PostComment(string comment, string inReplyTo)
		{
			Authenticate();

			string call_id = GetNextCallNr();

			Dictionary<string, string> requestParams = new Dictionary<string, string>();
			requestParams.Add("method", "stream.addcomment");
			requestParams.Add("api_key", apiKey);
			requestParams.Add("session_key", sessionKey);
			requestParams.Add("call_id", call_id);
			requestParams.Add("v", "1.0");
			requestParams.Add("post_id", inReplyTo);
			requestParams.Add("comment", comment);

			var result = channel.AddComment(apiKey, sessionKey, call_id, GenerateSignature(requestParams, sessionSecret), inReplyTo, comment);

			if (IsError(result))
			{
				Logger.Error("Unable to update status. Error = {0}", LogSource.Channel, result);
			}
		}

		public void Dispose()
		{
			((IDisposable)channel).Dispose();

			channel = null;
		}

		#region Helper methods

		bool IsError(XElement resultElement)
		{
			var ns = resultElement.GetDefaultNamespace();

			return (resultElement.Element(ns + "error_code") != null);
		}

		string GenerateSignature(IDictionary<string, string> args, string currentSecret)
		{
			SortedDictionary<string, string> sortedD = new SortedDictionary<string, string>(args);
			List<string> argsAsKvpStrings = ConvertParameterDictionaryToList(sortedD);

			StringBuilder signatureBuilder = new StringBuilder();
			// Append all the parameters to the signature input paramaters
			foreach (string s in argsAsKvpStrings)
				signatureBuilder.Append(s);

			// Append the secret to the signature builder
			signatureBuilder.Append(currentSecret);
			byte[] hash;

			MD5 md5 = MD5.Create();
			// Compute the MD5 hash of the signature builder
			hash = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureBuilder.ToString()));

			// Reinitialize the signature builder to store the actual signature
			signatureBuilder = new StringBuilder();

			// Append the hash to the signature
			for (int i = 0; i < hash.Length; i++)
				signatureBuilder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));

			return signatureBuilder.ToString();
		}

		List<string> ConvertParameterDictionaryToList(IDictionary<string, string> parameterDictionary)
		{
			List<string> parameters = new List<string>();

			foreach (KeyValuePair<string, string> kvp in parameterDictionary)
			{
				parameters.Add(String.Format(CultureInfo.InvariantCulture, "{0}={1}", kvp.Key, kvp.Value));
			}
			return parameters;
		}

		string GetNextCallNr()
		{
			return DateTime.Now.Ticks.ToString();
		}

		#endregion
	}
}