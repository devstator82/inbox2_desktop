using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Text.Html;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Conversations.Controls
{
	/// <summary>
	/// Interaction logic for BrowserThreadView.xaml
	/// </summary>
	public partial class BrowserThreadView : UserControl
	{
		private readonly ChromeWebbrowserSource scroller;
		private readonly DispatcherTimer timer;
		private Message message;

		public BrowserThreadView()
		{
			InitializeComponent();

			scroller = new ChromeWebbrowserSource(HtmlView);
			scroller.ExtentHeightChanged += Scroller_ExtentHeightChanged;
			timer = new DispatcherTimer(DispatcherPriority.Input, Dispatcher);
			timer.Tick += TimerTick;

			DataContext = this;
		}		

		public void Show(Message message)
		{
			this.message = message;

			RockScroll.Value = 0;
			timer.IsEnabled = true;
		}

		void TimerTick(object sender, EventArgs e)
		{
			HtmlView.LoadHtml(GetMessageHtmlView(message));

			timer.IsEnabled = false;
		}

		public void Hide()
		{
			HtmlView.Dispose();
		}

		void Scroller_ExtentHeightChanged(object sender, EventArgs e)
		{
			// Update scrollbar
			RockScroll.Maximum = scroller.ExtentHeight;
		}

		void RockScroll_Scroll(object sender, ScrollEventArgs e)
		{
			double offset = (scroller.ExtentHeight - scroller.ViewportHeight) / RockScroll.Maximum * e.NewValue;

			scroller.ScrollTo(offset);
		}
		
		static string GetMessageHtmlView(Message source)
		{
			// Hack: load core assembly directly (should be in memory anyway)
			Assembly asm = Assembly.LoadFrom("Inbox2.Core.Streams.dll");

			//Load HTML File:
			Stream htmlFile = asm.GetManifestResourceStream("Inbox2.Core.Streams.ThreadView.html");
			string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

			var access = new ClientMessageAccess(source);
			var sanitized = HtmlSanitizer.Sanitize(access.GetBestBodyMatch(TextConversion.ToHtml));

			return htmlFile.ReadString()
				.Replace("#rootfolder#", "file://" + path.Replace("\\", "//") + "/")
				.Replace("#Title#", HttpUtility.HtmlEncode(source.Context))
				.Replace("#HtmlSource#", sanitized);
		}	
	}
}
