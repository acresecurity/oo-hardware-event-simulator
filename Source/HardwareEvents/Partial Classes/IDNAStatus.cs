
namespace OpenOptions.dnaFusion.Flex.V1
{
    public interface IDNAStatus
    {
        DNAStatus Status
        {
            get;
            set;
        }

        int PackedAddress
        {
            get;
            set;
        }
    }
}
