using Inbox2.Framework.Plugins.Entities;
using Inbox2.Platform.Framework.Collections;
using Inbox2.Plugins.Calendar.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Inbox2.Plugins.Calendar.Resources
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is not null
            if (value == null) return String.Empty;

            // Get the message
            DateTime date = (DateTime)value;

            // Return the string
            return String.Format("{0:dd-MM-yyyy}", date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateAndTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is not null
            if (value == null) return String.Empty;

            // Get the message
            DateTime date = (DateTime) value;

            // Return the string
            return String.Format("{0:dd-MM-yyyy} {0:HH:mm}", date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DayLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is not null
            if (value == null) return 1.0;

            // Check if the month of the day is the current month of the navigater
            if ((bool)value) return 1.0;

            // It is not the current month, make the label lighter
            return 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EventDateAndTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is not null
            if (value == null) return String.Empty;

            // Get the message
            Event calendarevent = (Event) value;

            // Check if it is a whole day event
            if (calendarevent.IsWholeDay)
                return String.Format("On {0:d MMMM yyyy} Lasts whole day", calendarevent.StartDate);
            
            // Return the default
            return String.Format("On {0:d MMMM yyyy} From {0:HH:mm} to {1:HH:mm}", calendarevent.StartDate, calendarevent.EndDate);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
