using System;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Framework.OAuth;

namespace Inbox2.Channels.Yammer
{
	public class YammerRedirectBuilder : IWebRedirectBuilder
	{
		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public string SuccessUri
		{
			get { return "oauth/authorized?consumer_token="; }
		}

		public YammerRedirectBuilder()
		{
			Token = SafeSession.Current["/Channels/Yammer/Redirect/Token"] as string;
			TokenSecret = SafeSession.Current["/Channels/Yammer/Redirect/TokenSecret"] as string;
		}

		public Uri BuildRedirectUri()
		{
			// 1. Get Auth Token
			var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/oauth/request_token"), ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret);
			var parts = NameValueParser.GetCollection(result, "&");

			Token = parts["oauth_token"];
			TokenSecret = parts["oauth_token_secret"];

			// Save data in session for re-materialization
			SafeSession.Current["/Channels/Yammer/Redirect/Token"] = Token;
			SafeSession.Current["/Channels/Yammer/Redirect/TokenSecret"] = TokenSecret;

			return new Uri("https://www.yammer.com/oauth/authorize?oauth_token=" + Token);
		}

		public string ParseVerifier(string returnValue)
		{
			return NameValueParser.GetCollection(new Uri(returnValue).Query, "&")["oauth_verifier"];
		}

		public bool ValidateReturnValue(string verifier)
		{
			// 2. Exchange auth.token for access.token
			var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/oauth/access_token?callback_token=" + verifier), 
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, Token, TokenSecret, verifier);

			var parts = NameValueParser.GetCollection(result, "&");

			if (ChannelContext.Current == null)
				throw new ArgumentNullException("ChannelContext.Current");

			// Remove token from session
			SafeSession.Current.Remove("/Channels/Yammer/Redirect/Token");
			SafeSession.Current.Remove("/Channels/Yammer/Redirect/TokenSecret");
			
			// 7. Save auth keys
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Yammer/AuthToken", parts["oauth_token"]);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Yammer/AuthSecret", parts["oauth_token_secret"]);
			
			Token = parts["oauth_token"];
			TokenSecret = parts["oauth_token_secret"];

			return true;
		}

		public string GetUsername()
		{
			var result = YammerWebRequest.PerformRequest(new Uri("https://www.yammer.com/api/v1/users/current.xml"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, Token, TokenSecret);

			return result.Element("response").Element("name").Value;
		}		
	}
}
