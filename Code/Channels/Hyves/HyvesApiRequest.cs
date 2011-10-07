using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Framework.OAuth;

namespace Inbox2.Channels.Hyves
{
	public static class HyvesApiRequest
	{
		public const string AllMethods = "cities.get,countries.get,blogs.create,blogs.createRespect,blogs.get,blogs.getByTag,blogs.getByUser,blogs.getComments,blogs.getForFriends,blogs.getRespects,fancylayout.parse,friends.get,friends.getDistance,friends.getIncomingInvitations,friends.getOutgoingInvitations,gadgets.get,gadgets.getByUser,gadgets.create,gadgets.createRespect,gadgets.getComments,gadgets.getRespects,listeners.create,listeners.delete,listeners.get,listeners.getAll,listeners.getByType,media.get,media.getAlbums,media.getByAlbum,media.getByTag,media.getComments,media.getRespects,media.createRespect,pings.get,pings.getByTargetUser,pings.getByUser,regions.get,tips.createRespect,tips.get,tips.getByUser,tips.getCategories,tips.getComments,tips.getForFriends,tips.getRespects,users.get,users.getByUsername,users.getLoggedin,users.getRespects,users.getScraps,users.getTestimonials,users.search,users.searchInFriends,users.createRespect,wwws.get,wwws.getByUser,wwws.getForFriends,wwws.create,messages.getUnreadCount,messages.delete,messages.get,messages.getInbox,messages.setRead,messages.send";

		public static XElement PerformRequest(Uri uri)
		{
			return PerformRequest(uri, null, null);
		}

		public static XElement PerformRequest(Uri uri, string token, string tokenSecret)
		{
			string data = OAuthUriBuilder.Build(uri,
				"/Settings/Channels/Hyves/ApiKey".AsKey("MTY3NV_uGz3T0QPqZ8hPKCI5Ep5m"), 
				"/Settings/Channels/Hyves/ApiSecret".AsKey("MTY3NV_y9u4XHMH0d2jFzT0eN6tX"), 
				token, 
				tokenSecret);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://data.hyves-api.nl/"));
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "inbox2";

			byte[] bytes = Encoding.UTF8.GetBytes(data);
			request.ContentLength = bytes.Length;

			// Write post data to request stream
			using (Stream requestStream = request.GetRequestStream())
				requestStream.Write(bytes, 0, bytes.Length);

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			return XElement.Parse(response.GetResponseStream().ReadString());
		}
	}
}
