using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using TabItem = Inbox2.Framework.UI.Controls.TabItem;

namespace Inbox2.Framework.Plugins.SharedControls.Profiles
{
	/// <summary>
	/// Interaction logic for UserProfileControl.xaml
	/// </summary>
	public partial class UserProfileControl : UserControl
	{
		public static readonly DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(UserProfileControl), new UIPropertyMetadata(false));
		public static readonly DependencyProperty PersonProperty = DependencyProperty.Register("Person", typeof(Person), typeof(UserProfileControl), new UIPropertyMetadata(null, UserProfileControl_PersonChanged));
		public static readonly DependencyProperty SourceAddressProperty = DependencyProperty.Register("SourceAddress", typeof(SourceAddress), typeof(UserProfileControl), new UIPropertyMetadata(null, UserProfileControl_SourceAddresChanged));

        public static readonly BitmapSource _Fallback;

        static UserProfileControl()
		{
            _Fallback = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/inbox-icon.png"));
			_Fallback.Freeze();
		}

		static void UserProfileControl_SourceAddresChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			// Find and attach person
			var address = (SourceAddress) args.NewValue;

			if (address != null)
			{
				var person = VirtualMailBox.VirtualMailBox.Current.FindPerson(address);

				if (person == null)
				{
					// Person not found, create a fake one
					person = new Person(address);
					person.Profiles.Add(new Profile { SourceAddress = address, ScreenName = address.DisplayName, Address = address.Address });
				}

				// Attach person to control
				((UserProfileControl)d).Person = person;
			}
		}

		static void UserProfileControl_PersonChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			// Create tabs
			if ((args.NewValue as Person) != null)
				((UserProfileControl)d).CreateProfileTabs();
		}

		public bool ShowHeader
		{
			get { return (bool)GetValue(ShowHeaderProperty); }
			set { SetValue(ShowHeaderProperty, value); }
		}

		public Person Person
		{
			get { return (Person)GetValue(PersonProperty); }
			set { SetValue(PersonProperty, value); }
		}

		public SourceAddress SourceAddress
		{
			get { return (SourceAddress)GetValue(SourceAddressProperty); }
			set { SetValue(SourceAddressProperty, value); }
		}

		public UserProfileControl()
		{
			InitializeComponent();

			DataContext = this;
		}

		void CreateProfileTabs()
		{
			ProfileTab.Items.Clear();

			var profiles = new List<Profile>();

			foreach (var profile in Person.Profiles)
			{
				var channel = profile.SourceChannel;

				if (channel != null && channel.Charasteristics.SupportsProfiles)
				{
					// get channel icon
					var assembly = channel.GetType().Assembly;
					var resourceNameFormat = assembly.GetName().Name + ".Resources.icon-13.png";
					BitmapSource image;

					using (var stream = assembly.GetManifestResourceStream(resourceNameFormat))
						image = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0];

					// Create profile control for hard profiles
					var control = new ChannelProfileControl(Person, profile, channel);
                    var tabItem = new TabItem
                      	{
                      		Content = control, 
							Icon = image, 
							AllowDelete = false, 
							Style = (Style)FindResource("PersonsSocialTabItem"),
							Header = ColorConverter.ConvertFromString(ChannelsManager.GetChannelColor(channel)), 										
                      	};

					ProfileTab.Items.Add(tabItem);
				}
				else
				{
					profiles.Add(profile);
				}
			}

			// If we have soft profiles for this person show them
			if (profiles.Count > 0)
			{
				var control = new SoftProfilesControl(Person);

				ProfileTab.Items.Add(new TabItem
                 	{
                 		Content = control,
                 		Header = ColorConverter.ConvertFromString("#757677"),
                 		Icon = _Fallback,
                 		AllowDelete = false,
                 		Style = (Style) FindResource("PersonsOtherTabItem")
                 	});

				control.Profiles.AddRange(profiles);
			}
		}

		void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Compose message from user profile");

			EventBroker.Publish(AppEvents.New, (SourceAddress)e.Parameter);
		}

		void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
	}
}
