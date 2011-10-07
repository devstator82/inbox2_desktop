using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.UI.AsyncImage
{
	public class AsyncAvatar : IAsyncImage, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly string filename;
		private readonly Action loaded;
		private readonly Action cleanup;

		private ImageSource _asyncSource;
		private long _accessed;

		public ImageSource AsyncSource
		{
			get
			{
				_accessed = DateTime.Now.Ticks;

				return _asyncSource;
			}
		}

		public long Accessed
		{
			get { return _accessed; }
		}

		public AsyncAvatar(string filename, Action loaded, Action cleanup)
		{
			this.filename = filename;
			this.loaded = loaded;
			this.cleanup = cleanup;

			_asyncSource = AsyncImageQueue._Profile;

			AsyncImageQueue.Enqueue(this);
		}

		public void Load()
		{
			try
			{
				_asyncSource = new BitmapImage(new Uri(filename));
				_asyncSource.Freeze();

				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
				{
					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("AsyncSource"));

					if (loaded != null)
						loaded();
				});				
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to create an AsyncThumbnailImage. Filename = {0} Exception = {1}", LogSource.BackgroundTask, filename, ex);
			}
		}

		public void Unload()
		{
			_asyncSource = null;

			if (cleanup != null)
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, cleanup);
		}
	}
}
