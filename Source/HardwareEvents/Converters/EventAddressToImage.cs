using System;
using System.Windows;

namespace HardwareEvents.Converters
{
    public class EventAddressToImage : BaseEventConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as IPackedAddress;
            if (item != null && item.SourceSystem == 0) // Mercury hardware
            {
                switch (item.PackedAddress & 0x1F)
                {
                    case 0:
                        return ToImage("SSP.png");
                    case 1:
                        return ToImage("SSPE.png");
                    case 2:
                        return ToImage("SSPC.png");
                    case 4:
                        return ToImage("RSC1.png");
                    case 5:
                        return ToImage("RSC2.png");
                    case 6:
                        return ToImage("OSC16.png");
                    case 7:
                        return ToImage("MP.png");
                    case 8:
                        return ToImage("CP.png");
                    case 9:
                        return ToImage("Door1.png");
                    case 10:
                        return ToImage("Door2.png");
                    case 15:
                        return ToImage("TS.png");
                    case 16:
                        return ToImage("Macro.png");
                    case 17:
                        return ToImage("Trigger.png");
                    case 18:
                        return ToImage("MacroCommand.png");
                    case 19:
                        return ToImage("MPG.png");
                    case 20:
                        return ToImage("Area.png");
                    case 26:
                        return ToImage("Alarms.png");
                    case 29:
                        return ToImage("Camera3.png");
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
