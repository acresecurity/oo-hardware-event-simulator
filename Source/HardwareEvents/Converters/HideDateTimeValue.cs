using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HardwareEvents.Converters
{
    public class HideDateTimeValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            if (!(value is DateTime))
                return DependencyProperty.UnsetValue;

            var dateTime = (DateTime)value;
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                return DependencyProperty.UnsetValue;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
