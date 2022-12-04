using System;
using System.Globalization;
using System.Windows.Data;

namespace Kubmill.Helpers
{
    internal class EnumToBooleanConverter : IValueConverter
    {
        public EnumToBooleanConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceType = value.GetType();
            var enumString = parameter as string;

            if (!sourceType.IsEnum)
            {
                return new();
            }

            Enum.TryParse(sourceType, enumString, out object? result);

            return result?.Equals(value) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(targetType, (string)parameter);
        }
    }
}
