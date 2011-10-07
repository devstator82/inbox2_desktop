using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Interop;
using Inbox2.Framework.UI;
using Inbox2.Platform.Framework;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for ApplicationHostControl.xaml
	/// </summary>
	public partial class ApplicationHostControl : UserControl
	{
		protected ViewController ViewController
		{
			get { return ((ViewController)ClientState.Current.ViewController); }
		}

		public ApplicationHostControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		public void Initialize(Window window)
		{
			WindowSettings.SetSave(window, true);

			if (WindowChrome._IsCompositionEnabled)
				WindowChrome.SetWindowChrome(window, new WindowChrome
                 	{
                 		RoundCorners = true, 
						RoundCornersRadius = 8,
						ClientBorderThickness = new Thickness(10, 100, 10, 30)
                 	});	
		}

		public void Shutdown()
		{
			ViewController.Shutdown();
		}		

		void UserControl_OnLoaded(object sender, RoutedEventArgs e)
		{
			ViewController.Initialize(Root);

			using (new CodeTimer("ViewController/Startup"))
				ViewController.Startup();

			Desktop.RefreshHandles();
		}			
	}
}
