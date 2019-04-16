using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Stein.Localization;

namespace Stein.Views.Converters
{
    [ValueConversion(typeof(IEnumerable<long>), typeof(bool))]
    internal class DownloadProgressToStringConverter
        : MarkupExtension, IMultiValueConverter
    {
        private static IMultiValueConverter _instance;

        public static IMultiValueConverter Instance => _instance ?? (_instance = new DownloadProgressToStringConverter());

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => !(v is long)) || values.Length != 2)
                return Binding.DoNothing;
            var bytesDownloaded = (long) values[0];
            var bytesDownloadedString = GetDataUnitString(bytesDownloaded);
            var bytesTotal = (long)values[1];
            var bytesTotalString = GetDataUnitString(bytesTotal);

            return String.Format(Strings.XOfX, bytesDownloadedString, bytesTotalString);
        }

        private static string GetDataUnitString(long bytes)
        {
            const long kbUnit = 1024;
            const long mbUnit = kbUnit * 1024;
            const long gbUnit = mbUnit * 1024;

            if (bytes > gbUnit)
            {
                var gbCount = (double)bytes / gbUnit;
                return String.Format("{0:0.#} GB", gbCount);
            }
            if (bytes > mbUnit)
            {
                var mbCount = (double)bytes / mbUnit;
                return String.Format("{0:0.#} MB", mbCount);
            }
            if (bytes > kbUnit)
            {
                var kbCount = (double)bytes / kbUnit;
                return String.Format("{0:0.#} KB", kbCount);
            }
            return String.Format("{0} B", bytes);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
    }
}
