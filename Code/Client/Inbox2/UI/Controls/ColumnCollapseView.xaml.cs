using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inbox2.UI.Controls
{
    /// <summary>
    /// Interaction logic for ColumnCollapseView.xaml
    /// </summary>
    public partial class ColumnCollapseView : UserControl
    {
        public ColumnCollapseView()
        {
            InitializeComponent();
        }

    	public void AddCollapsedView(ColumnDefinition def, string header, Action expandAction)
    	{
			var control = new ColumnCollapseControl { ColumnName = header };

			control.ExpandAction = delegate
           	{
				// Execute expand action
           		if (expandAction != null)
           			expandAction();

				// Remove control from view
				ColumnCollapseStackPanel.Children.Remove(control);
           	};

			ColumnCollapseStackPanel.Children.Add(control);
    	}
    }
}
