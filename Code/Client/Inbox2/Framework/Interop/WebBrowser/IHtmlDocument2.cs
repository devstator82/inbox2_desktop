using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[Guid( "332C4425-26CB-11D0-B483-00C04FD90119" ), ComVisible( true ), SuppressUnmanagedCodeSecurity, InterfaceType( ComInterfaceType.InterfaceIsDual )]
	internal interface IHTMLDocument2
	{
/*		[return: MarshalAs( UnmanagedType.IDispatch )]
		object GetScript();
		UnsafeIHTMLElementCollection GetAll();
		UnsafeIHTMLElement GetBody();
		UnsafeIHTMLElement GetActiveElement();
		UnsafeIHTMLElementCollection GetImages();
		UnsafeIHTMLElementCollection GetApplets();
		UnsafeIHTMLElementCollection GetLinks();
		UnsafeIHTMLElementCollection GetForms();
		UnsafeIHTMLElementCollection GetAnchors();
		void SetTitle( string p );
		string GetTitle();
		UnsafeIHTMLElementCollection GetScripts();
		void SetDesignMode( string p );
		string GetDesignMode();
		[return: MarshalAs( UnmanagedType.Interface )]
		object GetSelection();
		string GetReadyState();
		[return: MarshalAs( UnmanagedType.Interface )]
		object GetFrames();
		UnsafeIHTMLElementCollection GetEmbeds();
		UnsafeIHTMLElementCollection GetPlugins();
		void SetAlinkColor( object c );
		object GetAlinkColor();
		void SetBgColor( object c );
		object GetBgColor();
		void SetFgColor( object c );
		object GetFgColor();
		void SetLinkColor( object c );
		object GetLinkColor();
		void SetVlinkColor( object c );
		object GetVlinkColor();
		string GetReferrer();
		[return: MarshalAs( UnmanagedType.Interface )]
		UnsafeIHTMLLocation GetLocation();
		string GetLastModified();
		void SetUrl( string p );
		string GetUrl();
		void SetDomain( string p );
		string GetDomain();
		void SetCookie( string p );
		string GetCookie();
		void SetExpando( bool p );
		bool GetExpando();
		void SetCharset( string p );
		string GetCharset();
		void SetDefaultCharset( string p );
		string GetDefaultCharset();
		string GetMimeType();
		string GetFileSize();
		string GetFileCreatedDate();
		string GetFileModifiedDate();
		string GetFileUpdatedDate();
		string GetSecurity();
		string GetProtocol();
		string GetNameProp();
		int Write( [In, MarshalAs( UnmanagedType.SafeArray )] object[] psarray );
		int WriteLine( [In, MarshalAs( UnmanagedType.SafeArray )] object[] psarray );
		[return: MarshalAs( UnmanagedType.Interface )]
		object Open( string mimeExtension, object name, object features, object replace );
		void Close();
		void Clear();
		bool QueryCommandSupported( string cmdID );
		bool QueryCommandEnabled( string cmdID );
		bool QueryCommandState( string cmdID );
		bool QueryCommandIndeterm( string cmdID );
		string QueryCommandText( string cmdID );
		object QueryCommandValue( string cmdID );
		bool ExecCommand( string cmdID, bool showUI, object value );
		bool ExecCommandShowHelp( string cmdID );
		UnsafeIHTMLElement CreateElement( string eTag );
		void SetOnhelp( object p );
		object GetOnhelp();
		void SetOnclick( object p );
		object GetOnclick();
		void SetOndblclick( object p );
		object GetOndblclick();
		void SetOnkeyup( object p );
		object GetOnkeyup();
		void SetOnkeydown( object p );
		object GetOnkeydown();
		void SetOnkeypress( object p );
		object GetOnkeypress();
		void SetOnmouseup( object p );
		object GetOnmouseup();
		void SetOnmousedown( object p );
		object GetOnmousedown();
		void SetOnmousemove( object p );
		object GetOnmousemove();
		void SetOnmouseout( object p );
		object GetOnmouseout();
		void SetOnmouseover( object p );
		object GetOnmouseover();
		void SetOnreadystatechange( object p );
		object GetOnreadystatechange();
		void SetOnafterupdate( object p );
		object GetOnafterupdate();
		void SetOnrowexit( object p );
		object GetOnrowexit();
		void SetOnrowenter( object p );
		object GetOnrowenter();
		void SetOndragstart( object p );
		object GetOndragstart();
		void SetOnselectstart( object p );
		object GetOnselectstart();
		UnsafeIHTMLElement ElementFromPoint( int x, int y );
		[return: MarshalAs( UnmanagedType.Interface )]
		UnsafeIHTMLWindow2 GetParentWindow();
		[return: MarshalAs( UnmanagedType.Interface )]
		object GetStyleSheets();
		void SetOnbeforeupdate( object p );
		object GetOnbeforeupdate();
		void SetOnerrorupdate( object p );
		object GetOnerrorupdate();
		string toString();
		[return: MarshalAs( UnmanagedType.Interface )]
		object CreateStyleSheet( string bstrHref, int lIndex );*/
	}
}