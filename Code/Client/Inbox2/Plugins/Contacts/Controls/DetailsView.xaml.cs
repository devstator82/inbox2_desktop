using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Inbox2.Framework;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.VirtualMailBox;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Plugins.Contacts.Helpers;

namespace Inbox2.Plugins.Contacts.Controls
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : UserControl, INotifyPropertyChanged, IPersistableTab
    {
        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs> RequestCloseTab;        

    	private readonly VirtualMailBox mailbox;

        #endregion 

        #region Properties

		public Person SelectedPerson { get; private set; }

    	public string Title
        {
            get
            {
                if (SelectedPerson == null)
                    return String.Empty;

                return SelectedPerson.Firstname + " " + SelectedPerson.Lastname;
            }
        }

    	public Control CustomHeaderContent
    	{
    		get { return null; }
    	}

    	public PluginPackage Plugin
        {
            get { return PluginsManager.Current.GetPlugin<ContactsPlugin>(); }
        }

        public ViewType ViewType
        {
            get { return ViewType.DetailsView; }
        }

        public ContactsState State
        {
            get { return (ContactsState)Plugin.State; }
        }

        #endregion

        #region Construction

        public DetailsView()
        {
			mailbox = VirtualMailBox.Current;

            InitializeComponent();

			DataContext = this;
        }

        #endregion

        #region Methods

		public bool CanCloseTab()
		{
			return true;
		}

        public void LoadData(object data)
        {
        	var d = ((ViewContactDataHelper) data);

			if (d.PersonId != 0)
			{
				using (mailbox.ReaderLock)
					SelectedPerson = mailbox.Persons.First(p => p.PersonId == d.PersonId);

				var conversationsViewSource = new CollectionViewSource { Source = SelectedPerson.Conversations };
				conversationsViewSource.SortDescriptions.Add(new SortDescription("SortDate", ListSortDirection.Descending));

				var documentsViewSource = new CollectionViewSource { Source = SelectedPerson.Documents };
				documentsViewSource.SortDescriptions.Add(new SortDescription("Filename", ListSortDirection.Ascending));

				ConversationsColumn.OverrideViewSource(conversationsViewSource);
				DocumentsColumn.OverrideViewSource(documentsViewSource);
			}

			OnPropertyChanged("Title");
			OnPropertyChanged("SelectedPerson");
        }

        public object SaveData()
        {
            return new ViewContactDataHelper { PersonId = SelectedPerson.PersonId.Value };
        }

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

        #endregion

        #region Event Handlers
        
        #endregion
    }
}
