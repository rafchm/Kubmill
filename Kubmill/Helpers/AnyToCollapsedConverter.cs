using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kubmill.Helpers
{
    internal class AnyToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)new AnyToVisibleConverter()
                .Convert(value, targetType, parameter, culture) == Visibility.Visible ?
                Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
