using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace Inbox2.Framework.Interop.WebBrowser
{
	[InterfaceType( ComInterfaceType.InterfaceIsDual ), ComVisible( true ), SuppressUnmanagedCodeSecurity, Guid( "163BB1E0-6E00-11CF-837A-48DC04C10000" )]
	internal interface IHTMLLocation
	{
		void SetHref( [In] string p );
		string GetHref();
		void SetProtocol( [In] string p );
		string GetProtocol();
		void SetHost( [In] string p );
		string GetHost();
		void SetHostname( [In] string p );
		string GetHostname();
		void SetPort( [In] string p );
		string GetPort();
		void SetPathname( [In] string p );
		string GetPathname();
		void SetSearch( [In] string p );
		string GetSearch();
		void SetHash( [In] string p );
		string GetHash();
		void Reload( [In] bool flag );
		void Replace( [In] string bstr );
		void Assign( [In] string bstr );
	}
}