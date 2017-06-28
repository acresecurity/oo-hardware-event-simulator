using System.Collections.Generic;
using HardwareEvents;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAReader : IDNAStatus, IEventGenerator
    {
        public DNAReader()
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

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; set; }

        #endregion
    }
}
