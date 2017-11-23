﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfBase.Converters
{
    public class DateTimeToStringConverter
        : IValueConverter
    {
        public static DateTimeToStringConverter Instance = new DateTimeToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime)value;
            if (dateTime == DateTime.MinValue)
                return null;
            if (parameter is string && !String.IsNullOrEmpty(parameter as string))
                return dateTime.ToString(parameter as string);
            return dateTime.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
