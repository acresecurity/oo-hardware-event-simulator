using System;

namespace HardwareEvents
{
    public interface IPopulation
    {
        bool NeedsToByPopulated();

        Action Populate
        {
            get;
            set;
        }
    }
}
