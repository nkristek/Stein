using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Stein.Localization;
using Stein.ViewModels.Types;

namespace Stein.Views.Converters
{
    [ValueConversion(typeof(InstallerOperation), typeof(string))]
    internal class InstallerOperationToLocalizedStringConverter
        : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ?? (_instance = new InstallerOperationToLocalizedStringConverter());

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is InstallerOperation))
                return Binding.DoNothing;

            switch ((InstallerOperation) value)
            {
                case InstallerOperation.DoNothing: return Strings.Nothing;
                case InstallerOperation.Install: return Strings.Install;
                case InstallerOperation.Uninstall: return Strings.Uninstall;
                default: return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return Binding.DoNothing;

            var stringValue = (string) value;
            if (stringValue == Strings.Nothing)
                return InstallerOperation.DoNothing;
            if (stringValue == Strings.Install)
                return InstallerOperation.Install;
            if (stringValue == Strings.Uninstall)
                return InstallerOperation.Uninstall;

            if (Enum.TryParse(stringValue, out InstallerOperation operationType))
                return operationType;
            return Binding.DoNothing;
        }
    }
}
