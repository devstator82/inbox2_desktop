using System;
using System.ComponentModel;
using System.Windows;

namespace Inbox2.Core.Configuration
{
	/// <summary>
	/// Persists a Window's Size, Location and WindowState to UserScopeSettings 
	/// </summary>
	public class WindowSettings
	{
		/// <summary>
		/// Register the "Save" attached property and the "OnSaveInvalidated" callback 
		/// </summary>
		public static readonly DependencyProperty SaveProperty
		   = DependencyProperty.RegisterAttached("Save", typeof(bool), typeof(WindowSettings),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSaveInvalidated)));

		public static void SetSave(DependencyObject dependencyObject, bool enabled)
		{
			dependencyObject.SetValue(SaveProperty, enabled);
		}

		private readonly Window window;

		public WindowSettings(Window window)
		{
			this.window = window;
		}

		/// <summary>
		/// Called when Save is changed on an object.
		/// </summary>
		private static void OnSaveInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Window window = dependencyObject as Window;
			if (window != null)
			{
				if ((bool)e.NewValue)
				{
					WindowSettings settings = new WindowSettings(window);
					settings.Attach();
				}
			}
		}

		/// <summary>
		/// Load the Window Size Location and State from the settings object
		/// </summary>
		protected virtual void LoadWindowState()
		{
			if (this.Settings.Location != Rect.Empty)
			{
				this.window.Left = this.Settings.Location.Left;
				this.window.Top = this.Settings.Location.Top;
				this.window.Width = this.Settings.Location.Width;
				this.window.Height = this.Settings.Location.Height;
			}
			else
			{
				this.window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}

			this.window.WindowState = this.Settings.WindowState;
		}


		/// <summary>
		/// Save the Window Size, Location and State to the settings object
		/// </summary>
		protected virtual void SaveWindowState()
		{
			this.Settings.WindowState = this.window.WindowState;
			this.Settings.Location = this.window.RestoreBounds;
			this.Settings.Save();
		}

		private void Attach()
		{
			if (this.window != null)
			{
				this.window.Closing += new CancelEventHandler(window_Closing);
				this.window.Initialized += new EventHandler(window_Initialized);
			}
		}

		private void window_Initialized(object sender, EventArgs e)
		{
			LoadWindowState();
		}

		private void window_Closing(object sender, CancelEventArgs e)
		{
			SaveWindowState();
		}

		[Browsable(false)]
		public ClientSettings Settings
		{
			get
			{
				return SettingsManager.ClientSettings;
			}
		}
	}

}
