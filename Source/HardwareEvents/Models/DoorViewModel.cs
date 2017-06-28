using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class DoorViewModel : ModelBase, IDNAStatus, IPopulation, IEventGenerator
    {
        public DoorViewModel(DNADoor door, IFlexProvider provider)
        {
            Door = door;
            _provider = provider;

            Populate = RetrievePoints;

            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 10, // Door Cycle in progress
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x00 (was 0x00), AccPt: 0x02 (was 0x00)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 13, // Request to Exit: Door Used
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = "REX: 0",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 71, // Access Granted
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = "Fmt: 0",
                        CardNumber = "3164",
                        CameraID = door.Camera,
                        FirstName = "Goober",
                        LastName = "Munch"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 1, // Door Forced
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x01 (was 0x00), AccPt: 0x04 (was 0x00)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 2, // Door Held
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x01 (was 0x01), AccPt: 0x10 (was 0x00)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 3, // Door Closed
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x00 (was 0x00), AccPt: 0x00 (was 0x02)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 4, // Door Opened
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x01 (was 0x01), AccPt: 0x01 (was 0x10)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 40, // Door Locked
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        TransactionData = $"({door.DoorID}) Door: 0x01 (was 0x01), AccPt: 0x00 (was 0x01)",
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 110, // Reader Mode: Disabled
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 111, // Reader Mode: Unlocked
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 112, // Reader Mode: Locked (Rex Only)
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 113, // Reader Mode: Facility code Only
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 114, // Reader Mode: Card Only
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 115, // Reader Mode: PIN only
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 116, // Reader Mode: Card AND PIN
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 117, // Reader Mode: Card OR PIN
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 190, // Temp Reader Mode: Disabled
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 191, // Temp Reader Mode: Unlocked
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{door.Site}.{door.Controller}.D{door.DoorID}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = door.Description,
                        CameraID = door.Camera,
                    }
                }
            };
        }

        private readonly IFlexProvider _provider;

        public DNADoor Door
        {
            get;
            private set;
        }

        public DNAStatus Status
        {
            get => Door.Status;
            set
            {
                Door.Status = value;
                RaisePropertyChanged();
            }
        }

        public int PackedAddress
        {
            get => Door.PackedAddress;
            set
            {
                Door.PackedAddress = value;
            }
        }

        private ObservableCollection<object> _children;
        public ObservableCollection<object> Children
        {
            get
            {
                if (_children == null)
                    _children = new ObservableCollection<object>{ new DummyLoadingObject() };
                return _children;
            }
        }

        #region Implementation of IEventGenerator

        private IEnumerable<EventSelectionModel> _events;

        public IEnumerable<EventSelectionModel> Events
        {
            get => _events;
            set
            {
                _events = value;
                RaisePropertyChanged();
            }
        }

        public bool IsChildOfDoor { get; } = false;

        #endregion

        private DNAPoints Points
        {
            get;
            set;
        }

        private void RetrievePoints()
        {
            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindDoorPoints(_provider.ApiKey, Door.Site, Door.Controller, Door.DoorID,
                lAsyncResult =>
                {
                    var result = service.EndFindDoorPoints(lAsyncResult);
                    System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (Action)(() =>
                        {
                            Children.Clear();

                            Points = result;

                            // Don't subscribe these points for hardware status. Once they are assigned to a door
                            // you now longer get status from them. Instead we need to show the status based
                            // on the state of the door.
                            Points.InputPoints.ForEach(p =>
                            {
                                Children.Add(p);
                                p.IsChildOfDoor = true;
                            });
                            Points.OutputPoints.ForEach(p =>
                            {
                                Children.Add(p);
                                p.IsChildOfDoor = true;
                            });
                            Points.Readers.ForEach(p =>
                            {
                                Children.Add(p);
                                p.IsChildOfDoor = true;
                            });
                        }));
                }, null);
        }

        private DelegateCommand _traceHistoryByDoor;

        public DelegateCommand TraceHistoryByDoor
        {
            get
            {
                if (_traceHistoryByDoor == null)
                {
                    _traceHistoryByDoor = new DelegateCommand(
                        p =>
                        {
                            var window = new Windows.InfoReadyDoorTraceHistory(Door, _provider);
                            window.Show();
                        },
                        p => true); // TODO Check the function permissions
                }
                return _traceHistoryByDoor;
            }
        }

        private DelegateCommand _whoHasAccessToDoor;

        public DelegateCommand WhoHasAccessToDoor
        {
            get
            {
                if (_whoHasAccessToDoor == null)
                {
                    _whoHasAccessToDoor = new DelegateCommand(
                        p =>
                        {
                            var window = new Windows.InfoReadyWhoHasAccessToDoor(Door, _provider);
                            window.Show();
                        },
                        p => true); // TODO Check the function permissions
                }
                return _whoHasAccessToDoor;
            }
        }

        private DelegateCommand _whoDoesNotHaveAccessToDoor;

        public DelegateCommand WhoDoesNotHaveAccessToDoor
        {
            get
            {
                if (_whoDoesNotHaveAccessToDoor == null)
                {
                    _whoDoesNotHaveAccessToDoor = new DelegateCommand(
                        p =>
                        {
                            var window = new Windows.InfoReadyWhoDoesNotHaveAccessToDoor(Door, _provider);
                            window.Show();
                        },
                        p => true); // TODO Check the function permissions
                }
                return _whoDoesNotHaveAccessToDoor;
            }
        }
        
        #region IPopulation

        public bool NeedsToByPopulated()
        {
            return Children.OfType<DummyNode>().Any();
        }

        public Action Populate
        {
            get;
            set;
        }

        #endregion
    }
}