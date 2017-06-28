using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAElevator : IDNAStatus
    {
        public DNAElevator()
        {
            Status = new DNAStatus();
        }

        public string PackedAddressToStr => V1.PackedAddress.ToString(PackedAddress).Trim();

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

        public void ControlDoorMode(DNADoorMode doorMode)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginControlDoorMode(provider.ApiKey, Site, Controller, ElevatorID, doorMode,
                lAsyncResult =>
                {
                    service.EndControlDoorMode(lAsyncResult);
                }, null);
        }

        private DelegateCommand _controlDoorModeCommand;

        public DelegateCommand ControlDoorModeCommand
        {
            get
            {
                if (_controlDoorModeCommand == null)
                    _controlDoorModeCommand = new DelegateCommand(
                        p => ControlDoorMode((DNADoorMode)p),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);

                return _controlDoorModeCommand;
            }
        }
    }
}