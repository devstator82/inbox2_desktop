using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Stats;
using Inbox2.Framework.Threading;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Plugins.Contacts.Controls
{
	/// <summary>
	/// Interaction logic for SearchProfilesControl.xaml
	/// </summary>
	public partial class SearchProfilesControl : UserControl, INotifyPropertyChanged
	{
		#region Dependency properties

		public static readonly DependencyProperty PersonProperty = DependencyProperty.Register("Person", typeof(Person), typeof(SearchProfilesControl), new UIPropertyMetadata(null, UserProfileControl_PersonChanged));

		private static void UserProfileControl_PersonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var person = e.NewValue as Person;
			var control = ((SearchProfilesControl) d);

			if (person != null)
			{
				control.Profiles.Clear();
				control.SearchTextBox.Text = person.Name;
			}
		}

		#endregion

		private readonly VirtualMailBox mailbox;
		private string currentSearchTerm;

		public static RoutedUICommand AddProfile = new RoutedUICommand(); 
		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> PersonRedirected;
		public event EventHandler<EventArgs> ProfileMerged;

		public Person Person
		{
			get { return (Person)GetValue(PersonProperty); }
			set { SetValue(PersonProperty, value); }
		}

		public AdvancedObservableCollection<Profile> Profiles { get; private set; }

		public SearchProfilesControl()
		{
			mailbox = VirtualMailBox.Current;

			InitializeComponent();

			DataContext = this;

			Profiles = new AdvancedObservableCollection<Profile>();
		}

		void PerformSearch()
		{
			loader.Visibility = Visibility.Visible;

			var keywords = currentSearchTerm
				.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
				.Where(s => s != null)
				.ToArray();

			new BackgroundActionTask(delegate
			{
				var profiles = new List<Profile>();

				foreach (var keyword in keywords)
				{
					if (keyword.Length > 2)
					{
						using (mailbox.Profiles.ReaderLock)
						{
							string keyword1 = keyword;

							profiles.AddRange(mailbox.Profiles.Where(p =>
								(p.ScreenName != null && p.ScreenName.IndexOf(keyword1, StringComparison.InvariantCultureIgnoreCase) > -1) ||
								(p.Address != null && p.Address.IndexOf(keyword1, StringComparison.InvariantCultureIgnoreCase) > -1) ||
								(p.CompanyName != null && p.CompanyName.IndexOf(keyword1, StringComparison.InvariantCultureIgnoreCase) > -1)));
						}
					}
				}

				Thread.CurrentThread.ExecuteOnUIThread(delegate
				{
					loader.Visibility = Visibility.Hidden;

					// remove any profiles that belong to the person we are currently viewing
					Profiles.Replace(profiles.Where(p => p.Person != Person).Distinct());
				});

			}).ExecuteAsync();
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (String.IsNullOrEmpty(SearchTextBox.Text.Trim()))
			{
				Profiles.Clear();
			}

			if (SearchTextBox.Text.Length > 2)
			{
				currentSearchTerm = SearchTextBox.Text.Trim();

				PerformSearch();
			}
		}

		void AddProfile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		void AddProfile_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Join profile");

			var profile = (Profile) e.Parameter;

			profile.JoinWith(Person);

			if (PersonRedirected != null)
				PersonRedirected(this, EventArgs.Empty);

			Person.RebuildScore();

			PerformSearch();

			if (ProfileMerged != null)
				ProfileMerged(this, EventArgs.Empty);
		}
	}
}