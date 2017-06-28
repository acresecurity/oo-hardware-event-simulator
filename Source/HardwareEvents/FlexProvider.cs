using HardwareEvents.Interfaces;

namespace HardwareEvents
{
    public class FlexProvider : IFlexProvider
    {
        public string ApiKey => "a214ce66-ff77-4aee-8964-406f9817758e";

        public string ServiceUrl => $@"http://{System.Environment.MachineName}/xmlrpc";
    }
}
