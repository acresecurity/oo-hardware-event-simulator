using System;
using System.Collections.Generic;
using HardwareEvents;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAOutputPoint : IDNAStatus, IEventGenerator
    {
        public DNAOutputPoint()
        {
            Status = new DNAStatus();
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 42, // Control Point Activated
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.O{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "4: Status = 0x01 (was 0x00) <0x01>",
                        CameraID = Camera
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 43, // Control Point Deactivated
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.O{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "3: Status = 0x00 (was 0x01) <0x01>",
                        CameraID = Camera
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 29, // Offline or Out of Service
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.O{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "1: Status = 0x80 (was 0x08) <0x88>",
                        CameraID = Camera
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

        private void SetControlPointMode(DNAControlPointMode mode)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginControlPointMode(provider.ApiKey, Site, Controller, SubController, PointNumber, mode,
                lAsyncResult =>
                {
                    service.EndControlPointMode(lAsyncResult);
                }, null
            );
        }

        private DelegateCommand _controlPointModeCommand;

        public DelegateCommand ControlPointModeCommand
        {
            get
            {
                if (_controlPointModeCommand == null)
                    _controlPointModeCommand = new DelegateCommand(
                        p => SetControlPointMode((DNAControlPointMode)p),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);
                return _controlPointModeCommand;
            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; set; }

        #endregion
    }
}
