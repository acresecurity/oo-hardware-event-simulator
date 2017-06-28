
namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class DNAController : IDNAStatus
    {
        public DNAController()
        {
            Status = new DNAStatus();
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
    }
}
