using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.ValueTypes;
using Inbox2.Framework.Threading.AsyncUpdate;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Interfaces.Enumerations;
using Inbox2.Plugins.Documents.Controls;
using Inbox2.Plugins.Documents.Helpers;

namespace Inbox2.Plugins.Documents
{
	public class DocumentsState : PluginStateBase
	{
		#region Properties

		/// <summary>
		/// Gets or sets the conversations.
		/// </summary>
		/// <value>The conversations.</value>
		public ThreadSafeCollection<Document> Documents
		{
			get { return VirtualMailBox.Current.Documents; }
		}

		/// <summary>
		/// Gets or sets the documents view source.
		/// </summary>
		/// <value>The documents view source.</value>
		public CollectionViewSource DocumentsViewSource { get; private set; }

		/// <summary>
		/// Gets or sets the images view source.
		/// </summary>
		/// <value>The images view source.</value>
		public CollectionViewSource ImagesViewSource { get; private set; }

		/// <summary>
		/// Gets or sets the selected documents.
		/// </summary>
		/// <value>The selected documents.</value>
		public AdvancedObservableCollection<Document> SelectedDocuments { get; private set; }

		/// <summary>
		/// Gets the selected document.
		/// </summary>
		/// <value>The selected document.</value>
		public Document SelectedDocument
		{
			get { return SelectedDocuments.FirstOrDefault(); }
		}

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public SortHelper DocumentSort { get; private set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
		public FilterHelper DocumentFilter { get; private set; }

        /// <summary>
        /// Gets or sets the image sort.
        /// </summary>
        /// <value>The sort.</value>
        public SortHelper ImageSort { get; private set; }

        /// <summary>
        /// Gets or sets the image filter.
        /// </summary>
        /// <value>The filter.</value>
        public FilterHelper ImageFilter { get; private set; }

		/// <summary>
		/// Type of view that is currently active.
		/// </summary>
		public DetailsViewType DetailsViewType { get; set; }

		public override bool CanView
		{
			get { return SelectedDocument != null; }
		}

		public override bool CanReply
		{
			get { return true; }
		}

		public override bool CanForward
		{
			get { return true; }
		}

		public override bool CanDelete
		{
			get { return true; }
		}

		public override bool CanMarkRead
		{
			get { return SelectedDocument != null && !SelectedDocument.IsRead; }
		}

		public override bool CanMarkUnread
		{
			get { return SelectedDocument != null && SelectedDocument.IsRead; }
		}
        
		#endregion

		#region Constructors

		public DocumentsState()
		{
			SelectedDocuments = new AdvancedObservableCollection<Document>();

			DocumentsViewSource = new CollectionViewSource { Source = Documents };			

            DocumentSort = new SortHelper(DocumentsViewSource, "Documents");
            DocumentSort.LoadSettings();
			DocumentFilter = new FilterHelper(DocumentsViewSource);

            DocumentsViewSource.View.Filter = DocumentsViewSourceFilter;

			ImagesViewSource = new CollectionViewSource { Source = Documents };

            ImageSort = new SortHelper(ImagesViewSource, "Images");
            ImageSort.LoadSettings();

			ImageFilter = new FilterHelper(ImagesViewSource);

            ImagesViewSource.View.Filter = ImagesViewSourceFilter;

			SelectedDocuments.CollectionChanged += delegate
           	{
           		OnPropertyChanged("SelectedDocument");

           		OnSelectionChanged();
           	};
		}

		#endregion

		#region Methods

		bool DocumentsViewSourceFilter(object sender)
		{
			Document document = (Document)sender;

			bool isVisible = document.DocumentFolder != Folders.Trash
				   && document.ContentType == ContentType.Attachment
				   && document.IsImage == false;

			if (isVisible && DocumentFilter.IsInSearchMode)
				isVisible = DocumentFilter.IsSearchVisible(document);

			return isVisible;
		}

		bool ImagesViewSourceFilter(object sender)
		{
			Document document = (Document)sender;

			bool isVisible = document.DocumentFolder != Folders.Trash
				   && document.ContentType == ContentType.Attachment
				   && document.IsImage;

			if (isVisible && ImageFilter.IsInSearchMode)
				isVisible = ImageFilter.IsSearchVisible(document);

			return isVisible;
		}

        public override void Shutdown()
        {
            DocumentSort.SaveSettings();
            ImageSort.SaveSettings();
        }

		protected override void NewCore()
		{
			
		}

		protected override void ViewCore()
		{
			EventBroker.Publish(AppEvents.View, SelectedDocument);
		}

		protected override void DeleteCore()
		{
			var previousSelection = new List<Document>(SelectedDocuments);

			var documentsCopy = SelectedDocuments.Select(d => d.DuckCopy<Document>()).ToList();

			#region Do action

			Action doAction = delegate
			{
				var documentsView = DetailsViewType == DetailsViewType.Documents ? 
					(IEditableCollectionView)DocumentsViewSource.View :
					(IEditableCollectionView)ImagesViewSource.View;

				foreach (var document in SelectedDocuments.ToList())
				{
					documentsView.EditItem(document);

					document.DocumentFolder = Folders.Trash;

					AsyncUpdateQueue.Enqueue(document);

					documentsView.CommitEdit();
				}
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var document in previousSelection)
				{
					Document document1 = document;
					var oldDocument = documentsCopy.Single(d => d.DocumentId == document1.DocumentId);

					document.DocumentFolder = oldDocument.DocumentFolder;

					AsyncUpdateQueue.Enqueue(document);
				}

				// We cannot use the IEditableObject appraoch here because the document in question
				// probably might not be in view anymore. So instead we will refresh the whole view.
				if (DetailsViewType == DetailsViewType.Documents)
					DocumentsViewSource.View.Refresh();
				else
					ImagesViewSource.View.Refresh();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		protected override void MarkReadCore()
		{
			var previousSelection = new List<Document>(SelectedDocuments);

			var documentsCopy = SelectedDocuments.Select(d => d.DuckCopy<Document>()).ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var document in documentsCopy)
				{
					var documentsView = DetailsViewType == DetailsViewType.Documents ?
						(IEditableCollectionView)DocumentsViewSource.View :
						(IEditableCollectionView)ImagesViewSource.View;

					documentsView.EditItem(document);

					document.MarkRead();

					AsyncUpdateQueue.Enqueue(document);

					documentsView.CommitEdit();
				}
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var document in previousSelection)
				{
					Document document1 = document;
					var oldDocument = documentsCopy.Single(d => d.DocumentId == document1.DocumentId);

					document.IsRead = oldDocument.IsRead;
					document.UpdateProperty("IsRead");

					AsyncUpdateQueue.Enqueue(document);
				}

				if (DetailsViewType == DetailsViewType.Documents)
					DocumentsViewSource.View.Refresh();
				else
					ImagesViewSource.View.Refresh();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		protected override void MarkUnreadCore()
		{
			var previousSelection = new List<Document>(SelectedDocuments);

			var documentsCopy = SelectedDocuments.Select(d => d.DuckCopy<Document>()).ToList();

			#region Do action

			Action doAction = delegate
			{
				foreach (var document in SelectedDocuments.ToList())
				{
					var documentsView = DetailsViewType == DetailsViewType.Documents ?
						(IEditableCollectionView)DocumentsViewSource.View :
						(IEditableCollectionView)ImagesViewSource.View;

					documentsView.EditItem(document);

					document.MarkUnread();

					AsyncUpdateQueue.Enqueue(document);

					documentsView.CommitEdit();
				}
			};

			#endregion

			#region Undo action

			Action undoAction = delegate
			{
				foreach (var document in previousSelection)
				{
					Document document1 = document;
					var oldDocument = documentsCopy.Single(d => d.DocumentId == document1.DocumentId);

					document.IsRead = oldDocument.IsRead;
					document.UpdateProperty("IsRead");

					AsyncUpdateQueue.Enqueue(document);
				}

				if (DetailsViewType == DetailsViewType.Documents)
					DocumentsViewSource.View.Refresh();
				else
					ImagesViewSource.View.Refresh();
			};

			#endregion

			ClientState.Current.UndoManager.Execute(new HistoryAction(doAction, undoAction));
		}

		#endregion
	}
}
