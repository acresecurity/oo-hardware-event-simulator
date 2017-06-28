using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using HardwareEvents.Interfaces;
using HardwareEvents.Windows;
using OpenOptions.dnaFusion.Flex.Common;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class ElevatorViewModel : ModelBase, IPopulation, IEventGenerator
    {
        public ElevatorViewModel(ControllerViewModel parent, DNAElevator elevator, IFlexProvider provider)
        {
            _provider = provider;

            Parent = parent;
            Elevator = elevator;

            Populate = RetrievePoints;
        }

        private readonly IFlexProvider _provider;

        public ControllerViewModel Parent
        {
            get;
            private set;
        }

        public DNAElevator Elevator
        {
            get;
            private set;
        }

        public bool IsGlobalAccessLevelMember
        {
            get
            {
                return Elevator is DNAAccessLevelElevator;
            }
        }

        public string AccessLevelFloorGroup
        {
            get
            {
                var item = Elevator as DNAAccessLevelElevator;
                return item != null ? $" : {item.FloorGroup}" : String.Empty;
            }
        }

        private ObservableCollection<object> _children;

        public ObservableCollection<object> Children
        {
            get
            {
                if (_children == null)
                    _children = new ObservableCollection<object> { new DummyLoadingObject() };
                return _children;
            }
        }

        private DNAPoints Points
        {
            get;
            set;
        }

        private void RetrievePoints()
        {
            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindElevatorPoints(_provider.ApiKey, Elevator.Site, Elevator.Controller, Elevator.ElevatorID,
                lAsyncResult =>
                {
                    var result = service.EndFindElevatorPoints(lAsyncResult);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
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

        private ObservableCollection<GenericSelectionModel<DNAFloor>> _floors;

        public ObservableCollection<GenericSelectionModel<DNAFloor>> Floors
        {
            get
            {
                if (_floors == null)
                {
                    var query = Parent.Floors.OrderBy(p => p.Floor)
                                             .Where(p => p.Floor >= Elevator.StartingFloor && p.Floor <= (Elevator.StartingFloor + Elevator.FloorQuantity));
                    _floors = new ObservableCollection<GenericSelectionModel<DNAFloor>>();
                    foreach (var item in query)
                        Floors.Add(new GenericSelectionModel<DNAFloor>(item));
                }
                return _floors;
            }
        }

        public class DoorModeSelectionItem
        {
            public DNADoorMode Mode { get; set; }
            public string Description { get; set; }
        }

        public static readonly List<DoorModeSelectionItem> DoorModes = CreateDoorModeSelections();

        private static List<DoorModeSelectionItem> CreateDoorModeSelections()
        {
            var result = new List<DoorModeSelectionItem>
            {
                new DoorModeSelectionItem {Mode = DNADoorMode.Disable, Description = "1 - Disabled"},
                new DoorModeSelectionItem {Mode = DNADoorMode.Unlocked, Description = "2 - Unlocked"},
                new DoorModeSelectionItem {Mode = DNADoorMode.Locked, Description = "3 - Locked"},
                new DoorModeSelectionItem
                {
                    Mode = DNADoorMode.FacilityCodeOnly,
                    Description = "4 - Facility Code Only"
                },
                new DoorModeSelectionItem {Mode = DNADoorMode.CardOnly, Description = "5 - Card Only"},
                new DoorModeSelectionItem {Mode = DNADoorMode.PinOnly, Description = "6 - Pin Only"},
                new DoorModeSelectionItem
                {
                    Mode = DNADoorMode.CardAndPinRequired,
                    Description = "7 - Card and Pin Required"
                },
                new DoorModeSelectionItem
                {
                    Mode = DNADoorMode.CardOrPinRequired,
                    Description = "8 - Card or Pin Required"
                }
            };

            return result;
        }

        private DelegateCommand _unlockFloorsCommand;

        public DelegateCommand UnlockFloorsCommand
        {
            get
            {
                if (_unlockFloorsCommand == null)
                {
                    _unlockFloorsCommand = new DelegateCommand(
                        p =>
                        {
                            UnlockFloors();
                        },
                        p => Parent.Floors != null && Parent.Floors.Count > 0)
                        .ListenOn(Parent, p => p.Floors);
                }
                return _unlockFloorsCommand;
            }
        }

        private void UnlockFloors()
        {
            var floors = Floors.Where(p => p.IsSelected).Select(p => p.Item.Floor).ToArray();
            Floors.Where(p => p.IsSelected).ForEach(p => p.IsSelected = false);

            var service = new FlexV1_DNAFusion { Url = _provider.ServiceUrl };
            service.BeginMomentaryUnlockElevatorFloors(_provider.ApiKey, Elevator.Site, Elevator.Controller, Elevator.ElevatorID, floors,
                lAsyncResult =>
                {
                    service.EndMomentaryUnlockElevatorFloors(lAsyncResult);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                    {
                        // need to close the context menu popup
                    }));
                }, null);
        }

        private DelegateCommand _selectAllFloorsCommand;

        public DelegateCommand SelectAllFloorsCommand
        {
            get
            {
                if (_selectAllFloorsCommand == null)
                {
                    _selectAllFloorsCommand = new DelegateCommand(
                        p =>
                        {
                            foreach (var item in Floors)
                                item.IsSelected = true;
                        },
                        p => Parent.Floors != null && Parent.Floors.Count > 0)
                        .ListenOn(Parent, p => p.Floors);
                }
                return _selectAllFloorsCommand;
            }
        }

        private DelegateCommand _unselectAllFloorsCommand;

        public DelegateCommand UnselectAllFloorsCommand
        {
            get
            {
                if (_unselectAllFloorsCommand == null)
                {
                    _unselectAllFloorsCommand = new DelegateCommand(
                        p =>
                        {
                            foreach (var item in Floors)
                                item.IsSelected = false;
                        },
                        p => Parent.Floors != null && Parent.Floors.Count > 0)
                        .ListenOn(Parent, p => p.Floors);
                }
                return _unselectAllFloorsCommand;
            }
        }

        private DelegateCommand _traceHistoryByElevator;

        public DelegateCommand TraceHistoryByElevator
        {
            get
            {
                if (_traceHistoryByElevator == null)
                {
                    _traceHistoryByElevator = new DelegateCommand(
                        p =>
                        {
                            var window = new InfoReadyDoorTraceHistory(Elevator, _provider);
                            window.Show();
                        });
                }
                return _traceHistoryByElevator;
            }
        }

        private DelegateCommand _whoHasAccessToElevator;

        public DelegateCommand WhoHasAccessToElevator
        {
            get
            {
                if (_whoHasAccessToElevator == null)
                {
                    _whoHasAccessToElevator = new DelegateCommand(
                        p =>
                        {
                            var window = new InfoReadyWhoHasAccessToDoor(Elevator, _provider);
                            window.Show();
                        });
                }
                return _whoHasAccessToElevator;
            }
        }

        private DelegateCommand _whoDoesNotHaveAccessToElevator;

        public DelegateCommand WhoDoesNotHaveAccessToElevator
        {
            get
            {
                if (_whoDoesNotHaveAccessToElevator == null)
                {
                    _whoDoesNotHaveAccessToElevator = new DelegateCommand(
                        p =>
                        {
                            var window = new InfoReadyWhoDoesNotHaveAccessToDoor(Elevator, _provider);
                            window.Show();
                        });
                }
                return _whoDoesNotHaveAccessToElevator;
            }
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; set; }

        public bool IsChildOfDoor { get; } = false;

        #endregion

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
