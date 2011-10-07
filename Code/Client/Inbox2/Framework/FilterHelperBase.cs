using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Inbox2.Framework
{
	public abstract class FilterHelperBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private CollectionViewSource view;
		private bool isLoading;

		protected abstract string SettingsKeyPrefix { get; }

		protected FilterHelperBase(CollectionViewSource view)
		{
			this.view = view;
		}

		public virtual void SaveSettings()
		{
			foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (property.PropertyType == typeof(bool))
				{
					ClientState.Current.Context.SaveSetting(
						SettingsKeyPrefix + "Filter/" + property.Name, property.GetValue(this, null));
				}
			}
		}

		public virtual void LoadSettings()
		{
			isLoading = true;

			foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (property.PropertyType == typeof (bool))
				{
					if (ClientState.Current.Context.HasSetting(SettingsKeyPrefix + "Filter/" + property.Name))
					{
						bool val = (bool)ClientState.Current.Context.GetSetting(SettingsKeyPrefix + "Filter/" + property.Name);

						property.SetValue(this, val, null);
					}
				}
			}

			isLoading = false;
		}

		protected void RefreshView()
		{
			if (isLoading)
				return;

			if (view != null)
				view.View.Refresh();
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
