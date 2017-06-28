
namespace HardwareEvents
{
    public interface ICardFlags
    {
        int CardFlags
        {
            get;
        }

        bool Watched
        {
            get;
        }

        bool AlarmCard
        {
            get;
        }

        int Priority
        {
            get;
        }

        bool Note
        {
            get;
        }

        bool Other
        {
            get;
        }

        bool Temp
        {
            get;
        }

        bool Contract
        {
            get;
        }

        bool Visitor
        {
            get;
        }

        bool Photos
        {
            get;
        }

        int CardTypeEx
        {
            get;
        }

        bool NoDemand
        {
            get;
        }

        int PerType
        {
            get;
        }

        int CardType
        {
            get;
        }
    }
}
