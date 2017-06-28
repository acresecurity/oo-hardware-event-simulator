using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Converters
{
    public class EventToPersonnelType : BaseEventConverter
    {
        private static Dictionary<int, string> _personnelTypes = null;

        protected Dictionary<int, string> PersonnelTypes()
        {
            if (_personnelTypes != null)
                return _personnelTypes;

            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();

            var service = new FlexV1 { Url = provider.ServiceUrl };
            var personnel = service.RetrievePersonnelTypes(provider.ApiKey);

            _personnelTypes = personnel.ToDictionary(p => p.ID, p => p.Description);

            return _personnelTypes;
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.IsAssignableTo<ICardFlags>() && value.IsAssignableTo<ICardType>() && value.CastTo<ICardType>().IsCardEvent)
            {
                var index = value.CastTo<ICardFlags>().PerType;

                string result;
                if (PersonnelTypes().TryGetValue(index, out result))
                    return result;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
