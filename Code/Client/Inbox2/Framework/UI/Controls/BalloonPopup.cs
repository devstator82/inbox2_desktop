using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Interop;

namespace Inbox2.Framework.UI.Controls
{
	public delegate bool AllowBalloonCallback();

	/// <summary>
	/// Mostly based on topmost code from here: http://chriscavanagh.wordpress.com/2008/08/13/non-topmost-wpf-popup/
	/// with some additions of my own.
	/// </summary>
	public class BalloonPopup : Popup
	{
		public static readonly DependencyProperty IsTopmostProperty = DependencyProperty.Register("IsTopmost", typeof(bool), typeof(BalloonPopup), new FrameworkPropertyMetadata(false, OnIsTopmostChanged));

		public event AllowBalloonCallback AllowBalloonCallback;		

		private bool? _appliedTopMost;
		private bool _alreadyLoaded;
		private Window _parentWindow;
		
		private DateTime? nextOpen;
		private Timer timer;
		private bool fadingOut;

		public bool IsTopmost
		{
			get { return (bool)GetValue(IsTopmostProperty); }
			set { SetValue(IsTopmostProperty, value); }
		}

		public BalloonPopup()
		{
			this.Loaded += PopupNonTopmost_Loaded;
			this.Unloaded += BetterPopup_Unloaded;
		}

		public void TryOpen()
		{
			if (!fadingOut)
				IsOpen = true;
		}

		/// <summary>
		/// Try to show the popup.
		/// </summary>
		/// <param name="timeout">Automatically close the popup after the given duration in milliseconds.</param>
		/// <param name="staysclosedfor">After popup is closed it is not allowed to be reopened for the given duration in milliseconds.</param>
		public void TryOpen(long timeout, int staysclosedfor)
		{
			if (!fadingOut)
			{
				if (nextOpen.HasValue && DateTime.Now < nextOpen)
					return;

				IsOpen = true;

				FadeOutIn(timeout, staysclosedfor);			    
			}
		}

		public void TryClose()
		{
			if (timer != null)
				timer.Dispose();

			fadingOut = false;
			IsOpen = false;

            EventBroker.Publish(AppEvents.RequestFocus);
		}

		public void FadeOutIn(long timeout, int staysclosedfor)
		{
			fadingOut = true;			

			timer = new Timer(delegate
			{				
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
	             	{
						nextOpen = DateTime.Now.AddMilliseconds(staysclosedfor);
						TryClose();
	             	});

			}, null, timeout, Timeout.Infinite);
		}

		protected override void OnInitialized(EventArgs e)
		{
			if (Child is IControllablePopup)
			{
				IControllablePopup child = (IControllablePopup) Child;

				child.RequestClosePopup += delegate { TryClose(); };
			}
			else
			{
				Button button = LogicalTreeWalker.FindName<Button>(this, "PART_CloseButton");

				if (button != null)
					button.Click += delegate { TryClose(); };				
			}

			base.OnInitialized(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				TryClose();

			base.OnKeyDown(e);
		}

		void PopupNonTopmost_Loaded(object sender, RoutedEventArgs e)
		{
			if (!_alreadyLoaded)
			{
				_alreadyLoaded = true;

				if (this.Child != null)
				{
					this.Child.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(Child_PreviewMouseLeftButtonDown), true);
				}

				_parentWindow = Window.GetWindow(this);

				if (_parentWindow != null)
				{
					_parentWindow.Activated += ParentWindow_Activated;
					_parentWindow.Deactivated += ParentWindow_Deactivated;
				}
			}
		}

		void BetterPopup_Unloaded(object sender, RoutedEventArgs e)
		{			

			if (_parentWindow != null)
			{
				_parentWindow.Activated -= ParentWindow_Activated;
				_parentWindow.Deactivated -= ParentWindow_Deactivated;
			}
		}

		void ParentWindow_Activated(object sender, EventArgs e)
		{
			SetTopmostState(true);
		}

		void ParentWindow_Deactivated(object sender, EventArgs e)
		{
			if (IsTopmost == false)
			{
				SetTopmostState(IsTopmost);
			}
		}

		void Child_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			SetTopmostState(true);

			if (!_parentWindow.IsActive && IsTopmost == false)
			{
				_parentWindow.Activate();
			}
		}

		static void OnIsTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var thisobj = (BalloonPopup)obj;

			thisobj.SetTopmostState(thisobj.IsTopmost);
		}

		protected override void OnOpened(EventArgs e)
		{
			if (AllowBalloonCallback != null)
			{
				if (!AllowBalloonCallback())
				{
					TryClose();

					return;
				}
			}			

			SetTopmostState(IsTopmost);

			var fadeOutIn = PopupManager.GetFadeOutIn(this);

			if (fadeOutIn > 0)
				FadeOutIn(fadeOutIn, 1);

			PopupManager.ActivePopup = this;

			base.OnOpened(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			PopupManager.ActivePopup = null;

			base.OnClosed(e);
		}

		void SetTopmostState(bool isTop)
		{
			// Don’t apply state if it’s the same as incoming state
			if (_appliedTopMost.HasValue && _appliedTopMost == isTop)
				return;

			if (this.Child != null)
			{
				var hwndSource = (PresentationSource.FromVisual(this.Child)) as HwndSource;

				if (hwndSource != null)
				{
					var hwnd = hwndSource.Handle;

					RECT rect;

					if (WinApi.GetWindowRect(hwnd, out rect))
					{
						if (isTop)
						{
							WinApi.SetWindowPos(hwnd, WinApi.HWND_TOPMOST, rect.left, rect.top, (int)this.Width, (int)this.Height, WinApi.TOPMOST_FLAGS_NO_FOCUS);
						}
						else
						{
							// Z-Order would only get refreshed/reflected if clicking the
							// the titlebar (as opposed to other parts of the external
							// window) unless I first set the popup to HWND_BOTTOM
							// then HWND_TOP before HWND_NOTOPMOST
							WinApi.SetWindowPos(hwnd, WinApi.HWND_BOTTOM, rect.left, rect.top, (int)this.Width, (int)this.Height, WinApi.TOPMOST_FLAGS_NO_FOCUS);
							WinApi.SetWindowPos(hwnd, WinApi.HWND_TOP, rect.left, rect.top, (int)this.Width, (int)this.Height, WinApi.TOPMOST_FLAGS_NO_FOCUS);
							WinApi.SetWindowPos(hwnd, WinApi.HWND_NOTOPMOST, rect.left, rect.top, (int)this.Width, (int)this.Height, WinApi.TOPMOST_FLAGS_NO_FOCUS);
						}

						_appliedTopMost = isTop;
					}
				}
			}
		}
	}
}
