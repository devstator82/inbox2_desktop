using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Inbox2.Framework.Threading.Progress;

namespace Inbox2.UI.Helpers
{
	public class ProgressGroupTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ChannelTemplate { get; set; }

		public DataTemplate SyncTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var group = (ProgressGroup) item;

			return group.SourceChannelId == 0 ? SyncTemplate : ChannelTemplate;
		}
	}
}
