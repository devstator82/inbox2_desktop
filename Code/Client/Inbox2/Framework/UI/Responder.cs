using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Inbox2.Framework.UI
{
    public static class Responder
    {
        public static readonly DependencyProperty IsFirstResponderProperty =
            DependencyProperty.RegisterAttached("IsFirstResponder", typeof(bool), typeof(DependencyObject), new UIPropertyMetadata(false));

        public static readonly DependencyProperty IsSearchResponderProperty =
            DependencyProperty.RegisterAttached("IsSearchResponder", typeof(bool), typeof(DependencyObject), new UIPropertyMetadata(false));

        public static bool GetIsFirstResponder(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFirstResponderProperty);
        }

        public static void SetIsFirstResponder(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFirstResponderProperty, value);
        }

        public static bool GetIsSearchResponder(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSearchResponderProperty);
        }

        public static void SetIsSearchResponder(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSearchResponderProperty, value);
        }
    }
}
