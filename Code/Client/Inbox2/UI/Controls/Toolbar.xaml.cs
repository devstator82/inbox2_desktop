using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Threading.Progress;
using Inbox2.Platform.Framework;

namespace Inbox2.UI.Controls
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl, INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;

		public ProgressManager ProgressManager
		{
			get { return ProgressManager.Current; }
		}

    	public AppMessages Messages
    	{
			get { return ClientState.Current.Messages; }
    	}

    	public SyncState SyncState
    	{
			get { return ClientState.Current.SyncState; }
    	}

    	public bool HasNetworkConnection
    	{
			get { return NetworkInterface.GetIsNetworkAvailable(); }
    	}

        public Toolbar()
        {
			using (new CodeTimer("Toolbar/Constructor"))
			{
				InitializeComponent();

				Messages.OnSuccess += delegate { SuccessInfoPopup.TryOpen(3000, 10000); };

				NetworkChange.NetworkAvailabilityChanged += delegate { OnPropertyChanged("HasNetworkConnection"); };

				EventBroker.Subscribe(AppEvents.RebuildToolbar, 
					() => Thread.CurrentThread.ExecuteOnUIThread(RebuildToolbar));

				DataContext = this;
			}

            EventBroker.Subscribe(AppEvents.RequestOpenToolbarItem, delegate(int index)
                {
                    if (index <= ToolbarPluginsRightHost.Children.Count)
                    {
                        var plugin = ToolbarPluginsRightHost.Children[index -1] as IInvokeProvider;

                        if (plugin != null)
                            plugin.Invoke();
                    }
                });
        }		

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)RebuildToolbar);
		}		
		
		void RebuildToolbar()
		{
			foreach (var child in ToolbarPluginsLeftHost.Children.OfType<IDisposable>())
				child.Dispose();

			foreach (var child in ToolbarPluginsRightHost.Children.OfType<IDisposable>())
				child.Dispose();

			ToolbarPluginsLeftHost.Children.Clear();
			ToolbarPluginsRightHost.Children.Clear();

			CreatePluginButtons();
		}

		void CreatePluginButtons()
		{
			var items = 
				PluginsManager.Current.Plugins
					.Where(p => p.ToolbarItems != null)
					.SelectMany(p => p.ToolbarItems)
					.ToList();

			foreach (var item in items)
			{
				var control = item.CreateToolbarElement();

				if (item.ToolbarAlignment == ToolbarAlignment.Left)
					ToolbarPluginsLeftHost.Children.Add(control);
				else
					ToolbarPluginsRightHost.Children.Add(control);
			}
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
