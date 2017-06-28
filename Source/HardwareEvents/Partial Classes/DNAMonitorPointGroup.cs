using System;
using System.Collections.Generic;
using HardwareEvents;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAMonitorPointGroup : IDNAStatus, IEventGenerator
    {
        public DNAMonitorPointGroup()
        {
            Status = new DNAStatus();
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 200, // First Disarm Command Executed
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.MPG{MPGID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "MaskCount =1, Active =2 , ,"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 201, // Subsequent Disarm Command Executed
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.MPG{MPGID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "MaskCount =2, Active =2 , ,"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 202, // Override command: Armed
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.MPG{MPGID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "MaskCount =0, Active =2 , ,"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 203, // Override command: Disarmed
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.MPG{MPGID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "MaskCount =1, Active =2 , ,"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 204, // Force Arm Command
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.MPG{MPGID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = "MaskCount =0, Active =2 , ,"
                    }
                },
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

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
