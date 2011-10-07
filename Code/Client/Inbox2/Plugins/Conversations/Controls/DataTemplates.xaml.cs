using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.VirtualMailBox.Entities;

namespace Inbox2.Plugins.Conversations.Controls
{
	public partial class DataTemplates : ResourceDictionary
	{
		void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		void ListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				
				var document = (e.OriginalSource as DependencyObject)
					.FindListViewItem<Document>(((ListView)sender).ItemContainerGenerator);

				if (document != null)
				{
					EventBroker.Publish(AppEvents.View, document);
				}

				e.Handled = true;
			}
			catch
			{
				// Ignore
			}
		}		
	}
}
