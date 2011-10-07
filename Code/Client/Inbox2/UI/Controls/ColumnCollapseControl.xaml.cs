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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inbox2.UI.Controls
{
    /// <summary>
    /// Interaction logic for ColumnCollapseControl.xaml
    /// </summary>
    public partial class ColumnCollapseControl : UserControl
    {
        public string ColumnName { get; set; }

		public Action ExpandAction { get; set; }

        public ColumnCollapseControl()
        {
            InitializeComponent();

            DataContext = this;

            // Start Expand Column Animation
            Storyboard ExpandColumnGrid = (Storyboard)FindResource("ExpandColumn");
            ExpandColumnGrid.Begin(this);
        }

		void Button_Click(object sender, RoutedEventArgs e)
		{
            if (ExpandAction != null)
            {
                // Start Collapse Column Animation
                Storyboard CollapseColumnGrid = (Storyboard)FindResource("CollapseColumn");
                CollapseColumnGrid.Completed += delegate { ExpandAction(); };
                CollapseColumnGrid.Begin(this);
            }
		}
    }
}
