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
using System.Windows.Shapes;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Channels.Entities;

namespace Inbox2.Plugins.Notes.Windows
{
    /// <summary>
    /// Interaction logic for NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        public NewItemWindow()
        {
            InitializeComponent();

            Title = "New Note";
        }

        public NewItemWindow(Note source) : this()
        {
            Title = "Edit Note";
            
            NoteEditControl.SourceNote = source;
        }

        void NoteEditControl_NoteSaved(object sender, EventArgs e)
        {
            Close();
        }
    }
}
