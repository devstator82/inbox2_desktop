using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Framework.Localization;
using Inbox2.Platform.Framework;
using Inbox2.Plugins.Documents.Helpers;

namespace Inbox2.Plugins.Documents.Controls
{
	/// <summary>
	/// Interaction logic for DocumentsView.xaml
	/// </summary>
	public partial class DocumentsView : UserControl, INotifyPropertyChanged, IControllableTab
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<EventArgs> RequestCloseTab;

		#endregion

		#region Properties

		public string Title
		{
			get { return Strings.Documents; }
		}

		public Control CustomHeaderContent
		{
			get { return new SearchDockControl(DetailsViewType.Documents); }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<DocumentsPlugin>(); }
		}		

		public DocumentsState State
		{
			get { return (DocumentsState)Plugin.State; }
		}

		public CollectionViewSource DocumentsViewSource
		{
			get { return State.DocumentsViewSource; }
		}

		#endregion

		#region Construction

		public DocumentsView()
		{
			using (new CodeTimer("DocumentsView/Constructor"))
			{
				InitializeComponent();

				DataContext = this;
			}
		}

		#endregion

		#region Methods

		void SelectDocuments(IEnumerable documents)
		{
			State.SelectedDocuments.ReplaceWithCast(documents);
		}

		void SelectDocument(Document document)
		{
			State.SelectedDocuments.Replace(new[] { document });
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void FileDragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			if (fileNames != null && fileNames.Length > 0)
			{
				bool hasImage = fileNames.Any(s => Document.IsFilenameImage(s));

				e.Effects = hasImage ? DragDropEffects.None : DragDropEffects.Copy;
			}

			e.Handled = true;
		}

		void FileDrop(object sender, DragEventArgs e)
		{
			ClientStats.LogEvent("Drop file in documents view");

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			foreach (var fileName in fileNames)
			{
				DragDropHelper.UploadFile(fileName);
			}

			e.Handled = true;
		}

		#endregion

		#region Event handlers

		void DocumentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectDocuments(DocumentsListView.SelectedItems);

			OnPropertyChanged("State");
		}

		void DocumentsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ClientStats.LogEvent("View document in documents view (mouse)");

			EventBroker.Publish(AppEvents.View, State.SelectedDocument);
		}

		void DocumentsListView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			// Extra check to prevent tab switch from interfering with selection
			SelectDocument((Document)DocumentsListView.SelectedItem);

			if (e.Key == Key.Delete)
			{
				ClientStats.LogEvent("Delete document in documents view (keyboard)");

				State.Delete();
			}
			if (e.Key == Key.Enter)
			{
				ClientStats.LogEvent("View document in documents view (keyboard)");

				State.View();
			}
		}

		void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Open document in documents view");

			EventBroker.Publish(AppEvents.View, State.SelectedDocument);
		}

		void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Save document in documents view");

			EventBroker.Publish(AppEvents.Save, State.SelectedDocument);
		}

		void DeleteDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Delete document in documents view");

			State.Delete();
		}

		void OpenDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = State.SelectedDocument != null;
		}

		void SaveDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = State.SelectedDocument != null;
		}

		void DeleteDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = State.SelectedDocument != null;
		}

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			State.DetailsViewType = DetailsViewType.Documents;
		}

		#endregion		
	}
}
