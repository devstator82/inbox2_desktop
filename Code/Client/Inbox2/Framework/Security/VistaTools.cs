using System;
using System.Runtime.InteropServices;

//sample by Tim Anderson http://www.itwriting.com/blog

//Email: tim@itwriting.com


//THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED
//TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.


//This sample is a C# implementation of some of the functions in 
//Andrei Belogortseff [ http://www.tweak-uac.com ]
//though IsReallyVista is nothing to do with Andrei


//The intention is to make it easy for .NET developers to discover whether or not 
//UAC is enabled and/or the current process is elevated

namespace Inbox2.Framework.Security
{
	public class VistaTools
	{

		public const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
		public const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
		public const uint TOKEN_DUPLICATE = 0x0002;
		public const uint TOKEN_IMPERSONATE = 0x0004;
		public const uint TOKEN_QUERY = 0x0008;
		public const uint TOKEN_QUERY_SOURCE = 0x0010;
		public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
		public const uint TOKEN_ADJUST_GROUPS = 0x0040;
		public const uint TOKEN_ADJUST_DEFAULT = 0x0080;
		public const uint TOKEN_ADJUST_SESSIONID = 0x0100;

		public const uint TOKEN_ALL_ACCESS_P = (STANDARD_RIGHTS_REQUIRED |
												TOKEN_ASSIGN_PRIMARY |
												TOKEN_DUPLICATE |
												TOKEN_IMPERSONATE |
												TOKEN_QUERY |
												TOKEN_QUERY_SOURCE |
												TOKEN_ADJUST_PRIVILEGES |
												TOKEN_ADJUST_GROUPS |
												TOKEN_ADJUST_DEFAULT);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenProcessToken(IntPtr ProcessHandle,
											UInt32 DesiredAccess, out IntPtr TokenHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetCurrentProcess();

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetTokenInformation(
			IntPtr TokenHandle,
			TOKEN_INFORMATION_CLASS TokenInformationClass,
			IntPtr TokenInformation,
			uint TokenInformationLength,
			out uint ReturnLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = false)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr GetProcAddress(IntPtr hmodule, string procName);


		public static bool IsVista()
		{
			return (System.Environment.OSVersion.Version.Major >= 6);
		}

		public static bool IsReallyVista()
		{
			IntPtr hmodule = LoadLibrary("kernel32");

			if (hmodule.ToInt32() != 0)
			{
				//just any old API function that happens only to exist on Vista and higher
				IntPtr hProc = GetProcAddress(hmodule, "CreateThreadpoolWait");
				if (hProc.ToInt32() != 0)
				{
					return true;
				}
			}

			return false;

		}

		/// <summary>
		///
		///The possible values are:

		///TRUE - the current process is elevated.
		///	This value indicates that either UAC is enabled, and the process was elevated by 
		///	the administrator, or that UAC is disabled and the process was started by a user 
		///	who is a member of the Administrators group.

		///FALSE - the current process is not elevated (limited).
		///	This value indicates that either UAC is enabled, and the process was started normally, 
		///	without the elevation, or that UAC is disabled and the process was started by a standard user. 

		/// </summary>
		/// <returns>Bool indicating whether the current process is elevated</returns>
		public static bool IsElevated() //= NULL )
		{
			if (!IsReallyVista())
			{
				throw new VistaToolsException("Function requires Vista or higher");
			}

			bool bRetVal = false;
			IntPtr hToken = IntPtr.Zero;
			IntPtr hProcess = GetCurrentProcess();

			if (hProcess == IntPtr.Zero)
			{
				throw new VistaToolsException("Error getting current process handle");
			}

			bRetVal = OpenProcessToken(hProcess, TOKEN_QUERY, out hToken);


			if (!bRetVal)
			{
				throw new VistaToolsException("Error opening process token");
			}
			try
			{

				TOKEN_ELEVATION te;
				te.TokenIsElevated = 0;

				UInt32 dwReturnLength = 0;
				Int32 teSize = System.Runtime.InteropServices.Marshal.SizeOf(te);
				IntPtr tePtr = Marshal.AllocHGlobal(teSize);
				try
				{
					System.Runtime.InteropServices.Marshal.StructureToPtr(te, tePtr, true);

					bRetVal = GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevation, tePtr, (UInt32)teSize, out dwReturnLength);

					if ((!bRetVal) | (teSize != dwReturnLength))
					{
						throw new VistaToolsException("Error getting token information");
					}

					te = (TOKEN_ELEVATION)Marshal.PtrToStructure(tePtr, typeof(TOKEN_ELEVATION));

				}
				finally
				{
					Marshal.FreeHGlobal(tePtr);
				}

				return (te.TokenIsElevated != 0);

			}
			finally
			{
				CloseHandle(hToken);
			}

		}


		/// <summary>
		/// TokenElevationTypeDefault - User is not using a "split" token. 
		///This value indicates that either UAC is disabled, or the process is started
		///by a standard user (not a member of the Administrators group).

		///The following two values can be returned only if both the UAC is enabled and
		///the user is a member of the Administrator's group (that is, the user has a "split" token):

		///TokenElevationTypeFull - the process is running elevated. 

		///TokenElevationTypeLimited - the process is not running elevated.
		/// </summary>
		/// <returns>TokenElevationType</returns>
		public static TOKEN_ELEVATION_TYPE GetElevationType()
		{
			if (!IsReallyVista())
			{
				throw new VistaToolsException("Function requires Vista or higher");
			}

			bool bRetVal = false;
			IntPtr hToken = IntPtr.Zero;
			IntPtr hProcess = GetCurrentProcess();

			if (hProcess == IntPtr.Zero)
			{
				throw new VistaToolsException("Error getting current process handle");
			}

			bRetVal = OpenProcessToken(hProcess, TOKEN_QUERY, out hToken);


			if (!bRetVal)
			{
				throw new VistaToolsException("Error opening process token");
			}
			try
			{

				TOKEN_ELEVATION_TYPE tet = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

				UInt32 dwReturnLength = 0;
				UInt32 tetSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf((int)tet);
				IntPtr tetPtr = Marshal.AllocHGlobal((int)tetSize);
				try
				{

					bRetVal = GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenElevationType, tetPtr, tetSize, out dwReturnLength);

					if ((!bRetVal) | (tetSize != dwReturnLength))
					{
						throw new VistaToolsException("Error getting token information");
					}

					tet = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(tetPtr);
				}
				finally
				{
					Marshal.FreeHGlobal(tetPtr);
				}

				return tet;

			}
			finally
			{
				CloseHandle(hToken);
			}

		}
	}


	public struct TOKEN_ELEVATION
	{
		public UInt32 TokenIsElevated;
	}

	public enum TOKEN_ELEVATION_TYPE
	{
		TokenElevationTypeDefault = 1,
		TokenElevationTypeFull = 2,
		TokenElevationTypeLimited = 3
	}

	public enum TOKEN_INFORMATION_CLASS
	{
		TokenUser = 1,
		TokenGroups = 2,
		TokenPrivileges = 3,
		TokenOwner = 4,
		TokenPrimaryGroup = 5,
		TokenDefaultDacl = 6,
		TokenSource = 7,
		TokenType = 8,
		TokenImpersonationLevel = 9,
		TokenStatistics = 10,
		TokenRestrictedSids = 11,
		TokenSessionId = 12,
		TokenGroupsAndPrivileges = 13,
		TokenSessionReference = 14,
		TokenSandBoxInert = 15,
		TokenAuditPolicy = 16,
		TokenOrigin = 17,
		TokenElevationType = 18,
		TokenLinkedToken = 19,
		TokenElevation = 20,
		TokenHasRestrictions = 21,
		TokenAccessInformation = 22,
		TokenVirtualizationAllowed = 23,
		TokenVirtualizationEnabled = 24,
		TokenIntegrityLevel = 25,
		TokenUIAccess = 26,
		TokenMandatoryPolicy = 27,
		TokenLogonSid = 28,
		MaxTokenInfoClass = 29  // MaxTokenInfoClass should always be the last enum
	}

	//
	// Token Specific Access Rights.
	//



	//Exception class for HtmlEditor
	public class VistaToolsException : ApplicationException
	{
		// Default constructor
		public VistaToolsException()
		{
		}

		// Constructor accepting a single string message
		public VistaToolsException(string message)
			: base(message)
		{
		}

		// Constructor accepting a string message and an 
		// inner exception which will be wrapped by this 
		// custom exception class
		public VistaToolsException(string message,
								   Exception inner)
			: base(message, inner)
		{
		}
	}
}
