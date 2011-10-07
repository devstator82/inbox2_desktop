using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Framework.Localization;
using Inbox2.Plugins.Documents.Controls;

namespace Inbox2.Plugins.Documents.Helpers
{
	class DocumentsViewHelper : IOverviewPlugin
	{
		private readonly DocumentsState state;

		public DocumentsViewHelper(DocumentsState state)
		{
			this.state = state;
		}

		public string Header
		{
			get { return Strings.Documents; }
		}

		public ImageSource Icon
		{
			get { return new BitmapImage(new Uri("/Inbox2.UI.Resources;component/icons/docs-icon.png", UriKind.Relative)); }
		}

		public WellKnownView WellKnownView
		{
			get { return WellKnownView.Documents; }
		}

		public UIElement CreateView()
		{
			return new DocumentsView();
		}
	}
}
