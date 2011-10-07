using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.UI.TaskbarNotification;

namespace Inbox2.UI.Controls
{
    /// <summary>
    /// Interaction logic for NotifyBalloon.xaml
    /// </summary>
    public partial class NotifyBalloon : UserControl, INotifyPropertyChanged
    {
        #region Properties

    	/// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

    	/// <summary>
    	/// Gets or sets the new messages.
    	/// </summary>
    	/// <value>The new messages.</value>
    	public AdvancedObservableCollection<Message> NewMessages { get; private set; }
		public AdvancedObservableCollection<Message> VisibleMessages { get; private set; }

    	/// <summary>
    	/// Gets or sets the new messages.
    	/// </summary>
    	/// <value>The new messages.</value>
    	public AdvancedObservableCollection<UserStatus> NewStatusUpdates { get; private set; }
		public AdvancedObservableCollection<UserStatus> VisibleStatusUpdates { get; private set; }

    	/// <summary>
    	/// Gets or sets the new social searches.
    	/// </summary>
    	/// <value>The new social searches.</value>
    	public AdvancedObservableCollection<UserStatus> NewSocialSearches { get; private set; }
		public AdvancedObservableCollection<UserStatus> VisibleSocialSearches { get; private set; }

    	/// <summary>
        /// Gets the view controller.
        /// </summary>
        /// <value>The view controller.</value>
        protected ViewController ViewController
        {
            get { return ((ViewController)ClientState.Current.ViewController); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyBalloon"/> class.
        /// </summary>
        public NotifyBalloon()
        {
			NewMessages = new AdvancedObservableCollection<Message>();
			NewStatusUpdates = new AdvancedObservableCollection<UserStatus>();
			NewSocialSearches = new AdvancedObservableCollection<UserStatus>();
            VisibleMessages = new AdvancedObservableCollection<Message>();
			VisibleStatusUpdates = new AdvancedObservableCollection<UserStatus>();
			VisibleSocialSearches = new AdvancedObservableCollection<UserStatus>();

            InitializeComponent();

            DataContext = this;

            //TODO:
            //TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
        }

        #endregion

		#region Methods

		public void UpdateCounts(IEnumerable<Message> messages, IEnumerable<UserStatus> statusUpdates, IEnumerable<UserStatus> searchUpdates)
		{
			NewMessages.Replace(messages);
			VisibleMessages.Replace(NewMessages.Take(3));

			NewStatusUpdates.Replace(statusUpdates);
			VisibleStatusUpdates.Replace(NewStatusUpdates.Take(3));

			NewSocialSearches.Replace(searchUpdates);
			VisibleSocialSearches.Replace(NewSocialSearches.Take(3));
		}

		#endregion

		#region Event handlers

		void notifyBalloonGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
			var window = Application.Current.MainWindow;

			if (window.WindowState == WindowState.Minimized)
				window.WindowState = WindowState.Normal;

			window.Show();
			window.Activate();

			ClientState.Current.ViewController.MoveTo(WellKnownView.Overview);
        }

        void notifyBalloonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            //// If we're already running the fade-out animation, do not interrupt anymore
            //// (makes things too complicated for the sample)
            //if (isClosing)
            //{
            //    return;
            //}

            //ViewController.TaskBarNotifyManager.ResetBalloonCloseTimer();
        }

        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the fade-out animation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

			NewMessages.Clear();
			NewStatusUpdates.Clear();
			NewSocialSearches.Clear();
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewController.TaskBarNotifyManager.TaskbarIcon.CloseBalloon();
        }

        #endregion
    }
}
