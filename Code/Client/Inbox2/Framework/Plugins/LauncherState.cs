using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.ServiceModel;
using Inbox2.Framework.Collections;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.UI;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;

namespace Inbox2.Framework.Plugins
{
	public class LauncherState : INotifyPropertyChanged
	{
		#region Singleton access

		private static LauncherState _Current;

		/// <summary>
		/// Initializes the <see cref="LauncherState"/> class.
		/// </summary>
		static LauncherState()
		{
			_Current = new LauncherState();
		}

		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
		public static LauncherState Current
		{
			get { return _Current; }
		}

		#endregion

		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private AdvancedObservableCollection<FileInfo> selectedFiles;
		private AdvancedObservableCollection<SourceAddress> selectedAddresses;
		private AdvancedObservableCollection<Uri> selectedUris;
		private IClientAutomationService channel;

		#endregion

		#region Properties

		[Import]
		public List<LauncherCommand> LauncherCommands { get; set; }

		public AdvancedObservableCollection<FileInfo> SelectedFiles
		{
			get { return selectedFiles; }
		}

		public AdvancedObservableCollection<SourceAddress> SelectedAddresses
		{
			get { return selectedAddresses; }
		}

		public AdvancedObservableCollection<Uri> SelectedUris
		{
			get { return selectedUris; }
		}

		public IClientAutomationService Channel
		{
			get { return channel; }
		}

		public bool HasSelectedFiles
		{
			get { return SelectedFiles.Count > 0; }
		}

		public bool HasSelectedAddresses
		{
			get { return SelectedAddresses.Count > 0; }
		}

		public bool HasSelectedUris
		{
			get { return SelectedUris.Count > 0; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="LauncherState"/> class.
		/// </summary>
		private LauncherState()
		{
			ChannelFactory<IClientAutomationService> factory
				= new ChannelFactory<IClientAutomationService>(
					new NetNamedPipeBinding(), "net.pipe://localhost/Inbox2AutomationService");
			
			channel = factory.CreateChannel();
			selectedFiles = new AdvancedObservableCollection<FileInfo>();
			selectedAddresses = new AdvancedObservableCollection<SourceAddress>();
			selectedUris = new AdvancedObservableCollection<Uri>();
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