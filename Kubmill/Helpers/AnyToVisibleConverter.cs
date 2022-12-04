using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Kubmill.Helpers
{
    internal class AnyToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            else if (value is bool)
            {
                return (bool)value
                    ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (value is string)
            {
                return string.IsNullOrEmpty((string)value)
                    ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (value is IEnumerable<object>)
            {
                return ((IEnumerable<object>)value).Count() == 0
                    ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
