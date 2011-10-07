using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Inbox2.Plugins.StatusUpdates.Controls
{
	/// <summary>
	/// Interaction logic for Column.xaml
	/// </summary>
	public partial class Column : UserControl
	{		
		public Column()
		{
			InitializeComponent();

			DataContext = this;
		}

		public void OverrideViewSource(CollectionViewSource newSource)
		{
			StatusUpdatesSource.ItemsSource = newSource == null ? null : newSource.View;
		}
	}
}
