using System;
using System.Windows;
using System.Windows.Controls;

namespace Inbox2.Plugins.Documents.Resources
{
	public class DocumentViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate PreviewTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // if vista or higher
            return Environment.OSVersion.Version.Major >= 6 ? PreviewTemplate : DefaultTemplate;
        }
    }
}
