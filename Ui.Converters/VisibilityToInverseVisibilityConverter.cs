using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace nkristek.Ui.Converters
{
    /// <summary>
    /// Expects <see cref="obVisibilityject"/>.
    /// Returns the opposite, if parameter is set to hidden, it will return <see cref="Visibility.Hidden"/> if encountering <see cref="Visibility.Visible"/>
    /// </summary>
    public class VisibilityToInverseVisibilityConverter 
        : IValueConverter
    {
        public static readonly IValueConverter Instance = new VisibilityToInverseVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility valueAsVisibility = (Visibility)value;

            if (valueAsVisibility == Visibility.Visible)
                return "Hidden".Equals(parameter) ? Visibility.Hidden : Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
