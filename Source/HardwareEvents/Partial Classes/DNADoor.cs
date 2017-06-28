using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.Common;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNADoor : IDNAStatus
    {
        public DNADoor()
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
            service.BeginControlDoorMode(provider.ApiKey, Site, Controller, DoorNumber, doorMode,
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

        public void MomentaryUnlock()
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginMomentaryUnlockDoor(provider.ApiKey, Site, Controller, DoorNumber,
                lAsyncResult =>
                {
                    service.EndMomentaryUnlockDoor(lAsyncResult);
                }, null);
        }

        private DelegateCommand _momentaryUnlockCommand;

        public DelegateCommand MomentaryUnlockCommand
        {
            get
            {
                if (_momentaryUnlockCommand == null)
                    _momentaryUnlockCommand = new DelegateCommand(
                        p => MomentaryUnlock(),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);

                return _momentaryUnlockCommand;
            }
        }

        public void DoorMask(DNADoorAlarm doorAlarm, bool arm)
        {
            var provider = TinyIoC.TinyIoCContainer.Current.Resolve<IFlexProvider>();
            var service = XmlRpcProxy.Create<IFlexV1_DNAFusion_Async>(provider.ServiceUrl);
            service.BeginControlDoorMask(provider.ApiKey, Site, Controller, DoorID, doorAlarm, arm,
                lAsyncResult =>
                {
                    service.EndControlDoorMode(lAsyncResult);
                }, null
            );
        }

        private DelegateCommand _doorForcedMaskCommand;

        public DelegateCommand DoorForcedMaskCommand
        {
            get
            {
                if (_doorForcedMaskCommand == null)
                    _doorForcedMaskCommand = new DelegateCommand(
                        p => DoorMask(DNADoorAlarm.Forced, !Status.ForcedMasked),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);
                return _doorForcedMaskCommand;
            }
        }

        private DelegateCommand _doorForcedHeldCommand;

        public DelegateCommand DoorHeldMaskCommand
        {
            get
            {
                if (_doorForcedHeldCommand == null)
                    _doorForcedHeldCommand = new DelegateCommand(
                        p => DoorMask(DNADoorAlarm.Held, !Status.ForcedMasked),
                        p => Status.Online)
                        .ListenOn(this, p => p.Status);
                return _doorForcedHeldCommand;
            }
        }
    }
}
