using System;
using HardwareEvents.DataVirtualization;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents.Windows
{
    /// <summary>
    /// Interaction logic for InfoReadyWhoHasAccessToDoor.xaml
    /// </summary>
    public partial class InfoReadyWhoHasAccessToDoor
    {
        public InfoReadyWhoHasAccessToDoor(IFlexProvider provider)
        {
            InitializeComponent();

            _provider = provider;
        }

        public InfoReadyWhoHasAccessToDoor(DNADoor door, IFlexProvider provider)
            : this(provider)
        {
            _model = new Model(provider) { PackedAddressToStr = door.PackedAddressToStr, Description = door.Description };
            DataContext = _model;

            Title = string.Concat(door.PackedAddressToStr, ": ", door.Description);

            _site = door.Site;
            _controller = door.Controller;
            _door = door.DoorID;

            CreateView(Count, ItemsLoading);
        }

        public InfoReadyWhoHasAccessToDoor(DNAElevator elevator, IFlexProvider provider)
            : this(provider)
        {
            _model = new Model(provider) { PackedAddressToStr = elevator.PackedAddressToStr, Description = elevator.Description };
            DataContext = _model;

            Title = string.Concat(elevator.PackedAddressToStr, ": ", elevator.Description);

            _site = elevator.Site;
            _controller = elevator.Controller;
            _door = elevator.ElevatorID;

            CreateView(Count, ItemsLoading);
        }

        public InfoReadyWhoHasAccessToDoor(string hardwareAddress, string hardwareDescription, int site, int controller, int subController, IFlexProvider provider)
            : this(provider)
        {
            _model = new Model(provider) { PackedAddressToStr = hardwareAddress, Description = hardwareDescription };
            DataContext = _model;

            Title = string.Concat(hardwareAddress, ": ", hardwareDescription);

            _site = site;
            _controller = controller;
            _door = subController;

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
                return service.WhoHasAccessToDoorCount(_provider.ApiKey, _site, _controller, _door);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private WhoHasAccessToDoor[] ItemsLoading(int startIndex, int itemCount)
        {
            try
            {
                var service = new FlexV1_InfoReady { Url = _provider.ServiceUrl };
                return service.WhoHasAccessToDoor(_provider.ApiKey, startIndex, itemCount, _site, _controller, _door);
            }
            catch (Exception)
            {
                return new WhoHasAccessToDoor[0];
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

            public string PackedAddressToStr
            {
                get;
                set;
            }

            public string Description
            {
                get;
                set;
            }
        }
    }
}
