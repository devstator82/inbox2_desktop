using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;
using Inbox2.Framework.UI.Controls;

namespace Inbox2.Framework.UI
{
	public static class PopupManager
	{
		private static BalloonPopup activePopup;
		public static event EventHandler<EventArgs> ActivePopupChanged;
		public static readonly DependencyProperty AttachedPopupProperty = DependencyProperty.RegisterAttached("AttachedPopup", typeof(BalloonPopup), typeof(PopupManager), new UIPropertyMetadata(null, PopupManager_AttachedPopupChanged));
		public static readonly DependencyProperty FadeOutInProperty = DependencyProperty.RegisterAttached("FadeOutIn", typeof(int), typeof(BalloonPopup), new UIPropertyMetadata(0));		

		public static BalloonPopup ActivePopup
		{
			get { return activePopup; }
			set
			{
				activePopup = value;

				if (ActivePopupChanged != null)
				{
					ActivePopupChanged(null, EventArgs.Empty);
				}
			}
		}        	    
        
		public static BalloonPopup GetAttachedPopup(DependencyObject obj)
        {
            return (BalloonPopup)obj.GetValue(AttachedPopupProperty);
        }

        public static void SetAttachedPopup(DependencyObject obj, BalloonPopup value)
        {
            obj.SetValue(AttachedPopupProperty, value);
        }

		public static int GetFadeOutIn(DependencyObject obj)
		{
			return (int)obj.GetValue(FadeOutInProperty);
		}

		public static void SetFadeOutIn(DependencyObject obj, int value)
		{
			obj.SetValue(FadeOutInProperty, value);
		}

		static void PopupManager_AttachedPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			BalloonPopup popup = (BalloonPopup)args.NewValue;

			if (d is ToggleButton) AttachButton(popup, (ToggleButton)d);
            if (d is Button) AttachButton(popup, (Button)d);
			if (d is Selector) AttachListView(popup, (Selector)d);
        }		

        #region AttachedPopupChanged methods

        static void AttachButton(BalloonPopup popup, ToggleButton trigger)
		{
			Window owner = Window.GetWindow(trigger);

			Binding binding = new Binding("IsChecked");
			binding.Source = trigger;
			popup.SetBinding(Popup.IsOpenProperty, binding);

			if (owner != null)
			{
				owner.LocationChanged += delegate { if (popup.StaysOpen == false) trigger.IsChecked = false; };
				owner.SizeChanged += delegate { if (popup.StaysOpen == false) trigger.IsChecked = false; };
			}
		}

        static void AttachButton(BalloonPopup popup, Button trigger)
        {
            Window owner = Window.GetWindow(trigger);

            trigger.Click += delegate { popup.TryOpen(); };

            if (owner != null)
            {
                owner.LocationChanged += delegate { popup.TryClose(); };
                owner.SizeChanged += delegate { popup.TryClose(); };
            }
        }

		static void AttachListView(BalloonPopup popup, Selector trigger)
		{
			trigger.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
        	{
				if (e.AddedItems.Count == 0) return;
				if (popup.IsOpen) popup.TryClose();

				var source = trigger.SelectedItem;
				var selectedItem = (ListViewItem)trigger.ItemContainerGenerator.ContainerFromItem(source);

				popup.PlacementTarget = selectedItem;
				popup.TryOpen();
        	};
        }

        #endregion
    }
}
