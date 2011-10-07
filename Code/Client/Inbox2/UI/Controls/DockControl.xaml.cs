using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Framework.Collections;
using Inbox2.UI.Controls.Options;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for DockControl.xaml
	/// </summary>
	public partial class DockControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public AdvancedObservableCollection<ChannelInstance> Channels
		{
			get { return ChannelsManager.Channels; }
		}
		
		public bool HasNetworkConnection
		{
			get { return NetworkInterface.GetIsNetworkAvailable(); }
		}

		public DockControl()
		{
			InitializeComponent();

			DataContext = this;

			NetworkChange.NetworkAvailabilityChanged += delegate { OnPropertyChanged("HasNetworkConnection"); };
		}     

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void DockControl_OnLoaded(object sender, RoutedEventArgs e)
		{
			// Build menu's from plugins
			var items =
				PluginsManager.Current.Plugins.Where(p => p.NewItemView != null);

			foreach (var item in items)
			{
				var mi = new MenuItem
	         	{
	         		Header = item.NewItemView.Header,
	         		Icon = new Image { Source = item.NewItemView.Icon, Width = 16, Height = 16 }
	         	};

				var item1 = item;
				
				mi.Click += delegate { item1.State.New(); };
			}
		}
	
        void FilterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Toggle channel (dock)");

			var instance = (ChannelInstance)e.Parameter;

            instance.IsVisible = !instance.IsVisible;

        	new BackgroundActionTask(
        		() =>
        		ClientState.Current.DataService.ExecuteNonQuery(
        			String.Format("update ChannelConfigs set IsVisible='{0}' where ChannelConfigId={1}", 
						instance.IsVisible,
						instance.Configuration.ChannelId)
				)).ExecuteAsync();			
		}
		
        void AccountsSettings_Click(object sender, RoutedEventArgs e)
        {
			ClientStats.LogEvent("Settings (dock)");

			ClientState.Current.ViewController.ShowPopup<OptionsControl>();
        }

		void Help_Click(object sender, RoutedEventArgs e)
		{
			ClientStats.LogEvent("Help (dock)");

			new Process { StartInfo = new ProcessStartInfo("http://help.inbox2.com/") }.Start();
		}	    
	}
}
