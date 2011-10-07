using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Inbox2.Platform.Framework.Interop
{
	/// <summary>
	/// Assists in generating thumbnails for shell items.
	/// </summary>
	public static class ThumbnailGenerator
	{
		public static BitmapSource GenerateThumbnail(String filename)
		{
			OperatingSystem version = Environment.OSVersion;

			// Check if we are running on any other OS except Vista and Win7
			if (version.Platform == PlatformID.Win32Windows || (version.Platform == PlatformID.Win32NT && version.Version.Major < 6))
			{
				BitmapFrame bi = BitmapFrame.Create(new Uri(filename), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
				return bi.Thumbnail;
			}

			IShellItem ppsi = null;
			IntPtr hbitmap = IntPtr.Zero;

			try
			{
				Guid uuid = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");

				// Create an IShellItem from the filename.
				UnsafeNativeMethods.SHCreateItemFromParsingName(Path.GetFullPath(filename), IntPtr.Zero, uuid, out ppsi);

				// Get the thumbnail image.
				((IShellItemImageFactory)ppsi).GetImage(new SIZE(256, 256), 0x0, out hbitmap);

				BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

				// No resize needed
				return source;
			}
			finally
			{
				// Release COM stuff to avoid memory leaks.
				if (ppsi != null)
					Marshal.ReleaseComObject(ppsi);

				if (hbitmap != IntPtr.Zero)
					Marshal.Release(hbitmap);
			}			
		}

		/// <summary>
		/// Generates an Explorer-style thumbnail for any file or shell item. Requires Vista or above.
		/// </summary>
		/// <param name="filename" />The filename of the item.</param>
		/// <returns>The thumbnail of the item.</returns>
		public static BitmapSource GenerateThumbnail(String filename, int desiredHeight)
		{
			var source = GenerateThumbnail(filename);

			MemoryStream ms = new MemoryStream();

			PngBitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(source));
			encoder.Save(ms);

			// perform a resize
			// Do not resize as image is smaller then maxWidth and maxHeight
			if (source.Height <= desiredHeight)
				return source;

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			image.DecodePixelHeight = desiredHeight;
			image.DecodePixelWidth = ((int)source.Width * desiredHeight) / (int)source.Height;
			image.StreamSource = ms;
			image.CreateOptions = BitmapCreateOptions.None;
			image.CacheOption = BitmapCacheOption.Default;
			image.EndInit();

			return image;			
		}
	}
}
