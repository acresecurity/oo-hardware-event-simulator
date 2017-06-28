using System.Collections.Generic;
using System.Linq;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Converters
{
    public class BaseEventConverter : BaseAssetImageConverter
    {
        private static Dictionary<int, int> _groupIndex = null;

        private static Dictionary<int, string> _description = null;

        private static DNAEventDescription[] _eventDescriptions = null;

        protected Dictionary<int, int> GroupIndex()
        {
            if (_groupIndex != null)
                return _groupIndex;

            if (_eventDescriptions == null)
            {
                var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();

                var service = new FlexV1_Events {Url = provider.ServiceUrl};
                _eventDescriptions = service.RetrieveEventDescriptions(provider.ApiKey);
            }

            _groupIndex = _eventDescriptions.ToDictionary(p => p.Index, p => p.GroupIndex);
            return _groupIndex;
        }

        protected Dictionary<int, string> Description()
        {
            if (_description != null)
                return _description;

            if (_eventDescriptions == null)
            {
                var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();

                var service = new FlexV1_Events { Url = provider.ServiceUrl };
                _eventDescriptions = service.RetrieveEventDescriptions(provider.ApiKey);
            }

            _description = _eventDescriptions.ToDictionary(p => p.Index, p => p.Description);
            return _description;
        }
    }
}
