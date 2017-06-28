using System;
using System.Collections.Generic;
using HardwareEvents;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAInputPoint : IDNAStatus, IEventGenerator
    {
        public DNAInputPoint()
        {
            Status = new DNAStatus();
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 17, // Monitor Point Inactive
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.I{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "3: Status = 0x00 (was 0x01) <0x01>",
                        CameraID = Camera
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 18, // Monitor Point Active
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.I{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "4: Status = 0x00 (was 0x00) <0x01>",
                        CameraID = Camera
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 33, // Exit delay in progress
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}.I{PointNumber}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "5: Status = 0x04 (was 0x01) <0x05>",
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

        public void Mask(bool arm)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginControlPointMask(provider.ApiKey, Site, Controller, SubController, PointNumber, arm,
                lAsyncResult =>
                {
                    service.EndControlPointMask(lAsyncResult);
                }, null
            );
        }

        private DelegateCommand _maskPointCommand;

        public DelegateCommand MaskPointCommand
        {
            get
            {
                if (_maskPointCommand == null)
                    _maskPointCommand = new DelegateCommand(
                        p => Mask((bool)p),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);
                return _maskPointCommand;
            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; set; }

        #endregion
    }
}
