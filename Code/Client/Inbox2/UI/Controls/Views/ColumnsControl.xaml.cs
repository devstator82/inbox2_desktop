using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Inbox2.Core.Configuration;
using Inbox2.Framework;
using Inbox2.Framework.Plugins.SharedControls;
using Inbox2.Framework.UI;
using Inbox2.Platform.Logging;

namespace Inbox2.UI.Controls.Views
{
    /// <summary>
    /// Interaction logic for OverviewContainerControl.xaml
    /// </summary>
    public partial class ColumnsControl : UserControl
    {
        private ColumnCollapseView collapseView;

		public ColumnsControl()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            CreatePluginColumns();
        }

        void CreatePluginColumns()
        {
            int i;
        	var disabled = DebugKeys.DisabledPlugins;

            var columns =
                PluginsManager.Current.Plugins
					.Where(p => p.Colomn != null)
					.Where(p => !disabled.Contains(p.Name))
                    .OrderBy(p => p.Name)
                    .Select(p => p.Colomn)
                    .ToList();

            // Create a column for each plugin
            for (i = 0; i < columns.Count; i++)
            {
                var control = columns[i].CreateColumnView();
                var def = new ColumnDefinition { Width = new GridLength(columns[i].PreferredWidth, GridUnitType.Star) };

                OverviewContainerRootGrid.ColumnDefinitions.Add(def);
                OverviewContainerRootGrid.Children.Add(control);

                Grid.SetColumn(control, i);
                GridSplitter splitter = null;

                if (i < columns.Count - 1)
                {
                    splitter = CreateSplitter();

                    OverviewContainerRootGrid.Children.Add(splitter);
                    Grid.SetColumn(splitter, i);
                }

                var button = GetChildCollapseButton((FrameworkElement)control);

                // Attach collapse functionality to button
                if (button != null)
                {
                    GridColumnSizeHelper.SetCollapseAction(def, () => CollapseColumn(splitter, def, button.Content.ToString()));

                    button.Click += delegate { GridColumnSizeHelper.GetCollapseAction(def)(); };
                }
            }

            // Add ColumnCollapseView
            collapseView = new ColumnCollapseView();

            OverviewContainerRootGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Auto) });
            OverviewContainerRootGrid.Children.Add(collapseView);

            Grid.SetColumn(collapseView, i++);

            List<object> storedColumns;
            bool success = GridColumnSizeHelper.SetColumnSizes(OverviewContainerRootGrid, out storedColumns);

            // See if column was collapsed the last time we saved app state
            if (success)
            {
                foreach (var def in OverviewContainerRootGrid.ColumnDefinitions)
                {
                    if (def.Width.Value == 0)
                    {
                        // First set old width so that expand/save logic gets executed correctly
                        def.Width = GridColumnSizeHelper.GetPreviousGridLength(def);

                        Action x = GridColumnSizeHelper.GetCollapseAction(def);

                        // Execute collapse action defined when column was created
                        if (x != null) x();
                    }
                }
            }
        }

        GridSplitter CreateSplitter()
        {
            var splitter = new GridSplitter { Style = (Style)FindResource("OverviewsplitterBorderStyle") };

            splitter.MouseDoubleClick += delegate
            {
                // Collapse adjescent (right) column on double-click of this splitter
                int column = Grid.GetColumn(splitter);

                // Get the column adjescent to this column
                ColumnDefinition nextDef = OverviewContainerRootGrid.ColumnDefinitions[column + 1];

                Button button = null;

                foreach (UIElement child in OverviewContainerRootGrid.Children)
                {
                    // Find child belonging to the given column index
                    if (Grid.GetColumn(child) == column + 1)
                    {
                        // Get collapse button from next column definition
                        button = GetChildCollapseButton((FrameworkElement)child);
                    }
                }

                if (button != null)
                    CollapseColumn(splitter, nextDef, button.Content.ToString());
            };

            return splitter;
        }

        Button GetChildCollapseButton(FrameworkElement control)
        {
            Button button = control.FindName("CollapsableButton") as Button;

            if (button == null)
            {
                Logger.Debug("Control {0} did not have a button named CollapsableButton, this is probably an error", LogSource.UI, control);

                return null;
            }

            return button;
        }

        void CollapseColumn(GridSplitter splitter, ColumnDefinition def, string content)
		{
			// Ignore collapse if popup is opened
			if (PopupManager.ActivePopup != null)
				return;

			int collapsed = 0;

			// Count the number of collapsed items (exclusing collapseView and rockScroll)
			for (int i = 0; i < OverviewContainerRootGrid.ColumnDefinitions.Count -2; i++)
			{
				if (OverviewContainerRootGrid.ColumnDefinitions[i].Width.Value == 0)
					collapsed++;
			}

			// Only allowed to collapse when there are at least two columns which have Width > 0
			if (collapsed >= OverviewContainerRootGrid.ColumnDefinitions.Count - 3)
				return;

			GridColumnSizeHelper.SetPreviousGridLength(def, def.Width);

            //Start Collapse Column Animation, only when the column Width != 0
            if (def.Width != new GridLength(0))
            {
                Storyboard CollapseColumnGrid = (Storyboard)FindResource("CollapseColumn");
                Storyboard.SetTarget(CollapseColumnGrid, def);

                GridLengthAnimation gla = CollapseColumnGrid.Children[0] as GridLengthAnimation;
                gla.From = def.Width;
                gla.To = new GridLength(0);

                CollapseColumnGrid.Begin(this);
            }

		    if (splitter != null)
				splitter.IsEnabled = false;

			collapseView.AddCollapsedView(def, content, delegate
			{
                //Start Expand Column Animation, only when the column Width = 0
                if (def.Width == new GridLength(0))
                {
                    def.Width = GridColumnSizeHelper.GetPreviousGridLength(def);
                    GridLength previousWidth = def.Width;
                    def.Width = new GridLength(0);

                    Storyboard ExpandColumnGrid = (Storyboard)FindResource("CollapseColumn");
                    Storyboard.SetTarget(ExpandColumnGrid, def);

                    GridLengthAnimation gla = ExpandColumnGrid.Children[0] as GridLengthAnimation;
                    gla.From = new GridLength(0);
                    gla.To = previousWidth;

                    ExpandColumnGrid.Begin(this);
                }

                if (splitter != null)
					splitter.IsEnabled = true;
			});
		}
    }
}
