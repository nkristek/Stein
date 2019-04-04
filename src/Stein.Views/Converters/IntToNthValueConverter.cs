using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Stein.Views.Converters
{
    public class IntToNthValueConverter
        : MarkupExtension, IMultiValueConverter
    {
        public static readonly IMultiValueConverter Instance = new IntToNthValueConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = values.FirstOrDefault();
            if (value == null)
                return null;

            var intValue = (int) value;
            if (values.Length <= intValue + 1)
                return null;

            return values[intValue + 1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
