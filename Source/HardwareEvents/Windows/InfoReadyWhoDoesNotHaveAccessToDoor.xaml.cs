using System;
using HardwareEvents.DataVirtualization;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Windows
{
    /// <summary>
    /// Interaction logic for InfoReadyWhoDoesNotHaveAccessToDoor.xaml
    /// </summary>
    public partial class InfoReadyWhoDoesNotHaveAccessToDoor
    {
        public InfoReadyWhoDoesNotHaveAccessToDoor(IFlexProvider provider)
        {
            InitializeComponent();
            _provider = provider;
        }

        public InfoReadyWhoDoesNotHaveAccessToDoor(DNADoor door, IFlexProvider provider)
            : this(provider)
        {
            _model = new Model(provider) { Source = door };
            DataContext = _model;

            Title = string.Concat(door.PackedAddressToStr, ": ", door.Description);

            _site = door.Site;
            _controller = door.Controller;
            _door = door.DoorID;

            CreateView(Count, ItemsLoading);
        }

        public InfoReadyWhoDoesNotHaveAccessToDoor(DNAElevator elevator, IFlexProvider provider)
            : this(provider)
        {
            _model = new Model(provider) { Source = elevator };
            DataContext = _model;

            Title = string.Concat(elevator.PackedAddressToStr, ": ", elevator.Description);

            _site = elevator.Site;
            _controller = elevator.Controller;
            _door = elevator.ElevatorID;

            CreateView(Count, ItemsLoading);
        }

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
                return service.WhoDoesNotHaveAccessToDoorCount(_provider.ApiKey, _site, _controller, _door);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private WhoDoesNotHaveAccessToDoor[] ItemsLoading(int startIndex, int itemCount)
        {
            try
            {
                var service = new FlexV1_InfoReady { Url = _provider.ServiceUrl };
                return service.WhoDoesNotHaveAccessToDoor(_provider.ApiKey, startIndex, itemCount, _site, _controller, _door);
            }
            catch (Exception)
            {
                return new WhoDoesNotHaveAccessToDoor[0];
            }
        }

        private readonly int _site;
        private readonly int _controller;
        private readonly int _door;
        private readonly IFlexProvider _provider;
        private readonly Model _model;

        private class Model : InfoReadyBaseModel
        {
            public Model(IFlexProvider provider)
                : base(provider)
            {

            }

            public object Source { get; set; }
        }
    }
}
