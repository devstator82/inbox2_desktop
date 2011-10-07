using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Interfaces.Plugins;
using Inbox2.Plugins.Documents.Controls;
using Inbox2.Framework.Localization;

namespace Inbox2.Plugins.Documents.Helpers
{
	class ImagesViewHelper : IOverviewPlugin
	{
		private readonly DocumentsState state;

		public ImagesViewHelper(DocumentsState state)
		{
			this.state = state;
		}

		public string Header
		{
			get { return Strings.Images; }
		}

		public ImageSource Icon
		{
			get { return new BitmapImage(new Uri("/Inbox2.UI.Resources;component/icons/images-icon.png", UriKind.Relative)); }
		}

		public WellKnownView WellKnownView
		{
			get { return WellKnownView.Images; }
		}

		public UIElement CreateView()
		{
			return new ImagesView();
		}
	}
}
