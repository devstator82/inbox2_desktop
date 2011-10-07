using System;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Core.Configuration;

namespace Inbox2.UI.Controls.Options
{
	/// <summary>
	/// Interaction logic for AdvancedOptionsControl.xaml
	/// </summary>
	public partial class AdvancedOptionsControl : UserControl
	{
		public AdvancedOptionsControl()
		{
			InitializeComponent();
		}

		public bool TrySave()
		{
			SettingsManager.ClientSettings.AppConfiguration.IgnoreSslCertificateIssues = IgnoreSslErrorsCheckBox.IsChecked ?? false;

			return true;
		}

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			IgnoreSslErrorsCheckBox.IsChecked = SettingsManager.ClientSettings.AppConfiguration.IgnoreSslCertificateIssues;	
		}
	}
}
