﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Stein.Converters
{
    public class IEnumerableNotNullOrEmptyToBoolConverter
        : IValueConverter
    {
        public static IEnumerableNotNullOrEmptyToBoolConverter Instance = new IEnumerableNotNullOrEmptyToBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable<object>;
            if (enumerable == null)
                return false;
            return enumerable.Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
