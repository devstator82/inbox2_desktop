using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Inbox2.Framework.UI.Controls
{
    public class CustomBorder : Border
    {
        public static readonly DependencyProperty InnerStyleProperty = DependencyProperty.Register("InnerStyle", typeof(Style), typeof(CustomBorder), new UIPropertyMetadata(null));
        public static readonly DependencyProperty MiddleStyleProperty = DependencyProperty.Register("MiddleStyle", typeof(Style), typeof(CustomBorder), new UIPropertyMetadata(null));
        public static readonly DependencyProperty OuterStyleProperty = DependencyProperty.Register("OuterStyle", typeof(Style), typeof(CustomBorder), new UIPropertyMetadata(null));

        public Style InnerStyle
        {
            get { return (Style)GetValue(InnerStyleProperty); }
            set { SetValue(InnerStyleProperty, value); }
        }

        public Style MiddleStyle
        {
            get { return (Style)GetValue(MiddleStyleProperty); }
            set { SetValue(MiddleStyleProperty, value); }
        }

        public Style OuterStyle
        {
            get { return (Style)GetValue(OuterStyleProperty); }
            set { SetValue(OuterStyleProperty, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            var oldChild = Child;

            // First disconnect the old child
            Child = null;

            var innerBorder = new Border { Style = InnerStyle, Child = oldChild };
            var middleborder = new Border { Style = MiddleStyle, Child = innerBorder };
            var outerBorder = new Border { Style = OuterStyle, Child = middleborder };

            base.Child = outerBorder;

            base.OnInitialized(e);
        }
    }
}
