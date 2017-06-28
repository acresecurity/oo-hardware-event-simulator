using System;
using System.Collections.Generic;
using HardwareEvents;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNATimeSchedule : IDNAStatus, IEventGenerator
    {
        public DNATimeSchedule()
        {
            Status = new DNAStatus();
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 223, // Became inactive
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.TS{TimeSchedule}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 224, // Became active
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.TS{TimeSchedule}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description
                    }
                }
            };
        }

        private DNAStatus _status;

        public DNAStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                TriggerPropertyChanged("Status");
            }
        }

        public int PackedAddress
        {
            get
            {
                return Convert.ToInt32(V1.PackedAddress.Encode(DNAHardwareType.TimeSchedule, Site, Controller, TimeSchedule, 0));
            }
            set
            {

            }
        }

        public void SetTimeScheduleMode(DNATimeScheduleControl mode)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginControlTimeSchedule(provider.ApiKey, Site, Controller, TimeSchedule, mode,
                lAsyncResult =>
                {
                    service.EndControlTimeSchedule(lAsyncResult);
                }, null
            );
        }

        private DelegateCommand _timeScheduleModeCommand;

        public DelegateCommand TimeScheduleModeCommand
        {
            get
            {
                if (_timeScheduleModeCommand == null)
                    _timeScheduleModeCommand = new DelegateCommand(
                        p => SetTimeScheduleMode((DNATimeScheduleControl)p),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);
                return _timeScheduleModeCommand;
            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
