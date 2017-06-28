using System;
using System.Collections.Generic;
using HardwareEvents.Interfaces;
using OpenOptions.dnaFusion.Flex.V1;

namespace HardwareEvents
{
    public class InfoReadyBaseModel : ModelBase
    {
        public InfoReadyBaseModel(IFlexProvider provider)
        {
            Provider = provider;
        }

        protected IFlexProvider Provider { get; }

        private string _header;

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                RaisePropertyChanged();
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public event EventHandler OnTrace;

        private DelegateCommand _traceCommand;

        public DelegateCommand TraceCommand
        {
            get
            {
                if (_traceCommand == null)
                {
                    _traceCommand = new DelegateCommand(p =>
                    {
                        var onTrace = OnTrace;
                        onTrace?.Invoke(this, EventArgs.Empty);
                    },
                    p => !IsBusy)
                    .ListenOn(this, p => p.IsBusy);
                }
                return _traceCommand;
            }
        }

        public IEnumerable<TypeRecord> ActivateCardTypes => LoadCardTypes();

        private IEnumerable<TypeRecord> LoadCardTypes()
        {
            var service = new FlexV1 { Url = Provider.ServiceUrl };
            return service.RetrieveCardTypes(Provider.ApiKey);
        }

        public IEnumerable<TypeRecord> DisabledReasons => LoadDisabledReasons();

        private IEnumerable<TypeRecord> LoadDisabledReasons()
        {
            var service = new FlexV1 { Url = Provider.ServiceUrl };
            return service.RetrieveDisabledReasons(Provider.ApiKey);
        }
    }
}
