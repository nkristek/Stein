using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Stein.Localizations;

namespace Stein.Views.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    internal class DateTimeToRelativeStringConverter
        : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ?? (_instance = new DateTimeToRelativeStringConverter());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime dateTime))
                return Binding.DoNothing;

            var now = DateTime.Now;
            var timeSpan = now.Subtract(dateTime);

            if (new DateTime(now.Year, now.Month, now.Day) < dateTime && dateTime <= now)
            {
                // is the same day
                var hours = (int) timeSpan.TotalHours;
                var minutes = (int) timeSpan.TotalMinutes;
                var seconds = (int) timeSpan.TotalSeconds;
                
                if (hours == 1)
                    return Strings.OneHourAgo;
                if (hours > 1)
                    return String.Format(Strings.XHoursAgo, hours);

                if (minutes == 1)
                    return Strings.OneMinuteAgo;
                if (minutes > 1)
                    return String.Format(Strings.XMinutesAgo, minutes);

                if (seconds == 1)
                    return Strings.OneSecondAgo;

                return String.Format(Strings.XSecondsAgo, seconds);
            }

            if (new DateTime(now.Year, now.Month, now.Day) < dateTime && dateTime < now.AddDays(-1))
            {
                // is yesterday
                return $"{Strings.Yesterday} {dateTime.ToShortTimeString()}";
            }

            if (parameter is string s && !String.IsNullOrEmpty(s))
                return dateTime.ToString(s);

            return dateTime.ToString(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
