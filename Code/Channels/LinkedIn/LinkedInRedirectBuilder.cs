using System;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.LinkedIn
{
	public class LinkedInRedirectBuilder: IWebRedirectBuilder
	{
		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public string SuccessUri
		{
			get { return "http://jump.inbox2.com/channels/linkedin"; }
		}

		public LinkedInRedirectBuilder()
		{
			Token = SafeSession.Current["/Channels/LinkedIn/Redirect/Token"] as string;
			TokenSecret = SafeSession.Current["/Channels/LinkedIn/Redirect/TokenSecret"] as string;
		}

		public Uri BuildRedirectUri()
		{
			// 1. Get Auth Token
			var result = LinkedInWebRequest.PerformRequest(new Uri("https://api.linkedin.com/uas/oauth/requestToken"), 
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret);
			var parts = NameValueParser.GetCollection(result, "&");

			this.Token = parts["oauth_token"];
			this.TokenSecret = parts["oauth_token_secret"];

			// Save data in session for re-materialization
			SafeSession.Current["/Channels/LinkedIn/Redirect/Token"] = Token;
			SafeSession.Current["/Channels/LinkedIn/Redirect/TokenSecret"] = TokenSecret;

			return new Uri("https://api.linkedin.com/uas/oauth/authorize?oauth_token=" + Token + "&oauth_callback=" + BuildEnvironmentUrl());
		}

		public string ParseVerifier(string returnValue)
		{
			return NameValueParser.GetCollection(new Uri(returnValue).Query, "&")["oauth_verifier"];
		}

		public bool ValidateReturnValue(string verifier)
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri("https://api.linkedin.com/uas/oauth/accessToken"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, Token, TokenSecret, verifier);

			var parts = NameValueParser.GetCollection(result, "&");

			if (ChannelContext.Current == null)
				throw new ArgumentNullException("ChannelContext.Current");

			// Remove token from session
			SafeSession.Current.Remove("/Channels/LinkedIn/Redirect/Token");
			SafeSession.Current.Remove("/Channels/LinkedIn/Redirect/TokenSecret");

			// 7. Save auth keys
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/LinkedIn/AuthToken", parts["oauth_token"]);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/LinkedIn/AuthSecret", parts["oauth_token_secret"]);

			Token = parts["oauth_token"];
			TokenSecret = parts["oauth_token_secret"];

			return true;
		}

		public string GetUsername()
		{
			var result = LinkedInWebRequest.PerformRequest(new Uri("https://api.linkedin.com/v1/people/~"),
				ChannelHelper.ConsumerKey, ChannelHelper.ConsumerSecret, Token, TokenSecret);

			return result.Element("person").Element("first-name").Value + " " + result.Element("person").Element("last-name").Value;
		}

		static string BuildEnvironmentUrl()
		{
			string environment = "/Settings/Application/Environment".AsKey(String.Empty);

			if (String.IsNullOrEmpty(environment))
				return "http://jump.inbox2.com/channels/linkedin";

			return String.Concat("http://jump.", environment, ".inbox2.com/channels/linkedin");
		}
	}
}
