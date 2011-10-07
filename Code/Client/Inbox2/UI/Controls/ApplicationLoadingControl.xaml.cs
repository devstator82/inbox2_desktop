using System;
using System.Windows.Controls;
using Inbox2.Core.Configuration;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for ApplicationLoadingControl.xaml
	/// </summary>
	public partial class ApplicationLoadingControl : UserControl
	{
		public string Fullname
		{
			get { return SettingsManager.ClientSettings.AppConfiguration.Fullname; }
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLoadingControl"/> class.
        /// </summary>
		public ApplicationLoadingControl()
		{
			InitializeComponent();

			DataContext = this;
		}
	}
}
