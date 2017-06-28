using System;
using System.Collections.Generic;
using HardwareEvents;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAFloor : IDNAStatus, IEventGenerator
    {
        public DNAFloor()
        {
            Status = new DNAStatus();
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
                return Convert.ToInt32(V1.PackedAddress.Encode(DNAHardwareType.TimeSchedule, Site, Controller, Floor, 0));
            }
            set
            {

            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
