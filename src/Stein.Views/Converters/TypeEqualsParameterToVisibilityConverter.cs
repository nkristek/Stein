using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Stein.Views.Converters
{
    /// <summary>
    ///     Expects <see cref="Type" /> or <see cref="object" /> (uses result of <see cref="Object.GetType"/> in that case).
    ///     Returns <see cref="Visibility.Visible" /> if the given type equals the given parameter.
    ///     Returns <see cref="Visibility.Collapsed" /> otherwise.
    /// </summary>
    [ValueConversion(typeof(Type), typeof(Visibility))]
    public class TypeEqualsParameterToVisibilityConverter
#if NET35
        : IValueConverter
#else
        : MarkupExtension, IValueConverter
#endif
    {
        private static IValueConverter _instance;

        /// <summary>
        /// Static instance of this converter.
        /// </summary>
        public static IValueConverter Instance => _instance ?? (_instance = new TypeEqualsParameterToVisibilityConverter());

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Binding.DoNothing;
            if (!(value is Type type))
                type = value.GetType();
            if (!(parameter is Type parameterType))
                parameterType = parameter.GetType();
            return type.Equals(parameterType) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

#if !NET35
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }
#endif
    }
}
