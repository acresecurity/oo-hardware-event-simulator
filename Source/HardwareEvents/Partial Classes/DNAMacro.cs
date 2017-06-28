using System;
using System.Collections.Generic;
using HardwareEvents;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAMacro : IEventGenerator
    {
        public DNAMacro()
        {
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 160, // Cancel (Abort Delay)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 161, // Execute (Type 1)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 162, // Resume, If Paused
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 163, // Execute (Type 2)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 164, // Execute (Type 3)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 165, // Execute (Type 4)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 166, // Resume, If Paused (Type 2)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 167, // Resume, If Paused (Type 3)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 168, // Resume, If Paused (Type 4)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.M{MacroID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                    }
                },
            };
        }

        public void ExecuteMacro(int executeType)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginExecuteMacro(provider.ApiKey, Site, Controller, MacroID, executeType, service.EndExecuteMacro, null);
        }

        private DelegateCommand _executeMacroCommand;

        public DelegateCommand ExecuteMacroCommand
        {
            get
            {
                if (_executeMacroCommand == null)
                    _executeMacroCommand = new DelegateCommand(p =>
                    {
                        if (int.TryParse((string)p, out var executeType))
                            ExecuteMacro(executeType);
                    });
                return _executeMacroCommand;
            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; set; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
