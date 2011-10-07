using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Dock=System.Windows.Controls.Dock;
using ScrollViewer=System.Windows.Controls.ScrollViewer;

namespace Inbox2.Framework.UI.Controls
{
	[TemplatePart(Name = "PART_DropDown", Type = typeof(ToggleButton))]
	[TemplatePart(Name = "PART_RepeatLeft", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_RepeatRight", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_NewTabButton", Type = typeof(ButtonBase))]
	[TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
	public class TabControl : System.Windows.Controls.TabControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Using a DependencyProperty as the backing store for Dock.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderContentProperty =
			DependencyProperty.Register("HeaderContentProperty", typeof(DependencyObject), typeof(TabControl), new UIPropertyMetadata(null));

		public DependencyObject HeaderContent
		{
			get { return (DependencyObject)GetValue(HeaderContentProperty); }
			set
			{
				SetValue(HeaderContentProperty, value);				

				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("HeaderContent"));
			}
		}		

		public event EventHandler<TabItemEventArgs> TabItemAdded;
		public event EventHandler<TabItemEventArgs> TabItemSelected;
		public event EventHandler<TabItemCancelEventArgs> TabItemClosing;
		public event EventHandler<TabItemEventArgs> TabItemClosed;		

		// TemplatePart controls
		private ToggleButton _toggleButton;

		static TabControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));

			TabStripPlacementProperty.AddOwner(typeof(TabControl), new FrameworkPropertyMetadata(Dock.Top, new PropertyChangedCallback(OnTabStripPlacementChanged)));
		}

		/// <summary>
		/// OnTabStripPlacementChanged property callback
		/// </summary>
		/// <remarks>
		///     We need to supplement the base implementation with this method as the base method does not work when
		///     we are using virtualization in the tabpanel, it only updates visible items
		/// </remarks>
		private static void OnTabStripPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabControl tc = (TabControl)d;

			foreach (TabItem item in tc.Items)
				item.CoerceValue(TabItem.TabStripPlacementProperty);
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if (TabItemSelected != null && SelectedItem != null)
				TabItemSelected(this, new TabItemEventArgs((TabItem)SelectedItem));
		}

        /// <summary>
        /// Hack-fix for a weird issue with webbrowser causing a tabswitch, we have to
        /// perform the patching up on this level. No idea why. Just glad I found this workaround :-)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (e.KeyboardDevice.Target is WebBrowser)
            {
                e.Handled = true;

                (HeaderContent as FrameworkElement).FocusFirstResponder();
            }
        }

		#region Dependancy properties

		#region Brushes

		public Brush TabItemNormalBackground
		{
			get { return (Brush)GetValue(TabItemNormalBackgroundProperty); }
			set { SetValue(TabItemNormalBackgroundProperty, value); }
		}
		public static readonly DependencyProperty TabItemNormalBackgroundProperty = DependencyProperty.Register("TabItemNormalBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

		public Brush TabItemMouseOverBackground
		{
			get { return (Brush)GetValue(TabItemMouseOverBackgroundProperty); }
			set { SetValue(TabItemMouseOverBackgroundProperty, value); }
		}
		public static readonly DependencyProperty TabItemMouseOverBackgroundProperty = DependencyProperty.Register("TabItemMouseOverBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

		public Brush TabItemSelectedBackground
		{
			get { return (Brush)GetValue(TabItemSelectedBackgroundProperty); }
			set { SetValue(TabItemSelectedBackgroundProperty, value); }
		}
		public static readonly DependencyProperty TabItemSelectedBackgroundProperty = DependencyProperty.Register("TabItemSelectedBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));




		#endregion

		/*
         * Based on the whether the ControlTemplate implements the NewTab button and Close Buttons determines the functionality of the AllowAddNew & AllowDelete properties
         * If they are in the control template, then the visibility of the AddNew & TabItem buttons are bound to these properties
         * 
        */
		/// <summary>
		/// Allow the User to Add New TabItems
		/// </summary>
		public bool AllowAddNew
		{
			get { return (bool)GetValue(AllowAddNewProperty); }
			set { SetValue(AllowAddNewProperty, value); }
		}
		public static readonly DependencyProperty AllowAddNewProperty = DependencyProperty.Register("AllowAddNew", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));

		/// <summary>
		/// Allow the User to Delete TabItems
		/// </summary>
		public bool AllowDelete
		{
			get { return (bool)GetValue(AllowDeleteProperty); }
			set { SetValue(AllowDeleteProperty, value); }
		}
		public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register("AllowDelete", typeof(bool), typeof(TabControl),
			new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAllowDeleteChanged)));

		private static void OnAllowDeleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabControl tc = (TabControl)d;
			foreach (TabItem item in tc.Items)
				item.AllowDelete = (bool)e.NewValue;
		}

		/// <summary>
		/// Set new TabItem as the current selection
		/// </summary>
		public bool SelectNewTabOnCreate
		{
			get { return (bool)GetValue(SelectNewTabOnCreateProperty); }
			set { SetValue(SelectNewTabOnCreateProperty, value); }
		}
		public static readonly DependencyProperty SelectNewTabOnCreateProperty = DependencyProperty.Register("SelectNewTabOnCreate", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));


		/// <summary>
		/// Determines where new TabItems are added to the TabControl
		/// </summary>
		/// <remarks>
		///     Set to true (default) to add all new Tabs to the end of the TabControl
		///     Set to False to insert new tabs after the current selection
		/// </remarks>
		public bool AddNewTabToEnd
		{
			get { return (bool)GetValue(AddNewTabToEndProperty); }
			set { SetValue(AddNewTabToEndProperty, value); }
		}
		public static readonly DependencyProperty AddNewTabToEndProperty = DependencyProperty.Register("AddNewTabToEnd", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));

		/// <summary>
		/// defines the Minimum width of a TabItem
		/// </summary>
		[DefaultValue(80)]
		[Category("Layout")]
		[Description("Gets or Sets the minimum Width Constraint shared by all Items in the Control, individual child elements MinWidth property will overide this property")]
		public double TabItemMinWidth
		{
			get { return (double)GetValue(TabItemMinWidthProperty); }
			set { SetValue(TabItemMinWidthProperty, value); }
		}
		public static readonly DependencyProperty TabItemMinWidthProperty = DependencyProperty.Register("TabItemMinWidth", typeof(double), typeof(TabControl),
			new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMinWidth)));

		private static object CoerceMinWidth(DependencyObject d, object value)
		{
			TabControl tc = (TabControl)d;
			double newValue = (double)value;

			if (newValue > tc.TabItemMaxWidth)
				return tc.TabItemMaxWidth;

			return (newValue > 0 ? newValue : 0);
		}

		/// <summary>
		/// defines the Minimum height of a TabItem
		/// </summary>
		[DefaultValue(20)]
		[Category("Layout")]
		[Description("Gets or Sets the minimum Height Constraint shared by all Items in the Control, individual child elements MinHeight property will override this value")]
		public double TabItemMinHeight
		{
			get { return (double)GetValue(TabItemMinHeightProperty); }
			set { SetValue(TabItemMinHeightProperty, value); }
		}
		public static readonly DependencyProperty TabItemMinHeightProperty = DependencyProperty.Register("TabItemMinHeight", typeof(double), typeof(TabControl),
			new FrameworkPropertyMetadata(30.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMinHeight)));

		private static object CoerceMinHeight(DependencyObject d, object value)
		{
			TabControl tc = (TabControl)d;
			double newValue = (double)value;

			if (newValue > tc.TabItemMaxHeight)
				return tc.TabItemMaxHeight;

			return (newValue > 0 ? newValue : 0);
		}

		/// <summary>
		/// defines the Maximum width of a TabItem
		/// </summary>
		[DefaultValue(0.0)]
		[Category("Layout")]
		[Description("Gets or Sets the maximum width Constraint shared by all Items in the Control, individual child elements MaxWidth property will override this value")]
		public double TabItemMaxWidth
		{
			get { return (double)GetValue(TabItemMaxWidthProperty); }
			set { SetValue(TabItemMaxWidthProperty, value); }
		}
		public static readonly DependencyProperty TabItemMaxWidthProperty = DependencyProperty.Register("TabItemMaxWidth", typeof(double), typeof(TabControl),
			new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMaxWidth)));

		private static object CoerceMaxWidth(DependencyObject d, object value)
		{
			TabControl tc = (TabControl)d;
			double newValue = (double)value;

			if (newValue < tc.TabItemMinWidth)
				return tc.TabItemMinWidth;

			return newValue;
		}

		/// <summary>
		/// defines the Maximum width of a TabItem
		/// </summary>
		[DefaultValue(0.0)]
		[Category("Layout")]
		[Description("Gets or Sets the maximum height Constraint shared by all Items in the Control, individual child elements MaxHeight property will override this value")]
		public double TabItemMaxHeight
		{
			get { return (double)GetValue(TabItemMaxHeightProperty); }
			set { SetValue(TabItemMaxHeightProperty, value); }
		}
		public static readonly DependencyProperty TabItemMaxHeightProperty = DependencyProperty.Register("TabItemMaxHeight", typeof(double), typeof(TabControl),
			new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMaxHeight)));

		private static object CoerceMaxHeight(DependencyObject d, object value)
		{
			TabControl tc = (TabControl)d;
			double newValue = (double)value;

			if (newValue < tc.TabItemMinHeight)
				return tc.TabItemMinHeight;

			return newValue;
		}
		#endregion

		/// <summary>
		/// OnMinMaxChanged callback responds to any of the Min/Max dependancy properties changing
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabControl tc = (TabControl)d;
			if (tc.Template == null) return;

			VirtualizingTabPanel tp = Helper.FindVirtualizingTabPanel(tc);
			if (tp != null)
				tp.InvalidateMeasure();
		}

		/*
		 * Protected override methods
		 * 
		*/

		/// <summary>
		/// OnApplyTemplate override
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// set up the event handler for the template parts
			_toggleButton = this.Template.FindName("PART_DropDown", this) as ToggleButton;
			if (_toggleButton != null)
			{
				// create a context menu for the togglebutton
				System.Windows.Controls.ContextMenu cm = new ContextMenu();
				cm.PlacementTarget = _toggleButton;
				cm.Placement = PlacementMode.Bottom;

				// create a binding between the togglebutton's IsChecked Property
				// and the Context Menu's IsOpen Property
				Binding b = new Binding();
				b.Source = _toggleButton;
				b.Mode = BindingMode.TwoWay;
				b.Path = new PropertyPath(ToggleButton.IsCheckedProperty);

				cm.SetBinding(ContextMenu.IsOpenProperty, b);

				_toggleButton.ContextMenu = cm;
				_toggleButton.Checked += DropdownButton_Checked;
			}

			ScrollViewer scrollViewer = this.Template.FindName("PART_ScrollViewer", this) as ScrollViewer;

			// set up event handlers for the RepeatButtons Click event
			RepeatButton repeatLeft = this.Template.FindName("PART_RepeatLeft", this) as RepeatButton;
			if (repeatLeft != null)
			{
				repeatLeft.Click += delegate
				{
					if (scrollViewer != null)
						scrollViewer.LineLeft();

					GC.Collect();
				};
			}

			RepeatButton repeatRight = this.Template.FindName("PART_RepeatRight", this) as RepeatButton;
			if (repeatRight != null)
			{
				repeatRight.Click += delegate
				{
					if (scrollViewer != null)
						scrollViewer.LineRight();

					GC.Collect();
				};
			}

			// set up the event handler for the 'New Tab' Button Click event
			ButtonBase button = this.Template.FindName("PART_NewTabButton", this) as ButtonBase;
			if (button != null)
			{
				button.Click += delegate
				{
					int i = this.SelectedIndex;

					TabItem item = new TabItem();
					item.Header = "New Tab";

					if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
						this.Items.Add(item);
					else
						this.Items.Insert(++i, item);

					if (SelectNewTabOnCreate)
					{
						SelectedItem = item;

						VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
						if (itemsHost != null)
							itemsHost.MakeVisible(item, Rect.Empty);

						item.Focus();

						if (TabItemSelected != null)
							TabItemSelected(this, new TabItemEventArgs(item));
					}

					if (TabItemAdded != null)
						TabItemAdded(this, new TabItemEventArgs(item));
				};
			}
		}

		/// <summary>
		/// IsItemItsOwnContainerOverride
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TabItem;
		}
		/// <summary>
		/// GetContainerForItemOverride
		/// </summary>
		/// <returns></returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TabItem();
		}

		/// <summary>
		/// Handle the ToggleButton Checked event that displays a context menu of TabItem Headers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DropdownButton_Checked(object sender, RoutedEventArgs e)
		{
			if (_toggleButton == null) return;

			_toggleButton.ContextMenu.Items.Clear();
			if (TabStripPlacement == Dock.Bottom)
				_toggleButton.ContextMenu.Placement = PlacementMode.Top;
			else
				_toggleButton.ContextMenu.Placement = PlacementMode.Bottom;

			for (int i = 0; i < Items.Count; i++)
			{
				TabItem item = this.Items[i] as TabItem;
				if (item == null)
					return;

				object header = Helper.CloneElement(item.Header);
				object icon = Helper.CloneElement(item.Icon);

				MenuItem mi = new MenuItem() { Header = header, Icon = icon, Tag = i.ToString() };
				mi.Click += ContextMenuItem_Click;

				_toggleButton.ContextMenu.Items.Add(mi);
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (Items.Count == 0)
				return;

			TabItem ti = null;

			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				switch (e.Key)
				{
					case Key.F4:
					case Key.W:
						{
							ti = this.Items[SelectedIndex] as TabItem;

							if (ti.AllowDelete)
							{
								// This weird construct is because tabs that contain webbrowser controls crash
								// because they don't get a chance to peform a KeyUp.
								// This wat we delay the closing slightly giving them a time to do their key processing.
								Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() => RemoveItem(ti)));
							}

							break;
						}
					case Key.Tab:
						if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
						{
							int index = SelectedIndex;
							int direction = 1;
							if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
								direction = -1;

							while (true)
							{
								index += direction;
								if (index < 0)
									index = Items.Count - 1;
								else if (index > Items.Count - 1)
									index = 0;

								FrameworkElement ui = Items[index] as FrameworkElement;
								if (ui.Visibility == Visibility.Visible && ui.IsEnabled)
								{
									ti = Items[index] as TabItem;
									break;
								}
							}
						}

						e.Handled = SelectTabItem(ti);

						break;
				}
			}			

			if (!e.Handled)
				base.OnPreviewKeyDown(e);
		}

		internal bool SelectTabItem(TabItem ti)
		{
			var panel = Helper.FindVirtualizingTabPanel(this);

			if (panel != null)
				panel.MakeVisible(ti, Rect.Empty);				

			SelectedItem = ti;

			var result = ti.Focus();

			if (TabItemSelected != null)
				TabItemSelected(this, new TabItemEventArgs(ti));

			return result;
		}

		/// <summary>
		/// Handle the MenuItem's Click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ContextMenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem mi = sender as MenuItem;
			if (mi == null) return;

			int index;
			// get the index of the TabItem from the manuitems Tag property
			bool b = int.TryParse(mi.Tag.ToString(), out index);

			if (b)
			{
				TabItem tabItem = this.Items[index] as TabItem;
				if (tabItem != null)
				{
					VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
					if (itemsHost != null)
						itemsHost.MakeVisible(tabItem, Rect.Empty);

					tabItem.Focus();

					if (TabItemSelected != null)
						TabItemSelected(this, new TabItemEventArgs(tabItem));
				}
			}
		}

		/// <summary>
		/// Called by a child TabItem that wants to remove itself by clicking on the close button
		/// </summary>
		/// <param name="item"></param>
		public void RemoveItem(TabItem item)
		{
			// gives an opertunity to cancel the removal of the tabitem
			TabItemCancelEventArgs c = new TabItemCancelEventArgs(item);
			if (TabItemClosing != null)
				TabItemClosing(item, c);

			if (c.Cancel == true)
				return;
			
			var closable = Items.Cast<TabItem>().Count(t => t.AllowDelete);

			if (item.AllowDelete) closable--;

			// If there are no closable tabs left, move to home tab
			if (closable == 0)
				SelectedIndex = 0;

			this.Items.Remove(item);

			if (TabItemClosed != null)
				TabItemClosed(this, new TabItemEventArgs(item));			
		}
	}
}
