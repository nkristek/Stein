using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Stein.Localizations;
using Stein.ViewModels.Types;

namespace Stein.Views.Converters
{
    [ValueConversion(typeof(InstallationState), typeof(string))]
    internal class InstallationStateToLocalizedStringConverter
        : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ?? (_instance = new InstallationStateToLocalizedStringConverter());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is InstallationState))
                return Binding.DoNothing;

            switch ((InstallationState)value)
            {
                case InstallationState.Preparing: return Strings.Preparing;
                case InstallationState.Install: return Strings.Installing;
                case InstallationState.Reinstall: return Strings.Reinstalling;
                case InstallationState.Uninstall: return Strings.Uninstalling;
                case InstallationState.Cancelled: return Strings.Cancelled;
                default: return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return Binding.DoNothing;

            var stringValue = (string)value;
            if (stringValue == Strings.Preparing)
                return InstallationState.Preparing;
            if (stringValue == Strings.Installing)
                return InstallationState.Install;
            if (stringValue == Strings.Reinstalling)
                return InstallationState.Reinstall;
            if (stringValue == Strings.Uninstalling)
                return InstallationState.Uninstall;
            if (stringValue == Strings.Cancelled)
                return InstallationState.Cancelled;

            if (Enum.TryParse(stringValue, out InstallationState operationType))
                return operationType;
            return Binding.DoNothing;
        }
    }
}
