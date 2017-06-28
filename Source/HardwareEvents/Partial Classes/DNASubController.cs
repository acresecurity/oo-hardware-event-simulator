using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CookComputing.XmlRpc;
using HardwareEvents;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNASubController : IDNAStatus, IPopulation, IEventGenerator
    {
        public DNASubController()
        {
            Status = new DNAStatus();
            Populate = RetrievePoints;

            var children = new ObservableCollection<object> { new DummyLoadingObject() };
            Children = children;

            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 141, // Off-Line
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = $"{Description} - 2:Offline, Model: 1 Rev: 3 SN: 4 <2>!"
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 144, // On-Line
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.{SubController}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description,
                        TransactionData = $"{Description} - 3:Online, Model: 87 Rev: 4 SN: 15753 <5>!"
                    }
                }
            }.OrderBy(p => p.EventIndex);
        }

        private DNAPoints Points
        {
            get;
            set;
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

        private ObservableCollection<object> _children;

        public ObservableCollection<object> Children
        {
            get => _children;
            set
            {
                _children = value;
                TriggerPropertyChanged("Children");
            }
        }

        private void RetrievePoints()
        {
            var children = new ObservableCollection<object> { new DummyLoadingObject() };
            Children = children;

            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_Hardware_Async>(provider.ServiceUrl);
            service.BeginFindSubControllerPoints(provider.ApiKey, Site, Controller, SubController,
                lAsyncResult =>
                {
                    try
                    {
                        var result = service.EndFindSubControllerPoints(lAsyncResult);
                        System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (Action)(() =>
                            {
                                Points = result;

                                var points = new List<object>();
                                points.AddRange(Points.InputPoints);
                                points.AddRange(Points.OutputPoints);
                                points.AddRange(Points.Readers);
                                Children = new ObservableCollection<object>(points);
                            }));
                    }
                    catch (XmlRpcFaultException ex)
                    {

                    }
                }, null);
        }

        public bool NeedsToByPopulated()
        {
            return Points == null;
        }

        public Action Populate
        {
            get;
            set;
        }

        #region Implementation of IEventGenerator

        public IEnumerable<EventSelectionModel> Events { get; }

        public bool IsChildOfDoor { get; } = false;

        #endregion
    }
}
