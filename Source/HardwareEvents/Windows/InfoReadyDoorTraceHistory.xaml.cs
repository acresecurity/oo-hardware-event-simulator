using System;
using HardwareEvents.DataVirtualization;
using HardwareEvents.Interfaces;
using HardwareEvents.Models;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Windows
{
    /// <summary>
    /// Interaction logic for InfoReadyDoorTraceHistory.xaml
    /// </summary>
    public partial class InfoReadyDoorTraceHistory
    {
        public InfoReadyDoorTraceHistory(IFlexProvider provider)
        {
            InitializeComponent();

            Title = "Trace History Dialog";

            _provider = provider;

            _model = new Model(provider)
            {
                StartDate = DateTime.Now.AbsoluteStart(),
                EndDate = DateTime.Now.AbsoluteEnd(),
                AccessOnly = false,
                AccessGranted = true,
                AccessDenied = true,
            };
            _model.OnTrace += (s, e) => CreateView(Count, ItemsLoading);
        }

        public InfoReadyDoorTraceHistory(DNADoor door, IFlexProvider provider)
            : this(provider)
        {
            DataContext = _model;
            _model.Header = string.Concat(PackedAddress.ToString(door.PackedAddress), ": ", door.Description);

            _site = door.Site;
            _controller = door.Controller;
            _door = door.DoorID;
        }

        public InfoReadyDoorTraceHistory(DNAElevator elevator, IFlexProvider provider)
            : this(provider)
        {
            DataContext = _model;
            _model.Header = string.Concat(PackedAddress.ToString(elevator.PackedAddress), ": ", elevator.Description);

            _site = elevator.Site;
            _controller = elevator.Controller;
            _door = elevator.ElevatorID;
        }

        public InfoReadyDoorTraceHistory(string hardwareAddress, string hardwareDescription, int site, int controller, int door, IFlexProvider provider)
            : this(provider)
        {
            DataContext = _model;
            _model.Header = string.Concat(hardwareAddress, ": ", hardwareDescription);

            _site = site;
            _controller = controller;
            _door = door;
        }

        private readonly IFlexProvider _provider;

        private readonly int _site;
        private readonly int _controller;
        private readonly int _door;

        private readonly Model _model;

        protected void CreateView<T>(Func<int> itemCount, Func<int, int, T[]> itemsLoading, int loadSize = 25)
        {
            _model.IsBusy = true;

            var provider = new VirtualCollectionProvider<T>
            {
                VirtualItemCount = itemCount(),
                ItemsLoading = itemsLoading
            };

            var collection = new VirtualizingCollection<T>(provider, loadSize);

            InfoGridView.ItemsSource = collection;

            _model.Count = provider.VirtualItemCount;

            _model.IsBusy = false;
        }

        private int Count()
        {
            try
            {
                var service = new FlexV1_InfoReady { Url = _provider.ServiceUrl };
                return service.DoorTraceHistoryCount(_provider.ApiKey, _site, _controller, _door, _model.StartDate, _model.EndDate, _model.AccessOnly, _model.AccessGranted, _model.AccessDenied);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private DNATraceHistory[] ItemsLoading(int startIndex, int itemCount)
        {
            try
            {
                var service = new FlexV1_InfoReady { Url = _provider.ServiceUrl };
                return service.DoorTraceHistory(_provider.ApiKey, startIndex, itemCount, _site, _controller, _door, _model.StartDate, _model.EndDate, _model.AccessOnly, _model.AccessGranted, _model.AccessDenied);
            }
            catch (Exception)
            {
                return new DNATraceHistory[0];
            }
        }

        public class Model : InfoReadyTraceHistoryBaseModel
        {
            public Model(IFlexProvider provider) 
                : base(provider)
            {
            }

            public bool AccessOnly
            {
                get;
                set;
            }
        }
    }
}
