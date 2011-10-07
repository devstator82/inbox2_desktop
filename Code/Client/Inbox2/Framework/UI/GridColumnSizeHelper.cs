using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.UI
{
	public static class GridColumnSizeHelper
	{
		#region Dependency properties

		public static readonly DependencyProperty SaveGridColumnsSizeProperty =
			DependencyProperty.RegisterAttached("SaveGridColumnsSize", typeof(bool), typeof(GridColumnSizeHelper), new UIPropertyMetadata(false, GridColumnSizeHelper_SaveGridColumnsSizeChanged));

		public static readonly DependencyProperty PreviousGridLengthProperty =
			DependencyProperty.RegisterAttached("PreviousGridLength", typeof(GridLength), typeof(GridColumnSizeHelper));

		public static readonly DependencyProperty CollapseActionProperty =
			DependencyProperty.RegisterAttached("CollapseAction", typeof(Action), typeof(GridColumnSizeHelper));

		public static bool GetSaveGridColumnsSize(DependencyObject obj)
		{
			return (bool)obj.GetValue(SaveGridColumnsSizeProperty);
		}

		public static void SetSaveGridColumnsSize(DependencyObject obj, bool value)
		{
			obj.SetValue(SaveGridColumnsSizeProperty, value);
		}

		public static GridLength GetPreviousGridLength(DependencyObject obj)
		{
			return (GridLength)obj.GetValue(PreviousGridLengthProperty);
		}

		public static void SetPreviousGridLength(DependencyObject obj, GridLength value)
		{
			obj.SetValue(PreviousGridLengthProperty, value);
		}

		public static Action GetCollapseAction(DependencyObject obj)
		{
			return (Action)obj.GetValue(CollapseActionProperty);
		}

		public static void SetCollapseAction(DependencyObject obj, Action value)
		{
			obj.SetValue(CollapseActionProperty, value);
		}

		#endregion

		static void GridColumnSizeHelper_SaveGridColumnsSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			Grid grid = (Grid) sender;

			if (String.IsNullOrEmpty(grid.Name))
				throw new ApplicationException("Unable to attach to grid with an empty name. GridColumnSizeHelper requires a unique name for the grid in order to function properly.");

			grid.Loaded += grid_Loaded;
			grid.Unloaded += grid_Unloaded;
		}		

		public static bool SetColumnSizes(Grid grid, out List<object> columns)
		{
			if (CanSetGridSizes(grid, out columns))
			{
				Logger.Debug("Setting column values for grid ", LogSource.Layout, grid.Name);

				for (int i = 0; i < columns.Count; i++)
				{
					ColumnSize size = (ColumnSize)columns[i];

					// Set proper width for each column, specifiying a hardcoded en-US cultureinfo fixes a nasty parsing issue
					grid.ColumnDefinitions[i].Width =
						(GridLength)new GridLengthConverter().ConvertFrom(null, CultureInfo.GetCultureInfo("en-US"), size.Width);

					if (size.IsCollapsed)
					{
						// Save previous size for restoring from collapsed state
						SetPreviousGridLength(grid.ColumnDefinitions[i], 
							(GridLength)new GridLengthConverter().ConvertFrom(size.PreviousWidth));
					}

					Logger.Debug("\tColumn {0} has Width {1}, Width from settings was {2}",
					             LogSource.Layout, i, grid.ColumnDefinitions[i].Width, columns[i].ToString());
				}

				return true;
			}

			return false;
		}

		public static void SaveColumnSizes(Grid grid)
		{
			Logger.Debug("Storing column values for grid ", LogSource.Layout, grid.Name);

			for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
			{
				string key = String.Format("/Settings/GridColumnSize/{0}/{1}", grid.Name, i);

				Logger.Debug("\tColumn {0} has Width {1}", LogSource.Layout, i, grid.ColumnDefinitions[i].Width);

				ColumnSize size = new ColumnSize { Width = grid.ColumnDefinitions[i].Width.ToString() };

				if (grid.ColumnDefinitions[i].Width.Value == 0)
				{
					size.IsCollapsed = true;
					size.PreviousWidth = GetPreviousGridLength(grid.ColumnDefinitions[i]).ToString();
				}				

				// Watch the ToString call here, if you save a reference to the Width property, the value 
				// will default to Auto during application shutdown because another Save call is triggered.
				ClientState.Current.Context.SaveSetting(key, size);
			}
		}

		static bool CanSetGridSizes(Grid grid, out List<object> columns)
		{
			// Get all stored column settings for this grid
			columns = GetGridSizesFor(grid);

			if (columns.Count == 0)
			{
				// No saved column settings found
				return false;
			}

			if (grid.ColumnDefinitions.Count != columns.Count)
			{
				Logger.Debug("Invalid column count for grid {0}", LogSource.Layout, grid.Name);

				return false;
			}

			return true;
		}

		static List<object> GetGridSizesFor(Grid grid)
		{
			string key = String.Format("/Settings/GridColumnSize/{0}/", grid.Name);

			return ClientState.Current.Context.GetSettingsFor(key).ToList();
		}

		static void grid_Loaded(object sender, RoutedEventArgs e)
		{
			Grid grid = (Grid) sender;

			// Grids containing the word root are ignored by the load logic as these are grids
			// which contain dynamically loaded content.
			if (grid.Name.IndexOf("root", StringComparison.InvariantCultureIgnoreCase) > -1)
				return;

			List<object> columns;

			SetColumnSizes(grid, out columns);
		}

		static void grid_Unloaded(object sender, RoutedEventArgs e)
		{
			Grid grid = (Grid) sender;

			SaveColumnSizes(grid);
		}
	}
}
