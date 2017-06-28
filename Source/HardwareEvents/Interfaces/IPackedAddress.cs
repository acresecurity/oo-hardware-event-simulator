
namespace HardwareEvents
{
    public interface IPackedAddress
    {
        int SourceSystem
        {
            get;
        }

        long PackedAddress
        {
            get;
        }
    }
}
