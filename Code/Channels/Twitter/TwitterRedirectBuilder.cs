using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework;

namespace Inbox2.Channels.Twitter
{
	public class TwitterRedirectBuilder : IWebRedirectBuilder
	{
		private string username;

		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public string SuccessUri
		{
			get { return "http://jump.inbox2.com/channels/twitter"; }
		}

		public TwitterRedirectBuilder()
		{
			Token = SafeSession.Current["/Channels/Twitter/Redirect/Token"] as string;
			TokenSecret = SafeSession.Current["/Channels/Twitter/Redirect/TokenSecret"] as string;
		}	

		public Uri BuildRedirectUri()
		{
			var result = TwitterWebRequest.PerformRequest(new Uri("http://api.twitter.com/oauth/request_token"), 
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret);
			var parts = NameValueParser.GetCollection(result, "&");

			Token = parts["oauth_token"];
			TokenSecret = parts["oauth_token_secret"];

			// Save data in session for re-materialization
			SafeSession.Current["/Channels/Twitter/Redirect/Token"] = Token;
			SafeSession.Current["/Channels/Twitter/Redirect/TokenSecret"] = TokenSecret;

			return new Uri("https://api.twitter.com/oauth/authorize?oauth_token=" + Token);
		}

		public string ParseVerifier(string returnValue)
		{
			return NameValueParser.GetCollection(new Uri(returnValue).Query, "&")["oauth_verifier"];
		}

		public bool ValidateReturnValue(string verifier)
		{
			// 2. Exchange auth.token for access.token
			var result = TwitterWebRequest.PerformRequest(new Uri("https://api.twitter.com/oauth/access_token?callback_token=" + verifier),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, Token, TokenSecret, verifier);

			var parts = NameValueParser.GetCollection(result, "&");

			if (ChannelContext.Current == null)
				throw new ArgumentNullException("ChannelContext.Current");

			// Remove token from session
			SafeSession.Current.Remove("/Channels/Twitter/Redirect/Token");
			SafeSession.Current.Remove("/Channels/Twitter/Redirect/TokenSecret");

			// 7. Save auth keys
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Twitter/AuthToken", parts["oauth_token"]);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Twitter/AuthSecret", parts["oauth_token_secret"]);

			Token = parts["oauth_token"];
			TokenSecret = parts["oauth_token_secret"];

			username = parts["screen_name"];

			return true;
		}

		public string GetUsername()
		{
			return username;
		}
	}
}
