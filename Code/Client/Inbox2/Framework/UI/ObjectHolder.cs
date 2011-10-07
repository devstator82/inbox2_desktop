using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Inbox2.Framework.UI
{
	public class ObjectHolder<T> : FrameworkElement, INotifyPropertyChanged where T : class
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected T value;

		public T Value
		{
			get { return this.value; }
			protected set
			{
				if (this.value != null)
				{
					if (this.value is INotifyPropertyChanged)
						(this.value as INotifyPropertyChanged).PropertyChanged -= value_PropertyChanged;
				}

				this.value = value;

				if (this.value != null)
					if (this.value is INotifyPropertyChanged)
						(this.value as INotifyPropertyChanged).PropertyChanged += value_PropertyChanged;
			}
		}

		void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// This method raises the PropertyChanged event twice.  First it nullifies the 'value'
			// field and raises PropertyChanged.  Then it sets field back to the actual object and 
			// raises it again.  This is necessary because the WPF binding system will ignore a 
			// PropertyChanged notification if the property returns the same object reference as before.

			T temp = this.value;

			this.value = null;
			this.OnPropertyChanged("Value");

			this.value = temp;
			this.OnPropertyChanged("Value");
		}


		public ObjectHolder(T value)
		{
			this.Value = value;
			this.Unloaded += ObjectHolder_Unloaded;
		}

		void ObjectHolder_Unloaded(object sender, RoutedEventArgs e)
		{
			// Cleans up any event handlers still hanging around
			Value = null;
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
