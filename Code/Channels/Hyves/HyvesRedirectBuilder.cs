using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Framework;
using Inbox2.Platform.Framework.Extensions;

namespace Inbox2.Channels.Hyves
{
	public class HyvesRedirectBuilder : IWebRedirectBuilder
	{
		public string Token { get; set; }

		public string TokenSecret { get; set; }

		public string SuccessUri
		{
			get { return "http://jump.inbox2.com/channels/hyves"; }
		}

		public HyvesRedirectBuilder()
		{
			Token = SafeSession.Current["/Channels/Hyves/Redirect/Token"] as string;
			TokenSecret = SafeSession.Current["/Channels/Hyves/Redirect/TokenSecret"] as string;
		}

		public Uri BuildRedirectUri()
		{
			// 1. Get Auth Token
			Uri uri = new Uri(String.Format("http://data.hyves-api.nl/?methods={0}&expirationtype=infinite&ha_method=auth.requesttoken&ha_version=experimental&ha_format=xml&ha_fancylayout=false", HyvesApiRequest.AllMethods));

			XElement result = HyvesApiRequest.PerformRequest(uri);

			Token = result.Element("oauth_token").Value;
			TokenSecret = result.Element("oauth_token_secret").Value;

			// Save data in session for re-materialization
			SafeSession.Current["/Channels/Hyves/Redirect/Token"] = Token;
			SafeSession.Current["/Channels/Hyves/Redirect/TokenSecret"] = TokenSecret;

			return new Uri(String.Format("http://www.hyves.nl/api/authorize/?oauth_token={0}&oauth_callback={1}", Token, BuildEnvironmentUrl()));
		}

		public string ParseVerifier(string returnValue)
		{
			return NameValueParser.GetCollection(new Uri(returnValue).Query, "&")["oauth_token"];
		}

		public bool ValidateReturnValue(string returnValue)
		{
			// 6. Exchange auth.token for access.token
			Uri uri = new Uri("http://data.hyves-api.nl/?ha_method=auth.accesstoken&ha_version=experimental&ha_format=xml&ha_fancylayout=false");

			XElement accessResult = HyvesApiRequest.PerformRequest(uri, Token, TokenSecret);

			if (ChannelContext.Current == null)
				throw new ArgumentNullException("ChannelContext.Current");

			string authorizedToken = accessResult.Element("oauth_token").Value;
			string authorizedSecret = accessResult.Element("oauth_token_secret").Value;
			string userid = accessResult.Element("userid").Value;

			// Remove token from session
			SafeSession.Current.Remove("/Channels/Hyves/Redirect/Token");
			SafeSession.Current.Remove("/Channels/Hyves/Redirect/TokenSecret");

			// 7. Save auth keys
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Hyves/AuthToken", authorizedToken);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Hyves/AuthSecret", authorizedSecret);
			ChannelContext.Current.ClientContext.SaveSetting("/Channels/Hyves/UserId", userid);

			return true;
		}

		public string GetUsername()
		{
			var friend = HyvesClientChannel.PerformRequest(
				String.Format("http://data.hyves-api.nl/?userid={0}&ha_method=users.get&ha_version=experimental&ha_format=xml&ha_fancylayout=false", 
					ChannelContext.Current.ClientContext.GetSetting("/Channels/Hyves/UserId")));

			return friend.Element("user").Element("displayname").Value;
		}

		static string BuildEnvironmentUrl()
		{
			string environment = "/Settings/Application/Environment".AsKey(String.Empty);

			if (String.IsNullOrEmpty(environment))
				return "http://jump.inbox2.com/channels/hyves";

			return String.Concat("http://jump.", environment, ".inbox2.com/channels/hyves");
		}
	}
}
