using System;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class EventSelectionModel : ModelBase
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        private int _eventIndex;

        public int EventIndex
        {
            get => _eventIndex;
            set
            {
                _eventIndex = value;
                RaisePropertyChanged();
            }
        }

        public Func<EventSelectionModel, DNASendEvent> CreateEvent { get; set; }

        public DNASendEvent CreateSendEvent()
        {
            return CreateEvent?.Invoke(this);
        }
    }
}
