using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Inbox2.Framework.Interfaces.Enumerations;

namespace Inbox2.Framework.UI.Controls
{
	[TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonBase))]
	public class TabItem : System.Windows.Controls.TabItem
	{
		static TabItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
		}

		/// <summary>
		/// Provides a place to display an Icon on the TabItem and on the DropDown Context Menu
		/// </summary>
		public ImageSource Icon
		{
			get { return (ImageSource)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TabItem), new UIPropertyMetadata(null));
		public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register("AllowDelete", typeof(bool), typeof(TabItem), new UIPropertyMetadata(true));
		public static readonly DependencyProperty ShowHeaderProperty = DependencyProperty.Register("ShowHeader", typeof(bool), typeof(TabItem), new UIPropertyMetadata(true));
		
		/// <summary>
		/// Allow the TabItem to be Deleted by the end user
		/// </summary>
		public bool AllowDelete
		{
			get { return (bool)GetValue(AllowDeleteProperty); }
			set { SetValue(AllowDeleteProperty, value); }
		}

		/// <summary>
		/// Shows or hides the header
		/// </summary>
		public bool ShowHeader
		{
			get { return (bool)GetValue(ShowHeaderProperty); }
			set { SetValue(ShowHeaderProperty, value); }
		}

		public WellKnownView WellKnownView { get; set; }

		/// <summary>
		/// OnApplyTemplate override
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// wire up the CloseButton's Click event if the button exists
			ButtonBase button = this.Template.FindName("PART_CloseButton", this) as ButtonBase;
			if (button != null)
			{
				button.Click += delegate
				{
					// get the parent tabcontrol 
					var tc = this.FindAncestor<TabControl>();
					if (tc == null) return;

					// remove this tabitem from the parent tabcontrol
					tc.RemoveItem(this);
				};
			}
		}

		/// <summary>
		/// So this method has been overriden and re-implemented because the default behavior was causing 
		/// the WebBrowser to receive focus regardless of our own Responder chain. This override seems to
		/// solve that problem.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			TabItem tabItem;

			if (e.Source is TabItem)
				tabItem = e.Source as TabItem;
			else
			{
				// Try to find parent TabItem
				tabItem = (e.Source as DependencyObject).FindAncestor<TabItem>();
			}

			if (tabItem != null)
			{
				var tc = this.FindAncestor<TabControl>();

				if (tc != null)
				{
					e.Handled = tc.SelectTabItem(tabItem);
				}
			}
		}
	}
}
