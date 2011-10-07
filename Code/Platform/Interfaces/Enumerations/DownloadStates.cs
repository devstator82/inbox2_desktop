using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces.Enumerations
{
	public static class DownloadStates
	{
		public const int None = 0;
		public const int Initial = 10;
		public const int Downloading = 20;
		public const int Downloaded = 30;
		public const int DoNotDownload = 100;
	}
}