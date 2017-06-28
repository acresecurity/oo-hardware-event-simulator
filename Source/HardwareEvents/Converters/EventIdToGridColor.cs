using System;
using System.Windows.Media;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Converters
{
    public class EventIdToGridColor : BaseEventConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isBackground = false;
            if (parameter != null)
                isBackground = parameter.ToString() == "Background";

            if (value == null)
                return new SolidColorBrush(isBackground ? Colors.White : Colors.Black);

            var index = System.Convert.ToInt32(value);

            var description = GroupIndex();
            if (!description.ContainsKey(index))
                return new SolidColorBrush(isBackground ? Colors.White : Colors.Black);

            var color = new EventGridColor();
            return isBackground ? color[description[index]].Background : color[description[index]].Foreground;
        }
    }
}
