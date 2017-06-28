using System;
using System.Windows;

namespace HardwareEvents.Converters
{
    public class EventToPersonnelPhotoIcon : BaseEventConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.IsAssignableTo<ICardFlags>() && value.IsAssignableTo<ICardType>() && value.CastTo<ICardType>().IsCardEvent)
            {
                if (value.CastTo<ICardFlags>().Photos)
                    return ToImage("Photo.png");
            }
            return DependencyProperty.UnsetValue;
        }
    }
}