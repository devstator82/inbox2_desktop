using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Plugins.Documents.Helpers;

namespace Inbox2.Plugins.Documents.Controls
{
	public enum DetailsViewType
	{
		Documents,
		Images,
		Search
	}

	/// <summary>
	/// Interaction logic for Column.xaml
	/// </summary>
	public partial class Column : UserControl, IFocusChild
	{
		#region Fields

		public event EventHandler<EventArgs> SelectedDocumentChanged;

		#endregion

		#region Properties

		public DetailsViewType DetailsViewType { get; set; }

		public DocumentsPlugin Plugin
		{
			get { return PluginsManager.Current.GetPlugin<DocumentsPlugin>(); }
		}

		public DocumentsState State
		{
			get { return (DocumentsState)Plugin.State; }
		}

		public CollectionViewSource DocumentsViewSource
		{
			get
			{
				return DetailsViewType == DetailsViewType.Images ?
					State.ImagesViewSource : State.DocumentsViewSource;
			}
		}

		public string Title
		{
			get
			{
				return DetailsViewType == DetailsViewType.Images ?
					Strings.Images : Strings.Documents;
			}
		}

		public SortHelperBase Sort
		{
			get
			{
				return DetailsViewType == DetailsViewType.Images ?
					State.ImageSort : State.DocumentSort;
			}
		}

		public bool IsInSearchMode
		{
			get { return DetailsViewType == DetailsViewType.Search; }
		}

		public UIElement FocusElement
		{
			get { return DocumentsListView; }
		}

		#endregion

		#region Constructors

		public Column()
		{
			InitializeComponent();

			DataContext = this;
		}

		#endregion

		#region Methods

		public void OverrideViewSource(CollectionViewSource newSource)
		{
			DocumentsListView.ItemsSource = newSource == null ? null : newSource.View;
		}

		void SelectDocuments(IEnumerable documents)
		{
			State.SelectedDocuments.ReplaceWithCast(documents);

			OnSelectedDocumentChanged();
		}

		void SelectDocument(Document document)
		{
			State.SelectedDocuments.Replace(new[] { document });

			OnSelectedDocumentChanged();
		}

		void OnSelectedDocumentChanged()
		{
			if (SelectedDocumentChanged != null)
			{
				SelectedDocumentChanged(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Command handlers

		void New_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.New();
		}

		void View_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.View();
		}

		void Reply_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Reply();
		}

		void Forward_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Forward();
		}

		void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.Delete();
		}

		void MarkRead_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkRead();
		}

		void MarkUnread_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Plugin.State.MarkUnread();
		}

		void OpenDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.View, State.SelectedDocument);
		}

		void SaveDocument_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EventBroker.Publish(AppEvents.Save, State.SelectedDocument);
		}

		void View_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanView;
		}

		void Reply_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanReply;
		}

		void Forward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanForward;
		}

		void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanDelete;
		}

		void MarkRead_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkRead;
		}

		void MarkUnread_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Plugin != null
						   && Plugin.State.CanMarkUnread;
		}		

		void OpenDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = State.SelectedDocument != null;
		}

		void SaveDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = State.SelectedDocument != null;
		}

		#endregion

		#region Event handlers

		void DocumentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectDocuments(DocumentsListView.SelectedItems);
		}

		void DocumentsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var document = (e.OriginalSource as DependencyObject)
				.FindListViewItem<Document>(DocumentsListView.ItemContainerGenerator);

			if (document != null)
			{
				SelectDocument(document);

				State.View();
			}
        }

		void DocumentsListView_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			// Extra check to prevent tab switch from interfering with selection
			SelectDocument((Document)DocumentsListView.SelectedItem);

			if (e.Key == Key.Delete)
			{
				State.Delete();
			}
			if (e.Key == Key.Enter)
			{
				State.View();
			}
		}

		void FileDragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;

			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			if (fileNames != null && fileNames.Length > 0)
			{
				bool allowed = false;

				if (DetailsViewType == DetailsViewType.Documents)
				{
					allowed = !fileNames.Any(s => Document.IsFilenameImage(s));
				} 
				else if (DetailsViewType == DetailsViewType.Images)
				{
					allowed = !fileNames.Any(s => !Document.IsFilenameImage(s));
				}

				e.Effects = allowed ? DragDropEffects.Copy : DragDropEffects.None;
			}

			e.Handled = true;
		}

		void FileDrop(object sender, DragEventArgs e)
		{
			string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

			foreach (var fileName in fileNames)
			{
				DragDropHelper.UploadFile(fileName);
			}

			e.Handled = true;
		}

		#endregion		
	}
}
