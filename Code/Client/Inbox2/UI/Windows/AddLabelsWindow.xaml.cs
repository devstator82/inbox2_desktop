using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Inbox2.Framework.UI;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.UI.Windows
{
	/// <summary>
	/// Interaction logic for AddLabelsWindow.xaml
	/// </summary>
	public partial class AddLabelsWindow : Window
	{
		private readonly List<Message> _messages;
		private readonly List<Document> _documents;

		public AddLabelsWindow(List<Message> messages)
		{
			_messages = messages;

			InitializeComponent();
		}

		public AddLabelsWindow(List<Document> documents)
		{
			_documents = documents;

			InitializeComponent();
		}

		void Window_Loaded(object sender, RoutedEventArgs e)
		{
            FocusHelper.Focus(LabelsEditor);
		}

		void Button_Click(object sender, RoutedEventArgs e)
		{
			var labels = LabelsEditor.GetVisualLabels();

			// Add selected labels to all messages that don't have one yet
			foreach (var message in _messages)
			{
				foreach (var label in labels)
				{
					LabelsContainer label1 = label;

					if (!message.LabelsList.Any(l => l.Labelname.ToLower() == label1.Labelname.ToLower()))
					{
						message.AddLabel(new Label(label1.Labelname));
					}
				}
			}

			// Add selected labels to all documents that don't have one yet
			//foreach (var document in _documents)
			//{
			//    foreach (var label in labels)
			//    {
			//        LabelsContainer label1 = label;

			//        if (!document.LabelsList.Any(l => l.Labelname.ToLower() == label1.Labelname.ToLower()))
			//            document.AddLabel(label1.Labelname);
			//    }
			//}

			Close();
		}
	}
}
