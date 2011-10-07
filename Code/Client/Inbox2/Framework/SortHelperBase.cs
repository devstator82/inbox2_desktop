using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework
{
	public abstract class SortHelperBase : INotifyPropertyChanged
	{
		public event EventHandler BeforeSort;
		public event EventHandler AfterSort;

		public event PropertyChangedEventHandler PropertyChanged;		

		private CollectionViewSource source;
		
		protected bool ascending;
		protected bool descending;

		private bool isLoading;
        private bool isSorting;

		#region Properties

		protected abstract string SettingsKeyPrefix { get; }
		
		public bool Ascending
		{
			get { return ascending; }
			set
			{
				ascending = value;

				PerformSort();
			}
		}
		
		public bool Descending
		{
			get { return descending; }
			set
			{
				descending = value;

				PerformSort();
			}
		}

		#endregion

		protected SortHelperBase(CollectionViewSource source)
		{
			this.source = source;
		}

		protected virtual void PerformSort()
		{
            if (isLoading || isSorting)
				return;

			if (BeforeSort != null)
				BeforeSort(this, EventArgs.Empty);

		    isSorting = true;

			var filter = source.View.Filter;

			source.SortDescriptions.Clear();

			foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (property.PropertyType == typeof(bool) && (bool)property.GetValue(this, null))
				{
					var attributes = property.GetCustomAttributes(typeof (SortOnPropertyAttribute), true);

					if (attributes != null && attributes.Length > 0)
					{
						SortOnPropertyAttribute attr = (SortOnPropertyAttribute) attributes[0];

						source.SortDescriptions.Add(new SortDescription(attr.PropertyName,
							Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
					}
				}
			}

			source.View.Filter = filter;

		    isSorting = false;

			if (AfterSort != null)
				AfterSort(this, EventArgs.Empty);
		}

		public virtual void SaveSettings()
		{
			foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (property.PropertyType == typeof (bool) && (bool) property.GetValue(this, null))
				{
					var attributes = property.GetCustomAttributes(typeof (SortOnPropertyAttribute), true);

					if (attributes != null && attributes.Length > 0)
					{
						ClientState.Current.Context.SaveSetting(SettingsKeyPrefix + "/Sort/On", property.Name);
						ClientState.Current.Context.SaveSetting(SettingsKeyPrefix + "/Sort/Ascending", ascending);
					}
				}
			}
		}

		public virtual void LoadSettings()
		{
			if (!(ClientState.Current.Context.HasSetting(SettingsKeyPrefix + "/Sort/On")
				&& ClientState.Current.Context.HasSetting(SettingsKeyPrefix + "/Sort/Ascending")))
			{
				// Settings not defined yet
				PerformSort();
				return;
			}

			isLoading = true;

			string sortOn = (string)ClientState.Current.Context.GetSetting(SettingsKeyPrefix + "/Sort/On");
			bool sortAscending = (bool)ClientState.Current.Context.GetSetting(SettingsKeyPrefix + "/Sort/Ascending");

			ascending = sortAscending;
			descending = !ascending;

			PropertyInfo prop = GetType().GetProperty(sortOn, BindingFlags.Public | BindingFlags.Instance);

			if (prop == null)
			{
				Logger.Error("Property {0} was not found on Sort Helper", LogSource.UI, sortOn);

				return;
			}

			prop.SetValue(this, true, null);

			isLoading = false;

			PerformSort();
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
