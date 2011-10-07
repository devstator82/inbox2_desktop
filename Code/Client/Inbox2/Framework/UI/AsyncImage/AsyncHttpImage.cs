using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Inbox2.Platform.Framework.Extensions;
using Inbox2.Platform.Logging;

namespace Inbox2.Framework.UI.AsyncImage
{
	public class AsyncHttpImage : IAsyncImage, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		private readonly Func<string> loading;
		private readonly Action loaded;
		private readonly Action cleanup;
		private string url;

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

		public AsyncHttpImage(Func<string> loading)
		{
			this.loading = loading;

			_asyncSource = AsyncImageQueue._HourGlass;

			AsyncImageQueue.Enqueue(this);
		}

		public AsyncHttpImage(string url, Action loaded, Action cleanup)
		{
			this.url = url;
			this.loaded = loaded;
			this.cleanup = cleanup;

			_asyncSource = AsyncImageQueue._HourGlass;

			AsyncImageQueue.Enqueue(this);
		}

		public void Load()
		{
			try
			{
				// If we have a loading function, use that to get the url we want to load
				if (loading != null)
					this.url = loading();

				// We explicitly use the WebClient here because we need access to the system-wide browser cache and cookies collection
				var client = new WebClient { CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable) };

				var stream = new MemoryStream();				

				using (var istream = client.OpenRead(url))
				{
					// Read stream into our own byte buffer on the background thread
					istream.CopyTo(stream);
					stream.Seek(0, SeekOrigin.Begin);
				}

				var headers = client.ResponseHeaders;

				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
					{
						try
						{
							// Create imagesource on the foreground thread
							string extension = Path.GetExtension(url).ToLower();
							BitmapDecoder decoder = null;

							if (extension == ".gif")
								decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".png")
								decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".jpg" || extension == ".jpeg")
								decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".bmp")
								decoder = new BmpBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else
							{
								// We are not sure what extension we are looking at, lets see if there is a content-type available
								if (headers["Content-Type"] != null)
								{
									var type = headers["Content-Type"];

									if (type.IndexOf("gif", StringComparison.InvariantCultureIgnoreCase) > -1)
										decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
									else if (type.IndexOf("png", StringComparison.InvariantCultureIgnoreCase) > -1)
										decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
									else if (type.IndexOf("jpg", StringComparison.InvariantCultureIgnoreCase) > -1 || type.IndexOf("jpeg", StringComparison.InvariantCultureIgnoreCase) > -1)
										decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
									else if (type.IndexOf("bmp", StringComparison.InvariantCultureIgnoreCase) > -1)
										decoder = new BmpBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
								}
							}

							if (decoder == null)
							{
								Logger.Error("Unable to determine type of image to decode. url = {0}", LogSource.BackgroundTask, url);
								return;
							}

							_asyncSource = decoder.Frames[0];
							_asyncSource.Freeze();

							if (PropertyChanged != null)
								PropertyChanged(this, new PropertyChangedEventArgs("AsyncSource"));

							if (loaded != null) 
								loaded();							
						}
						catch (Exception ex)
						{
							Logger.Error("An error has occured while trying to download an AsyncHttpImage. Url = {0} Exception = {1}", LogSource.BackgroundTask, url, ex);
						}
						finally
						{
							stream.Dispose();
						}
					});
				
			}
			catch (Exception ex)
			{
				Logger.Error("An error has occured while trying to download an AsyncHttpImage. Url = {0} Exception = {1}", LogSource.BackgroundTask, url, ex);
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