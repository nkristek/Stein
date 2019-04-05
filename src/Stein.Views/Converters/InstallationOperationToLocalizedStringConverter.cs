using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Stein.Localization;
using Stein.ViewModels.Types;

namespace Stein.Views.Converters
{
    [ValueConversion(typeof(InstallationOperation), typeof(string))]
    internal class InstallationOperationToLocalizedStringConverter
        : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ?? (_instance = new InstallationOperationToLocalizedStringConverter());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is InstallationOperation))
                return Binding.DoNothing;

            switch ((InstallationOperation)value)
            {
                case InstallationOperation.None: return String.Empty;
                case InstallationOperation.Install: return Strings.Installing;
                case InstallationOperation.Uninstall: return Strings.Uninstalling;
                case InstallationOperation.Reinstall: return Strings.Reinstalling;
                default: return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return Binding.DoNothing;

            var stringValue = (string)value;
            if (String.IsNullOrEmpty(stringValue))
                return InstallationOperation.None;
            if (stringValue == Strings.Installing)
                return InstallationOperation.Install;
            if (stringValue == Strings.Reinstalling)
                return InstallationOperation.Reinstall;
            if (stringValue == Strings.Uninstalling)
                return InstallationOperation.Uninstall;

            if (Enum.TryParse(stringValue, out InstallationOperation operationType))
                return operationType;
            return Binding.DoNothing;
        }
    }
}
