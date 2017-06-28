using System;
using System.Windows;

namespace HardwareEvents.Converters
{
    public class EventDescription : BaseEventConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value is int)
                {
                    var description = Description();
                    if (description == null)
                        return DependencyProperty.UnsetValue;

                    if (description.ContainsKey((int)value))
                        return description[(int)value];
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
