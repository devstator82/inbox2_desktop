using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Inbox2.Framework.UI.AsyncImage
{
	public interface IAsyncImage
	{
		ImageSource AsyncSource { get; }

		long Accessed { get; }

		void Load();

		void Unload();
	}
}
