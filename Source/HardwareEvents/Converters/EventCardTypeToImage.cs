using System;
using System.Windows;

namespace HardwareEvents.Converters
{
    public class EventCardTypeToImage : BaseEventConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as ICardType;
            if (item != null && item.IsCardEvent)
            {
                switch (item.GetCardType())
                {
                    case 0: // Normal
                        return ToImage("Card1.png");
                    case 1: // Visitor
                        return ToImage("Visitor.png");
                    case 2: // Temporary
                        return ToImage("Temp.png");
                    case 3: // Disabled
                        return ToImage("Card2.png");
                    case 4: // Contractor
                        return ToImage("Contract.png");
                    case 5: // Vendor
                        return ToImage("Vendor.png");
                    case 6: // Custom 1
                        return ToImage("Custom1.png");
                    case 7: // Custom 2
                        return ToImage("Custom2.png");
                    case 8: // Custom 3
                        return ToImage("Custom3.png");
                    case 9: // Custom 4
                        return ToImage("Custom4.png");
                    case 10: // Custom 5
                        return ToImage("Custom5.png");
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
