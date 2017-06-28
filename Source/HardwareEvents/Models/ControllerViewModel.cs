using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CookComputing.XmlRpc;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class ControllerViewModel : ModelBase, IDNAStatus, IPopulation, IEventGenerator
    {
        public ControllerViewModel(DNAController controller, IFlexProvider provider)
        {
            Controller = controller;
            _provider = provider;

            Status = new DNAStatus();

            Doors = new DoorCollection(this, RetrieveDoors) { new DummyNode() };
            Elevators = new ElevatorCollection(this, RetrieveElevators) { new DummyNode() };
            TimeSchedules = new TimeScheduleCollection(this, RetrieveTimeSchedules) { new DummyNode() };
            MonitorPointGroups = new MonitorPointGroupCollection(this, RetrieveMonitorPointGroups) { new DummyNode() };
            MacroCollection = new MacroCollection(this, RetrieveMacros) { new DummyNode()} ;

            Populate = RetrieveSubControllers;

            RetrieveFloors();

            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 132,
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{controller.Site}.{controller.Controller}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = controller.Description,
                        TransactionData = "No error condition"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 131,
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{controller.Site}.{controller.Controller}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = controller.Description,
                        TransactionData = "Timeout"
                    }
                }
            }.OrderBy(p => p.EventIndex);
        }

        private readonly IFlexProvider _provider;

        public DNAController Controller
        {
            get;
            private set;
        }

        private DNAStatus _status;

        public DNAStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        public int PackedAddress
        {
            get => Controller.PackedAddress;
            set => Controller.PackedAddress = value;
        }

        private DoorCollection _doors;

        public DoorCollection Doors
        {
            get => _doors;
            set
            {
                _doors = value;
                RaisePropertyChanged();
            }
        }

        private ElevatorCollection _elevators;

        public ElevatorCollection Elevators
        {
            get => _elevators;
            set
            {
                _elevators = value;
                RaisePropertyChanged();
            }
        }

        private TimeScheduleCollection _timeSchedules;

        public TimeScheduleCollection TimeSchedules
        {
            get => _timeSchedules;
            set
            {
                _timeSchedules = value;
                RaisePropertyChanged();
            }
        }

        private MonitorPointGroupCollection _monitorPointGroups;

        public MonitorPointGroupCollection MonitorPointGroups
        {
            get => _monitorPointGroups;
            set
            {
                _monitorPointGroups = value;
                RaisePropertyChanged();
            }
        }

        private MacroCollection _macroCollection;

        public MacroCollection MacroCollection
        {
            get => _macroCollection;
            set
            {
                _macroCollection = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DNAFloor> _floors;

        public ObservableCollection<DNAFloor> Floors
        {
            get => _floors;
            set
            {
                _floors = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<object> _children;

        public ObservableCollection<object> Children
        {
            get
            {
                if (_children == null)
                {
                    var result = new List<object> { Doors, Elevators, MonitorPointGroups, TimeSchedules, MacroCollection, new DummyLoadingObject() };
                    _children = new ObservableCollection<object>(result);
                }

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

        private void RetrieveDoors()
        {
            Doors.Clear();
            Doors.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindDoors(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindDoors(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                Doors.Clear();
                                result.ForEach(p => Doors.Add(new DoorViewModel(p, _provider)));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {

                    }
                }, null);
        }

        private void RetrieveElevators()
        {
            Elevators.Clear();
            Elevators.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindElevators(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindElevators(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                Elevators.Clear();
                                result.ForEach(p => Elevators.Add(new ElevatorViewModel(this, p, _provider)));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {

                    }
                }, null);
        }

        private void RetrieveTimeSchedules()
        {
            TimeSchedules.Clear();
            TimeSchedules.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindTimeSchedules(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindTimeSchedules(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                TimeSchedules.Clear();
                                result.ForEach(p => TimeSchedules.Add(p));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {
                    }
                }, null);
        }

        private void RetrieveSubControllers()
        {
            if (Children.Count == 4)
                Children.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindSubControllers(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindSubControllers(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                var dummy = Children.OfType<DummyNode>().FirstOrDefault();
                                if (dummy != null)
                                    Children.Remove(dummy);

                                result.ForEach(p => Children.Add(p));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {
                    }
                }, null);
        }

        private void RetrieveMonitorPointGroups()
        {
            MonitorPointGroups.Clear();
            MonitorPointGroups.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindMonitorPointGroups(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindMonitorPointGroups(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                MonitorPointGroups.Clear();
                                result.ForEach(p => MonitorPointGroups.Add(p));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {
                    }
                }, null);
        }

        // Retrieve macros for this controller.
        private void RetrieveMacros()
        {
            MacroCollection.Clear();
            MacroCollection.Add(new DummyLoadingObject());

            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindMacros(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindMacros(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                MacroCollection.Clear();
                                result.ForEach(p => MacroCollection.Add(p));
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {
                    }
                }, null);
        }

        private void RetrieveFloors()
        {
            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(_provider.ServiceUrl);
            service.BeginFindFloors(_provider.ApiKey, Controller.Site, Controller.Controller,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindFloors(lAsyncResult);
                        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                Floors = new ObservableCollection<DNAFloor>(result);
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {
                    }
                }, null);
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
