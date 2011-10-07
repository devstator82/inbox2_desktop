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
using System.Windows.Threading;
using Inbox2.Core.Threading;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Threading;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.UI.Controls
{
	/// <summary>
	/// Interaction logic for AdminControl.xaml
	/// </summary>
	public partial class AdminControl : UserControl
	{
		public AdvancedObservableCollection<IBackgroundTask> Tasks { get; private set; }

		protected DispatcherTimer timer;

		public TaskQueue TaskQueue
		{
			get { return (TaskQueue)ClientState.Current.TaskQueue; }
		}

		public List<TaskProcessor> TaskProcessors
		{
			get { return TaskQueue.ProcessingPool.Processors; }
		}

		public AdminControl()
		{
			InitializeComponent();

			Tasks = new AdvancedObservableCollection<IBackgroundTask>();

			timer = new DispatcherTimer(TimeSpan.FromSeconds(1),
				DispatcherPriority.Normal,
				delegate
				{
					Application.Current.Dispatcher.BeginInvoke(
						(Action)(() => Tasks.Replace(TaskQueue.Tasks.ToList())), DispatcherPriority.DataBind);
				},
				Dispatcher.CurrentDispatcher);

			timer.IsEnabled = true;

			DataContext = this;
		}
	}
}
