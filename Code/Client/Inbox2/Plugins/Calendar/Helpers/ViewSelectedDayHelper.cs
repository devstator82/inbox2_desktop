using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Inbox2.Plugins.Calendar.Helpers
{
    public static class ViewSelectedDayHelper
    {
        private static List<ToggleButton> activebuttons = new List<ToggleButton>();
        public static List<ToggleButton> ActiveButtons
        {
            get { return activebuttons; }
            set
            {
                // Set the value and raise the active day changed event
                activebuttons = value;
                //if (ActiveButtonChanged != null) ActiveButtonChanged(null, EventArgs.Empty);
            }
        }

        public static event EventHandler<EventArgs> ActiveButtonsChanged;

        public static readonly DependencyProperty AttachedButtonProperty =
            DependencyProperty.RegisterAttached("AttachedButton", typeof(ToggleButton), typeof(ViewSelectedDayHelper),
                                                new UIPropertyMetadata(null, ViewSelectedDayHelper_AttachedButtonChanged));
        public static ToggleButton GetAttachedPopup(DependencyObject obj)
        {
            return (ToggleButton)obj.GetValue(AttachedButtonProperty);
        }
        public static void SetAttachedButton(DependencyObject obj, ToggleButton button)
        {
            obj.SetValue(AttachedButtonProperty, button);
        }

        static void ViewSelectedDayHelper_AttachedButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            // Determine the what type the object is
            if (d is ToggleButton) AttachButton((ToggleButton)d);
        }

        #region AttachedDayChanged methods

        static void AttachButton(ToggleButton trigger)
        {
            // When the ToggleButton is checked, execute the folowing code
            trigger.Checked +=
                delegate
                    {
                        // Uncheck the previous button if there is one
                        //if (ActiveButtons != null) ActiveButtons.IsChecked = false;

                        // This button is now the active button
                        //ActiveButton = trigger;
                        ActiveButtons.Add(trigger);

                        // The list is changed, so envoke the ActiveButtonsChanged event
                        ActiveButtonsChanged(null, EventArgs.Empty);
                    };

            // When the ToggleButton is unchecked, execute the folowing code
            trigger.Unchecked +=
                delegate
                    {
                        // There is no active button annymore
                        //ActiveButtons = null;
                        ActiveButtons.Remove(trigger);

                        // The list is changed, so envoke the ActiveButtonsChanged event
                        ActiveButtonsChanged(null, EventArgs.Empty);
                    };
        }

        #endregion
    }
}
