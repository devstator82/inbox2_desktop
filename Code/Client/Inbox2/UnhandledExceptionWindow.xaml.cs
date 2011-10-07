using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Platform.Framework.Web;

namespace Inbox2
{
	/// <summary>
	/// Interaction logic for UnhandledExceptionWindow.xaml
	/// </summary>
	public partial class UnhandledExceptionWindow : Window
	{
		public static RoutedCommand CancelCommand = new RoutedCommand();

		private readonly Exception exception;
		
		static string AssetBaseUrl
		{
			get
			{
				return String.Format("http://download{0}.inbox2.com/",
				   String.IsNullOrEmpty(CommandLine.Current.Environment) ? String.Empty : "." + CommandLine.Current.Environment);
			}
		}

		public string ExceptionMessage
		{
			get { return exception.Message; }
		}

		public UnhandledExceptionWindow(Exception exception)
		{
			InitializeComponent();

			this.exception = exception;
			this.DataContext = this;
		}

		void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		internal static void LogError(Exception ex)
		{
			//Check if it is 32bit or a 64bit system
			int environmentOsBit = System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr));
			string osBit = environmentOsBit == 8 ? "64 bit" : "32 bit";

			int deskHeight = Screen.PrimaryScreen.Bounds.Height;
			int deskWidth = Screen.PrimaryScreen.Bounds.Width;
			string osResolution = deskWidth.ToString() + 'x' + deskHeight;

			var post = new StringBuilder();
			post.AppendFormat("clientId={0}&", SettingsManager.ClientSettings.AppConfiguration.ClientId);
			post.AppendFormat("clientVersion={0}&", typeof(UnhandledExceptionWindow).Assembly.GetName().Version);
			post.AppendFormat("osVersion={0}&", Inbox2Environment.GetOperatingSystem());
			post.AppendFormat("osServicePack={0}&", Environment.OSVersion.ServicePack);
			post.AppendFormat("screenResolution={0}&", osResolution);
			post.AppendFormat("cpuArchitecture={0}&", osBit);
			post.AppendFormat("cpuCount={0}&", Environment.ProcessorCount);
			post.AppendFormat("errorString={0}", ex);

			HttpServiceRequest.Post(AssetBaseUrl + "error/submit", post.ToString(), false);
		}			
	}
}