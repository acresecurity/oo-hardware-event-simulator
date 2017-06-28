using System;
using HardwareEvents.Interfaces;

namespace HardwareEvents.Models
{
    public class InfoReadyTraceHistoryBaseModel : InfoReadyBaseModel
    {
        public InfoReadyTraceHistoryBaseModel(IFlexProvider provider)
            : base(provider)
        {
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public bool AccessGranted
        {
            get;
            set;
        }

        public bool AccessDenied
        {
            get;
            set;
        }
    }
}
