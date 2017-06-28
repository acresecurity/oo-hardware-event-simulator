
namespace HardwareEvents
{
    public interface ICardType
    {
        bool IsCardEvent
        {
            get;
        }


        int GetCardType();
    }
}
