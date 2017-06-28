using System.Collections.Generic;
using HardwareEvents;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public interface IEventGenerator
    {
        IEnumerable<EventSelectionModel> Events { get; }

        bool IsChildOfDoor { get; }
    }
}