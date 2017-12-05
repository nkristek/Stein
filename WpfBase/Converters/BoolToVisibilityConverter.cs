﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfBase.Converters
{
    public class BoolToVisibilityConverter
        : IValueConverter
    {
        public static BoolToVisibilityConverter Instance = new BoolToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return Visibility.Visible;

            switch (parameter as string)
            {
                case "Hidden": return Visibility.Hidden;
                default: return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && ((Visibility)value == Visibility.Visible || ((Visibility)value == Visibility.Hidden));
        }
    }
}
