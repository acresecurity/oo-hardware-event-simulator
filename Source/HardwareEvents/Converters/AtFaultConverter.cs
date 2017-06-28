using System;
using System.Windows.Data;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Converters
{
    public class AtFaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DNAFault)
                return (DNAFault)value != DNAFault.Inactive;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
