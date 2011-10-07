using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Inbox2.Framework.UI.Controls
{
    public class Accordion : StackPanel
    {
        static Accordion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Accordion),
                new FrameworkPropertyMetadata(typeof(Accordion)));			
        }

    	public Accordion()
    	{
			Loaded += Accordion_Loaded;
    	}

		void Accordion_Loaded(object sender, RoutedEventArgs e)
		{
			DataContextChanged += OnDataContextChanged;
		}

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
			Expander selectedExpander;
			foreach (UIElement element in Children)
			{
				selectedExpander = element as Expander;
				if (selectedExpander != null)
				{
					selectedExpander.Expanded += new RoutedEventHandler(selectedExpander_Expanded);
				}
			}
        }

        void selectedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander selectedExpander = sender as Expander;
            Expander otherExpander = null;
            ContentPresenter contentPresenter = null;
            double totalExpanderHeight = 0;

            if (selectedExpander != null)
            {
                foreach (UIElement element in this.Children)
                {
                    otherExpander = element as Expander;
                    if (otherExpander != null & otherExpander != selectedExpander)
                    {
                        if (otherExpander.IsExpanded)
                        {
                            contentPresenter = otherExpander.Template.FindName("ExpandSite", otherExpander) as ContentPresenter;
                            if (contentPresenter != null)
                                totalExpanderHeight -= contentPresenter.ActualHeight;
                        }
                        otherExpander.IsExpanded = false;
                        totalExpanderHeight += otherExpander.ActualHeight;
                    }
                }

                if (selectedExpander.IsExpanded)
                {
                    contentPresenter = selectedExpander.Template.FindName("ExpandSite", selectedExpander) as ContentPresenter;
                    if (contentPresenter != null)
                        contentPresenter.Height = this.ActualHeight - totalExpanderHeight - selectedExpander.ActualHeight;
                }
            }
        }
    }
}
