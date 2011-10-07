using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Inbox2.Framework.Interfaces.Enumerations;
using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Interfaces.Enumerations;

namespace Inbox2.Plugins.Notes.Resources
{
    // Note converters

    #region NoteIconConverter

    public class NoteIconConverter : IValueConverter
    {
        static readonly BitmapSource _Fallback;

        static NoteIconConverter()
        {
            _Fallback = new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/icon_forlater.png"));
            _Fallback.Freeze();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var note = (Note)value;

            if (note.ContentType == NoteTypes.Url)
                return new BitmapImage(new Uri("pack://application:,,,/Inbox2.UI.Resources;component/icons/icon_url.png"));

            return _Fallback;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region NoteUrlsToStringConverter

    public class NoteUrlsToStringConverter : IValueConverter
    {
        static Regex _UrlRegex;

        static NoteUrlsToStringConverter()
        {
            _UrlRegex = new Regex(@"(?i:http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> uris = new List<string>();

            foreach (Match m in _UrlRegex.Matches(value.ToString()))
                uris.Add(m.Value);

            return uris;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
