using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Framework.Plugins.SharedControls
{
	public class ToggleButtonTemplateSelector : DataTemplateSelector
	{
		public bool IsExpanded { get; set; }
		public DataTemplate FullTemplate { get; set; }
		public DataTemplate SimpleTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			return IsExpanded ? FullTemplate : SimpleTemplate;
		}
	}

	/// <summary>
	/// Interaction logic for StatusUpdateControl.xaml
	/// </summary>
	public partial class StatusUpdateControl : UserControl, IValueConverter, INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> RequestClose;

		public static RoutedCommand UpdateStatus = new RoutedCommand();
		public static RoutedCommand ToggleChannel = new RoutedCommand();

		public List<long> SelectedChannels { get; private set; }
		public UserStatus ReplyTo { get; set; }

		private readonly VirtualMailBox.VirtualMailBox mailbox = VirtualMailBox.VirtualMailBox.Current;
		private bool isExpanded;

		#endregion

		#region Properties

		public IEnumerable<ChannelInstance> Channels
		{
			get { return ChannelsManager.GetStatusChannels(); }
		}

		public bool HasTwitter
		{
			get
			{
				return SelectedChannels
					.Select(c => ChannelsManager.GetChannelObject(c))
					.Where(c => c != null)
					.Any(c => c.Configuration.DisplayName == "Twitter");
			}
		}

		public string Status
		{
			get { return StatusTextBox.Text; }
			set { StatusTextBox.Text = value; }
		}

		public bool IsExpanded
		{
			set
			{
				isExpanded = value;

				((ToggleButtonTemplateSelector)
				 FindResource("ToggleButtonTemplateSelector"))
					.IsExpanded = isExpanded;
			}
			get
			{
				return isExpanded;
			}
		}

		#endregion

		#region Construction

		public StatusUpdateControl()
		{
			SelectedChannels = new List<long>();

			Resources.Add("IsCheckedConverter", this);

			InitializeComponent();

			DataContext = this;			
		}

		#endregion

		#region Methods

		public void SaveChannelSelection()
		{
			// Save current selection
			ClientState.Current.Context.SaveSetting("/Settings/StatusUpdate/ChannelsSelection",
			                                        String.Join(",", SelectedChannels.Select(i => i.ToString()).ToArray()));
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return false;

			var channel = (ChannelInstance)value;

			return SelectedChannels.Contains(channel.Configuration.ChannelId);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}		

		void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		void ToggleChanneIndex(int index)
		{
			var channels = Channels.ToList();

			if (index <= channels.Count)
			{
				var channel = channels[index - 1];

				ToggleChannelInstance(channel);
			}
		}

		void ToggleChannelInstance(ChannelInstance channel)
		{
			if (SelectedChannels.Contains(channel.Configuration.ChannelId))
			{
				SelectedChannels.Remove(channel.Configuration.ChannelId);
			}
			else
			{
				SelectedChannels.Add(channel.Configuration.ChannelId);
			}

			// Forces a propertychanged event, which resets our toggle button
			channel.IsVisible = channel.IsVisible;

			OnPropertyChanged("HasTwitter");
		}

		public static IEnumerable<long> GetSavedSelection()
		{
			var setting = ClientState.Current.Context.GetSetting("/Settings/StatusUpdate/ChannelsSelection");

			if (setting == null || String.IsNullOrEmpty(setting as string))
				yield break;

			foreach (var channel in ((string)setting).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => Int64.Parse(s))
				.Select(i => ChannelsManager.GetChannelObject(i))
				.Where(c => c != null && c.Configuration.Charasteristics.SupportsStatusUpdates))
				yield return channel.Configuration.ChannelId;
		}

		#endregion

		#region Event handlers

		void StatusUpdateControl_OnLoaded(object sender, RoutedEventArgs e)
		{
			StatusTextBox.SelectionStart = StatusTextBox.Text.Length;
			StatusTextBox.Focus();
		}
	
		void StatusTextBox_OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (StatusTextBox.Text.Length > 0)
				{
					StatusTextBox.Text = String.Empty;
				}
				else
				{
					if (RequestClose != null)
						RequestClose(this, EventArgs.Empty);
				}
			}
		}

		void ToggleChannel_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			var channel = (ChannelInstance)e.Parameter;

			ToggleChannelInstance(channel);
		}

		void UpdateStatus_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			var status = new UserStatus
			    {
			        Status = StatusTextBox.Text.Trim(),
			        StatusType = StatusTypes.MyUpdate,
			        SortDate = DateTime.Now,
			        DateCreated = DateTime.Now,
			        TargetChannelId = String.Join(";", SelectedChannels.Select(i => i.ToString()).ToArray())
			    };

			mailbox.StatusUpdates.Add(status);

			ClientState.Current.DataService.Save(status);

			// Save command
			CommandQueue.Enqueue(AppCommands.SendStatusUpdate, status);
			
			if (!NetworkInterface.GetIsNetworkAvailable())
			{
				ClientState.Current.ShowMessage(
					new AppMessage(Strings.StatusWillBeUpdatedLater)
						{
							EntityId = status.StatusId,
							EntityType = EntityType.UserStatus
						}, MessageType.Success);
			}

			StatusTextBox.Text = String.Empty;
		}

		void ShortenUrls_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			StatusTextBox.Text = UrlShortenerService.Shorten(StatusTextBox.Text);
		}		
		
		void ToggleChannel1_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(1);
		}

		void ToggleChannel2_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(2);
		}

		void ToggleChannel3_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(3);
		}

		void ToggleChannel4_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(4);
		}

		void ToggleChannel5_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(5);
		}

		void ToggleChannel6_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(6);
		}

		void ToggleChannel7_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(7);
		}

		void ToggleChannel8_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(8);
		}

		void ToggleChannel9_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ToggleChanneIndex(9);
		}

		#region CanExecute

		void CanAlwaysExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void ToggleChannel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void UpdateStatus_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (StatusTextBox != null)
			{
				e.CanExecute = SelectedChannels.Count > 0 && !String.IsNullOrEmpty(StatusTextBox.Text.Trim());
			}
		}

		void ShortenUrls_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !String.IsNullOrEmpty(StatusTextBox.Text.Trim());
		}

		#endregion

		#endregion

	}
}
