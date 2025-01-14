# Hardware Tree Status and Event Simulator

**An experimental tool for simulating hardware events**

### Overview ###
Demonstrates how to use the Flex API to duplicate the DNA Hardware Tree and generate simulated events for each hardware item.

![HardwareEvents_Screenshot.jpg](https://bitbucket.org/repo/LorAepK/images/3327504881-HardwareEvents_Screenshot.jpg)

### Configuration ###
You will need to modify the API key and ServiceUrl in the FlexProvider.cs file so that it can connect to your Flex API service.
```
#!c#
    public class FlexProvider : IFlexProvider
    {
        public string ApiKey => "a214ce66-ff77-4aee-8964-406f9817758e";

        public string ServiceUrl => $@"http://{System.Environment.MachineName}/xmlrpc";
    }
```




### Building ###
1. Clone the repo.
2. Open in Visual Studio 2017.
3. Restore all packages.
    * It does use two Nuget packages.
        1. TinyIoC
        2. xmlrpcnet

### Simulated Events ###
Each hardware item has pre-defined events that are unique for that item. Transaction data is also fixed. It provides a way to generate events that you wouldn't normally see. The hardware has to already exist in DNA. Doesn't have to be active or exist, just setup inside of DNA Fusion.

Not all events are currently being simulated and the events that are do not demonstrate the normal day to day operation. Take a door for example, it does not first do an access granted event followed by a door open and then a door closed.

Events can be added by going to the partial class for each hardware item an by adding to the Events collection or creating one if it doesn't already exist.

```
#!c#
            Events = new List<EventSelectionModel>
            {
                new EventSelectionModel
                {
                    EventIndex = 223, // Became inactive
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.TS{TimeSchedule}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description
                    }
                },
                new EventSelectionModel
                {
                    EventIndex = 224, // Became active
                    CreateEvent = p => new DNASendEvent
                    {
                        Address = $"{Site}.{Controller}.TS{TimeSchedule}",
                        EventIndex = p.EventIndex,
                        EventDateTime = DateTime.Now,
                        HardwareDescription = Description
                    }
                }
            };
```



**Disclaimer**
This is "as-is" and should not be used on a production system. All transaction data will be highly inaccurate and most importantly, **not supported!**