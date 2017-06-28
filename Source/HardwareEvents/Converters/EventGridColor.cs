using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public class EventGridColor
    {
        private static readonly Dictionary<EventTypes, EventColor> Colors = new Dictionary<EventTypes, EventColor>();

        static EventGridColor()
        {
            PopulateList();
        }

        private static void PopulateList()
        {
            Colors.Add(EventTypes.Door, new EventColor(System.Windows.Media.Colors.Black));
            Colors.Add(EventTypes.Arm, new EventColor(System.Windows.Media.Colors.Purple));
            Colors.Add(EventTypes.Disarm, new EventColor(Color.FromArgb(255, 0, 128, 128)));
            Colors.Add(EventTypes.Secure, new EventColor(Color.FromArgb(255, 56, 116, 175)));
            Colors.Add(EventTypes.Alarms, new EventColor(System.Windows.Media.Colors.Red));
            Colors.Add(EventTypes.Comm, new EventColor(System.Windows.Media.Colors.Green));
            Colors.Add(EventTypes.Areas, new EventColor(Color.FromArgb(255, 56, 116, 175)));
            Colors.Add(EventTypes.Mpg, new EventColor(Color.FromArgb(255, 0, 105, 128)));
            Colors.Add(EventTypes.Time, new EventColor(System.Windows.Media.Colors.Black));
            Colors.Add(EventTypes.Asset, new EventColor(System.Windows.Media.Colors.Black));
            Colors.Add(EventTypes.TransOk, new EventColor(System.Windows.Media.Colors.Blue));
            Colors.Add(EventTypes.Trans, new EventColor(System.Windows.Media.Colors.Red));
            Colors.Add(EventTypes.Mode, new EventColor(Color.FromArgb(255, 166, 202, 240)));
            Colors.Add(EventTypes.User, new EventColor(System.Windows.Media.Colors.Purple));
            Colors.Add(EventTypes.Other, new EventColor(System.Windows.Media.Colors.Black));
        }

        public EventColor this[int Index]
        {
            get
            {
                var value = (EventTypes)Enum.ToObject(typeof(EventTypes), Index);
                return Colors.ContainsKey(value) ? Colors[value] : new EventColor(System.Windows.Media.Colors.Black);
            }
        }

        public EventColor this[EventTypes Index] => Colors.ContainsKey(Index) ? Colors[Index] : new EventColor(System.Windows.Media.Colors.Black);
    }
}
