using System.Windows;

namespace Inbox2.Core.Upgrade.Windows
{
    /// <summary>
    /// Interaction logic for Updater.xaml
    /// </summary>
	public partial class UpgradeWindow : Window
    {    	
    	public UpgradeWindow()
        {
        	InitializeComponent();
        }

		public void SetMaximum(int max)
		{
			ProgressBar.Maximum = max;
		}

		public void SetProgress(int progress)
		{
			ProgressBar.Value = progress;
		}
	}
}
