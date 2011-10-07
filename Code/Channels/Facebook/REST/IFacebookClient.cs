using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Linq;

namespace Inbox2.Channels.Facebook.REST
{
	[ServiceContract]
	[XmlSerializerFormat]
	public interface IFacebookClient
	{
		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare, 
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=auth.createToken&api_key={api_key}&sig={signature}&v=1.0")]
		XElement CreateToken(string api_key, string signature);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=auth.getSession&api_key={api_key}&sig={signature}&v=1.0&auth_token={auth_token}")]
		XElement GetSession(string api_key, string signature, string auth_token);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=users.getloggedinuser&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0")]
		XElement GetLoggedInUser(string api_key, string session_key, string call_id, string signature);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=friends.get&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0")]
		XElement GetContacts(string api_key, string session_key, string call_id, string signature);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=users.getinfo&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&uids={uids}&fields={fields}")]
		XElement GetUserInfo(string api_key, string session_key, string call_id, string signature, string uids, string fields);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=notifications.get&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0")]
		XElement GetNotifications(string api_key, string session_key, string call_id, string signature);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=stream.get&api_key={api_key}&session_key={session_key}&call_id={call_id}&source_ids={source_ids}&sig={signature}&v=1.0&limit={limit}")]
		XElement GetStream(string api_key, string session_key, string call_id, string signature, string source_ids, string limit);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=fql.query&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&query={query}")]
		XElement ExecuteQuery(string api_key, string session_key, string call_id, string signature, string query);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=fql.multiquery&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&queries={queries}")]
		XElement ExecuteMultiQueries(string api_key, string session_key, string call_id, string signature, string queries);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=status.set&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&status={status}")]
		XElement SetStatus(string api_key, string session_key, string call_id, string signature, string status);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=stream.addcomment&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&post_id={post_id}&comment={comment}")]
		XElement AddComment(string api_key, string session_key, string call_id, string signature, string post_id, string comment);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Bare,
			RequestFormat = WebMessageFormat.Xml,
			ResponseFormat = WebMessageFormat.Xml,
			UriTemplate = "?method=notifications.send&api_key={api_key}&session_key={session_key}&call_id={call_id}&sig={signature}&v=1.0&to_ids={to_ids}&notification={notification}")]
		XElement SendNotification(string api_key, string session_key, string call_id, string signature, string to_ids, string notification);
	}
}