using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FluidKit.Controls;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Stats;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Framework;
using Inbox2.Plugins.Documents.Helpers;

namespace Inbox2.Plugins.Documents.Controls
{
	/// <summary>
	/// Interaction logic for DetailsView.xaml
	/// </summary>
	public partial class ImagesView : UserControl, IControllableTab
	{
		#region Fields

		public event EventHandler<EventArgs> RequestCloseTab;

		#endregion

		#region Properties

		public string Title
		{
			get { return Strings.Images; }
		}

		public Control CustomHeaderContent
		{
			get { return new SearchDockControl(DetailsViewType.Images); }
		}

		public PluginPackage Plugin
		{
			get { return PluginsManager.Current.GetPlugin<DocumentsPlugin>(); }
		}

		public DocumentsState State
		{
			get { return (DocumentsState)Plugin.State; }
		}

		#endregion

		#region Construction

		public ImagesView()
		{
			using (new CodeTimer("ImagesView/Constructor"))
			{
				InitializeComponent();

				DataContext = this;

				State.ImageSort.BeforeSort += delegate { DocumentsListBox.ItemsSource = null; };
				State.ImageSort.AfterSort += delegate { DocumentsListBox.ItemsSource = State.ImagesViewSource.View; };
			}
		}

		#endregion

		#region Event Handlers

		void DocumentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Due to an issue with ElementFlow we need to synchronize the selected items manually
			var elementFlow = VisualTreeWalker.FindName<ElementFlow>("ef", DocumentsListBox);

            if (elementFlow != null)
			    elementFlow.SelectedIndex = DocumentsListBox.SelectedIndex;
		}

		void ElementFlowSelectedIndexChanged(object sender, EventArgs e)
		{
			DocumentsListBox.SelectedIndex = ((ElementFlow)sender).SelectedIndex;

			State.SelectedDocuments.Replace(new[] { (Document)DocumentsListBox.SelectedItem });
		}

		void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Open document in images view");

			EventBroker.Publish(AppEvents.View, State.SelectedDocument);
		}

		void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Save document in images view");

			EventBroker.Publish(AppEvents.Save, State.SelectedDocument);
		}

		void DeleteDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ClientStats.LogEvent("Delete document in images view");

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

		void FileDragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			if (fileNames != null && fileNames.Length > 0)
			{
				bool hasNonImages = fileNames.Any(s => !Document.IsFilenameImage(s));

				e.Effects = hasNonImages ? DragDropEffects.None : DragDropEffects.Copy;
			}

			e.Handled = true;
		}

		void FileDrop(object sender, DragEventArgs e)
		{
			ClientStats.LogEvent("Drop document in images view");

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			foreach (var fileName in fileNames)
			{
				DragDropHelper.UploadFile(fileName);
			}

			e.Handled = true;
		}

		void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			State.DetailsViewType = DetailsViewType.Images;
		}

		#endregion		
	}
}
