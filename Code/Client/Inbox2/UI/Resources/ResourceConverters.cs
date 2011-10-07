using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Inbox2.Core.Configuration.Channels;
using Inbox2.Framework;
using Inbox2.Framework.Extensions;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Localization;
using Inbox2.Framework.Threading;
using Inbox2.Framework.UI;
using Inbox2.Framework.UI.AsyncImage;
using Inbox2.Framework.VirtualMailBox.Entities;
using Inbox2.Platform.Channels;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Extensions;
using Inbox2.Platform.Channels.Interfaces;
using Inbox2.Platform.Channels.Text;
using Inbox2.Platform.Interfaces.Enumerations;
using Microsoft.Win32;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Label = Inbox2.Framework.VirtualMailBox.Entities.Label;
using Point = System.Windows.Point;

namespace Inbox2.UI.Resources
{
	#region PassThroughMultiValueConverter

	public class MultiValueResult
	{
		public object Object1 { get; set; }

		public object Object2 { get; set; }
	}

	public class PassThroughMultiValueConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return new MultiValueResult { Object1 = values[0], Object2 = values[1] };
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region PassThroughConverter

	public class PassThroughConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Message converters

	#region MessageDateConverter

	public class MessageDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return String.Empty;

			Message message = (Message)value;

			return String.Format("on {0:dd-MM-yy} at {0:HH:mm}", message.DateReceived ?? message.DateSent);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region DateToRelativeTimeConverter

	public class DateToRelativeTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return String.Empty;

			DateTime dt = (DateTime)value;

			return dt.ToRelativeTime();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Channel converters

	#region ChannelIconConverter

	public class ChannelIconConverter : IValueConverter
	{
		public static readonly BitmapSource _Fallback;
		static readonly Dictionary<string, BitmapSource> _SmallIconsCache;
		static readonly Dictionary<string, BitmapSource> _MediumIconsCache;
		static readonly Dictionary<string, BitmapSource> _LargeIconsCache;
		static object synclock = new object();

		static ChannelIconConverter()
		{
			_SmallIconsCache = new Dictionary<string, BitmapSource>();
			_MediumIconsCache = new Dictionary<string, BitmapSource>();
			_LargeIconsCache = new Dictionary<string, BitmapSource>();

			_Fallback = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/newmessage-icon.png"));
			_Fallback.Freeze();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return _Fallback;

			int size = 10;
			var channel = (ChannelConfiguration)value;

			if (parameter != null)
				size = Int32.Parse(parameter.ToString());

			bool contains;

			lock (synclock)
				contains = _SmallIconsCache.ContainsKey(channel.DisplayName);

			if (!contains)
			{
				lock (synclock)
				{
					// Icon not loaded yet
					// Get the assembly this channel is loaded from
					var assembly = channel.GetType().Assembly;
					var resourceNameFormat = assembly.GetName().Name + ".Resources.icon-{0}.png";

					using (var stream = assembly.GetManifestResourceStream(String.Format(resourceNameFormat, 10)))
					{
						if (stream == null) throw new ApplicationException("Channel has no small icon configured");
						_SmallIconsCache.Add(channel.DisplayName, new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0]);
					}

					using (var stream = assembly.GetManifestResourceStream(String.Format(resourceNameFormat, 13)))
					{
						if (stream == null) throw new ApplicationException("Channel has no medium icon configured");
						_MediumIconsCache.Add(channel.DisplayName, new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0]);
					}

					using (var stream = assembly.GetManifestResourceStream(String.Format(resourceNameFormat, 64)))
					{
						if (stream == null) throw new ApplicationException("Channel has no large icon configured");
						_LargeIconsCache.Add(channel.DisplayName, new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad).Frames[0]);
					}
				}
			}

			switch (size)
			{
				case 10: return _SmallIconsCache[channel.DisplayName];
				case 13: return _MediumIconsCache[channel.DisplayName];
				case 64: return _LargeIconsCache[channel.DisplayName];
				default: throw new ArgumentException("Channel icon size not supported: only 10, 13 and 64 are allowed");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region ChannelAvatarConverter

	public class ChannelAvatarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var channel = (ChannelInstance)value;

			if (channel.Configuration.Charasteristics.SupportsStatusUpdates)
			{
				var image = new AsyncHttpImage(delegate
					{
						ChannelContext.Current.ClientContext =
							new ChannelClientContext(ClientState.Current.Context, channel.Configuration);

						var profile = channel.StatusUpdatesChannel.GetProfile();

						return profile.AvatarUrl;
					});

				return image;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region ChannelSourceAddressConverter

	public class ChannelSourceAddressConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return String.Empty;

			if (value is IClientInputChannel)
			{
				IClientInputChannel channel = (IClientInputChannel)value;

				return channel.GetSourceAddress().Address;
			}

			if (value is long)
			{
				IClientInputChannel channel =
					ChannelsManager.GetChannelObject((long)value).InputChannel;

				return channel.GetSourceAddress().Address;
			}

			return String.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region ChannelSourceDisplayNameConverter

	public class ChannelSourceDisplayNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value != null)
			{
				var channel = ChannelsManager.GetChannelObject((long)value);

				return channel.Configuration.DisplayName;
			}

			return String.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Filename converters

	#region FilenameToIconConverter

	public class FilenameToIconConverter : IValueConverter
	{
		private static Dictionary<string, ImageSource> _internalCache;

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		private class Interop
		{
			public const uint SHGFI_ICON = 0x100;
			public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
			public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

			[DllImport("shell32.dll")]
			public static extern IntPtr SHGetFileInfo(string pszPath,
													  uint dwFileAttributes,
													  ref SHFILEINFO psfi,
													  uint cbSizeFileInfo,
													  uint uFlags);
		}


		/// <summary>
		/// Couldn't get it to work with the proper colour-mode. But after modifying our code to use
		/// the version from http://www.bendewey.name/code/FilenameIconImageConverter.html everything
		/// was fixed.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ImageSource source = null;
			lock (this)
			{
				if (_internalCache == null)
					_internalCache = new Dictionary<string, ImageSource>();

				string ext = Path.GetExtension(value == null ? "unknown.unknown" : value.ToString());

				int index = ext.IndexOf('?');

				if (index > -1)
					ext = ext.Substring(0, index);

				if (_internalCache.ContainsKey(ext))
				{
					return _internalCache[ext];
				}
				else
				{
					string resourcePath = Path.Combine(Path.GetTempPath(), "file" + ext);
					FileInfo resource = new FileInfo(resourcePath);
					try
					{
						if (!resource.Exists)
						{
							using (StreamWriter strm = resource.CreateText())
								strm.Close();
						}

						SHFILEINFO shinfo = new SHFILEINFO();

						uint size = parameter == null ? Interop.SHGFI_SMALLICON : Interop.SHGFI_LARGEICON;

						Interop.SHGetFileInfo(resource.FullName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Interop.SHGFI_ICON | size);

						Icon fileIcon = Icon.FromHandle(shinfo.hIcon);
						using (MemoryStream memStream = new MemoryStream())
						{
							Bitmap iconBitmap = fileIcon.ToBitmap();
							iconBitmap.Save(memStream, ImageFormat.Png);
							iconBitmap.Dispose();

							memStream.Seek(0, SeekOrigin.Begin);
							PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(memStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
							source = bitmapDecoder.Frames[0];
							source.Freeze();
						}
					}
					finally
					{
						resource.Delete();
					}

					if (source != null)
						_internalCache.Add(ext, source);
				}
			}

			return source;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region FilenameToDescriptionConverter

	public class FilenameToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "Unknown";

			string filename = value.ToString();
			string ext = Path.GetExtension(filename);

			RegistryKey key = Registry.ClassesRoot.OpenSubKey(ext);

			if (key == null)
				return "Unknown";

			string type = key.GetValue("") as string;

			if (String.IsNullOrEmpty(type))
				return "Unknown";

			key = Registry.ClassesRoot.OpenSubKey(type);

			if (key == null)
				return "Unknown";

			string desc = key.GetValue("") as string;

			return desc;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Object converters

	#region ObjectToObjectHolderConverter

	public class ObjectToObjectHolderConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			Type genericType = typeof(ObjectHolder<>).MakeGenericType(value.GetType());

			return Activator.CreateInstance(genericType, value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region ObjectToEnumerableConverter

	public class ObjectToEnumerableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<object> list = new List<object>();

			if (value == null)
				return list;

			list.Add(value);

			return list;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region MultiObjectSelectConverter

	public class MultiObjectSelectConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (var value in values)
			{
				if (value != null)
					return value;
			}

			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Visibility converters

	#region NotBooleanToVisibilityConverter

	public class NotBooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool oldValue = (bool)value;

			return new BooleanToVisibilityConverter().Convert(!oldValue, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool newValue = (bool)new BooleanToVisibilityConverter().ConvertBack(value, targetType, parameter, culture);

			return !newValue;
		}
	}

	#endregion

	#region BooleanToVisibilityCollapsedConverter

	public class BooleanToVisibilityCollapsedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool oldValue = (bool)value;

			var visib = (Visibility)new BooleanToVisibilityConverter().Convert(oldValue, targetType, parameter, culture);

			return visib == Visibility.Collapsed ? Visibility.Hidden : visib;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool newValue = (bool)new BooleanToVisibilityConverter().ConvertBack(value, targetType, parameter, culture);

			return !newValue;
		}
	}

	#endregion

	#region CountToVisibilityConverter

	public class CountToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Visibility.Collapsed;

			long count = 0;

			if (value is IList)
			{
				IList list = (IList)value;

				count = list.Count;
			}
			else if (value is SourceAddressCollection)
			{
				count = (value as SourceAddressCollection).Count();
			}
			else if (value is IEnumerable)
			{
				IEnumerable enumerable = (IEnumerable) value;

				foreach (var item in enumerable)
					count++;
			}			

			if (value is Int16)
				count = (Int16)value;

			if (value is Int32)
				count = (Int32)value;

			if (value is Int64)
				count = (Int64)value;

			long min = 0;

			if (parameter is string)
			{
				Int64.TryParse((string)parameter, out min);
			}

			return count > min ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region EmptyStringToVisibilityConverter

	public class EmptyStringToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BooleanToVisibilityConverter conv = new BooleanToVisibilityConverter();

			string str = (value as string);

			if (str == null)
				return Visibility.Collapsed;

			return String.IsNullOrEmpty(str.Trim()) ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region NullToVisibilityConverter

	public class NullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Visibility.Collapsed;

			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// String converters

	#region SnipStringConverter

	public class SnipStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null)
				throw new ArgumentNullException("parameter");

			if (value is string)
			{
				string str = (string)value;

				int chars = Int32.Parse(parameter.ToString());

				// Remove redundant spaces
				str = Regex.Replace(str, @"[\s]+", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);

				if (str.Length <= chars)
					return str.Replace("  ", "") + "...";

				return str.Substring(0, chars).Replace("  ", "") + "...";
			}
			else if (value is Stream)
			{
				SnipStringConverter converter = new SnipStringConverter();

				return converter.Convert(
					(value as Stream).ReadString(),
					targetType,
					parameter,
					culture);
			}
			else
			{
				throw new ApplicationException("Unsupported parameter type");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region BytesDisplayConverter

	public class BytesDisplayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Int64 bytes = Int64.Parse(value.ToString());

			Int64 kiloBytes = bytes / 1024;
			Int64 megaBytes = kiloBytes / 1024;

			if (kiloBytes > 1024)
				return String.Format("{0} MB", megaBytes);

			return String.Format("{0} KB", kiloBytes);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region StreamToStringConverter

	public class StreamToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Stream s = (Stream)value;

			return s.ReadString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region HtmlStringToTextElementConverter

	public class HtmlStringToTextElementConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;

			if (String.IsNullOrEmpty(str))
				return new TextBlock();

			var text = new TextBlock { TextWrapping = TextWrapping.Wrap };

			HtmlParser.AppendTo(text, str.MakeLinksClickableIncludingMailto());

			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region Messages

	public class MessageBodyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var message = (Message) value;

			var text = new TextBlock { TextWrapping = TextWrapping.Wrap, Style = (Style)Application.Current.FindResource("TextBlockContentForegroundStyle") };						

			if (String.IsNullOrEmpty(message.BodyPreview))
			{
				text.Inlines.Add(new TextBlock { Text = Strings.MessageHasNoBody, FontStyle = FontStyles.Italic });
			}
			else
			{
				HtmlParser.AppendTo(text, message.BodyPreview.MakeLinksClickableIncludingMailto());
			}

			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region UserStatusBodyConverter

	public class UserStatusBodyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = (UserStatus)value;

			var channel = status.SourceChannel ?? ChannelsManager.Channels.First(c => c.Configuration.DisplayName == "Twitter").Configuration;			
			
			var profileuri = channel.ProfileInfoBuilder.BuildServiceProfileUrl(status.From);
			Uri uri = Uri.IsWellFormedUriString(profileuri, UriKind.Absolute) ? new Uri(profileuri) : new Uri("http://www.google.com");

			var sender = new WebHyperlink { NavigateUri = uri, FontWeight = FontWeights.Bold, Style = (Style)Application.Current.FindResource("TitleNameHyperlink") };
			sender.Inlines.Add(new Run(status.From.DisplayName));
			
			var text = new TextBlock { TextWrapping = TextWrapping.Wrap, Foreground = (Brush)Application.Current.FindResource("SlightlyDimmedForegroundColor") };			

			text.Inlines.Add(sender);
			text.Inlines.Add(" ");

			if (status.To != null)
			{
				var toProfileuri = channel.ProfileInfoBuilder.BuildServiceProfileUrl(status.To);
				Uri toUri = Uri.IsWellFormedUriString(toProfileuri, UriKind.Absolute) ? new Uri(toProfileuri) : new Uri("http://www.google.com");
				
				var recipient = new WebHyperlink { NavigateUri = toUri, FontWeight = FontWeights.Bold, Style = (Style)Application.Current.FindResource("TitleNameHyperlink") };
				recipient.Inlines.Add(new Run(status.To.DisplayName));

				text.Inlines.Add(" » ");
				text.Inlines.Add(recipient);
				text.Inlines.Add(" ");
			}

			var source = status.Status.MakeLinksClickableIncludingMailto();

			if (channel.DisplayName == "Twitter")
				source = source.MakeTwitterLinksClickable();

			HtmlParser.AppendTo(text, source);

			foreach (var inline in text.Inlines)
			{
				// Attach markread handler to status updates
				if (inline is Hyperlink)
					(inline as Hyperlink).Click += delegate { status.TrackAction(ActionType.Read); };
			}

			text.Inlines.Add("\n");
			text.Inlines.Add(new TextBlock { Text = status.SortDate.ToRelativeTime(), Opacity = 0.35, Style = (Style)Application.Current.FindResource("NormalTextBlock") });

			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region UserStatusReplyConverter

	public class UserStatusReplyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var status = (UserStatus)value;

			var channel = status.SourceChannel ?? ChannelsManager.Channels.First(c => c.Configuration.DisplayName == "Twitter").Configuration;

			var profileuri = channel.ProfileInfoBuilder.BuildServiceProfileUrl(status.From);
			Uri uri = Uri.IsWellFormedUriString(profileuri, UriKind.Absolute) ? new Uri(profileuri) : new Uri("http://www.google.com");

			var sender = new WebHyperlink { NavigateUri = uri, FontWeight = FontWeights.Bold, Style = (Style)Application.Current.FindResource("TitleNameHyperlink") };
			sender.Inlines.Add(new Run(status.From.DisplayName));

			var intro = new TextBlock { Text = "Reply to ", Style = (Style)Application.Current.FindResource("NormalTextBlock") };
			var text = new TextBlock { TextTrimming = TextTrimming.CharacterEllipsis, Foreground = (Brush)Application.Current.FindResource("SlightlyDimmedForegroundColor") };

			text.Inlines.Add(intro);
			text.Inlines.Add(" ");
			text.Inlines.Add(sender);
			text.Inlines.Add(" ");
			text.Inlines.Add(status.Status);
			
			return text;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region FromSummaryConverter

	public class FromSummaryConverter : IValueConverter, IEqualityComparer<string>
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var conv = (Conversation) value;
			var sb = new StringBuilder();

			var uniques = conv.Messages
				.Where(m => m.MessageFolder != Folders.SentItems)
				.Select(c => c.From)
				.Distinct(new SourceAddressComparer())
				.Select(GetAppendAddress)
				.Distinct(this)
				.ToList();

			if (conv.Messages.Any(m => m.MessageFolder == Folders.SentItems))
				uniques.Add(Strings.Me);

			for (int i = 0; i < uniques.Count; i++)
			{
				sb.Append(uniques[i]);

				if (i == 0 && uniques.Count > 3)
					sb.Append(" ... ");
				else if (i < uniques.Count - 1)
					sb.Append(", ");
			}

			return sb.ToString();
		}

		string GetAppendAddress(SourceAddress address)
		{
			if (String.IsNullOrEmpty(address.DisplayName))
			{
				return SourceAddress.IsValidEmail(address.Address) ? 
					address.Address.Split('@')[0] : 
					address.ToString();
			}
			else
			{
				return SourceAddress.IsValidEmail(address.DisplayName) ? 
					address.DisplayName.Split('@')[0] : 
					PersonName.Parse(address.DisplayName).Firstname;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public bool Equals(string x, string y)
		{
			return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
		}

		public int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}

	#endregion

	// General converters

	#region ExecutionStatusToBrushConverter

	public class ExecutionStatusToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			BackgroundTask task = (BackgroundTask)value;

			switch (task.ExecutionStatus)
			{
				case ExecutionStatus.Pending:
					{
						if (task.ExecuteAfter > DateTime.MinValue)
						{
							return Application.Current.FindResource("DelayedBrush");
						}

						return Application.Current.FindResource("PendingBrush");
					}
				case ExecutionStatus.Running:
					return Application.Current.FindResource("RunningBrush");
				case ExecutionStatus.Error:
					return Application.Current.FindResource("ErrorBrush");
				case ExecutionStatus.Submitted:
					return Application.Current.FindResource("PendingBrush");
				case ExecutionStatus.Success:
					return Application.Current.FindResource("SuccessBrush");
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion	

	// Bitmap converters

	#region BitmapFormatConverter

	public class BitmapFormatConverter : IValueConverter
	{
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			BitmapSource source = (BitmapSource) value;
			return new FormatConvertedBitmap(source,
			 (System.Windows.Media.PixelFormat)new PixelFormatConverter().ConvertFrom(parameter), null, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	#endregion

	// Document converters

	#region DocumentDescriptionConverter

	public class DocumentDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return String.Empty;

			Document document = (Document)value;

			string description = (string)new FilenameToDescriptionConverter().Convert(document.Filename, targetType, parameter, culture);

			string size = (string)new BytesDisplayConverter().Convert(document.Size, targetType, parameter, culture);

			return String.Format("{0} {1}", description, size);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Label converters

	#region LabelBrushConverter

	public class LabelBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var label = (Label) value;
			int index = label.Labelname.Length == 0 ? 5 : ((label.Labelname[0] % 7) + 4);

			switch (label.LabelType)
			{
				case LabelType.Todo:
					return Application.Current.FindResource("LabelBackgroundBrush1");
				case LabelType.WaitingFor:
					return Application.Current.FindResource("LabelBackgroundBrush2");
				case LabelType.Someday:
					return Application.Current.FindResource("LabelBackgroundBrush3");
				default:
					return Application.Current.FindResource("LabelBackgroundBrush" + index);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region LabelsContainerBrushConverter

	public class LabelsContainerBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var label = (LabelsContainer)value;

			if (label.Labelname == Strings.Todo)
				return Application.Current.FindResource("LabelBackgroundBrush1");
			else if (label.Labelname == Strings.WaitingFor)
				return Application.Current.FindResource("LabelBackgroundBrush2");
			else if (label.Labelname == Strings.Someday)
				return Application.Current.FindResource("LabelBackgroundBrush3");
			else
			{
				int index = label.Labelname.Length == 0 ? 5 : ((label.Labelname[0] % 7) + 4);

				return Application.Current.FindResource("LabelBackgroundBrush" + index);	
			}			
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	// Color converters

	#region BrushToGradientBrushConverter

	public class BrushToGradientBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var color = (Color) value;

			var gradient = new LinearGradientBrush { StartPoint = new Point(0.5, 0), EndPoint = new Point(0.5, 1) };
			gradient.GradientStops.Add(new GradientStop(color, 0.2));
			gradient.GradientStops.Add(new GradientStop { Offset = 1 });

			return gradient;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region ColorToBrushConverter

	public class ColorToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var color = (Color)value;

			return new SolidColorBrush(color);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region WindowStateToMarginConverter

	public class WindowStateToMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			WindowState state = (WindowState) value;
			
			return state == WindowState.Maximized && GlassHelper.IsGlassEnabled() ? new Thickness(5) : new Thickness(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	#region GlassMarginConverter

	public class GlassMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return GlassHelper.IsGlassEnabled() ? new Thickness(0, 32, 0, 0) : new Thickness(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	#endregion

	internal static class GlassHelper
	{
		[DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled", PreserveSig = false)]
		private static extern void _DwmIsCompositionEnabled([Out, MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

		public static bool IsGlassEnabled()
		{
			if (Environment.OSVersion.Version.Major < 6)
				return false;

			bool glassEnabled;

			_DwmIsCompositionEnabled(out glassEnabled);

			return glassEnabled;
		}
	}
}
