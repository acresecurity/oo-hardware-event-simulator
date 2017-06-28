using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class BaseCollection : ObservableCollection<object>, IPopulation, IEventGenerator
    {
        public BaseCollection(object parent, Action populate)
        {
            Parent = parent;
            Populate = populate;
        }

        public object Parent
        {
            get;
            private set;
        }

        public Action Populate
        {
            get;
            set;
        }

        public bool NeedsToByPopulated()
        {
            return Items.OfType<DummyNode>().Any();
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
