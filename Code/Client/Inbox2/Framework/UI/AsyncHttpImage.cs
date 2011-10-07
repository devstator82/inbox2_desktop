using System;
using System.Collections.Generic;
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

namespace Inbox2.Framework.UI
{
	public class AsyncHttpImage : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		private readonly Func<string> loading;
		private readonly Action loaded;
		private string url;

		/// <summary>
		/// Default image that gets shown when image is still loading.
		/// </summary>
		protected static BitmapImage _HourGlass;

		static AsyncHttpImage()
		{
			_HourGlass = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/HourGlass.png"));
			_HourGlass.Freeze();			
		}
		
		public ImageSource AsyncSource { get; private set; }

		public AsyncHttpImage(Func<string> loading)
		{
			this.loading = loading;

			AsyncSource = _HourGlass;

			AsyncHttpQueue.Enqueue(this);
		}

		public AsyncHttpImage(string url, Action loaded)
		{
			this.url = url;
			this.loaded = loaded;

			AsyncSource = _HourGlass;

			AsyncHttpQueue.Enqueue(this);
		}

		internal void Load()
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

				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                 	{
                 		try
                 		{
							// Create imagesource on the foreground thread
							string extension = Path.GetExtension(url).ToLower();
							BitmapDecoder decoder;

							if (extension == ".gif")
								decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".png")
								decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".jpg")
								decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else if (extension == ".bmp")
								decoder = new BmpBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							else
								throw new ApplicationException(String.Format("Unsupported extension in AsyncHttpImage. Exception = {0}", extension));

							AsyncSource = decoder.Frames[0];
							AsyncSource.Freeze();

							if (PropertyChanged != null)
								PropertyChanged(this, new PropertyChangedEventArgs("AsyncSource"));

							if (loaded != null) loaded();							
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
	}
}