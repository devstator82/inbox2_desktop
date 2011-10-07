using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Plugins.SharedControls.Profiles
{
	/// <summary>
	/// Interaction logic for SoftProfilesControl.xaml
	/// </summary>
	public partial class SoftProfilesControl : UserControl
	{
		public Person Person { get; private set; }

		public AdvancedObservableCollection<Profile> Profiles { get; private set; }

		public Color BackgroundColor
		{
            get { return (Color)ColorConverter.ConvertFromString("#757677"); }
		}

		public SoftProfilesControl(Person person)
		{
			Person = person;
			Profiles = new AdvancedObservableCollection<Profile>();

			InitializeComponent();

			DataContext = this;
		}

		void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.New, (SourceAddress)e.Parameter);
		}

		void ViewAllButton_Click(object sender, RoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.View, Person);
		}
	}
}


