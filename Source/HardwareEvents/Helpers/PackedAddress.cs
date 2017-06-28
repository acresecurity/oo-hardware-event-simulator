using System;
#if !NET20
using System.Linq;
#endif

namespace OpenOptions.dnaFusion.Flex.V1
{
    // ReSharper disable UnusedMember.Global
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo
    public enum DVRType
    {
        DedicatedMicros,	    //0
        GeneralSolutions,	    //1
        GEKalatelDVMRE,		    //2 (not used)
        Pelco,			        //3
        MileStone,			    //4
        Integral,			    //5
        Instek,				    //6
        Intervid,	     	    //7 (Cathexis)
        Salient,			    //8
        ONSSI,				    //9	
        Axis,				    //10
        MarchNetworksSpectiva,	//11 (not used)
        Ionit,					//12
        PelcoEndura,			//13
        MileStoneXPCO,			//14
        ExacqVision,			//15
        Verint,					//16
        Avigilon,				//17
        ThreeXLogic,			//18
        Panasonic,				//19
        Aimetis,				//20
        Genetec,				//21
        LenSecLVMSI,			//22
        OnSSIOcularis,          //23
        VideoInsight,           //24
        IPCamera,               //25
        Bosch,                  //26
    };
    // ReSharper enable IdentifierTypo
    // ReSharper enable InconsistentNaming
    // ReSharper enable UnusedMember.Global

    public class PackedAddress
    {
        public static readonly DNAHardwareType[] NoPointTypes = { 
            DNAHardwareType.TimeSchedule, 
            DNAHardwareType.Macro, 
            DNAHardwareType.Trigger, 
            DNAHardwareType.TriggerVariable, 
            DNAHardwareType.MonitorPointGroup, 
            DNAHardwareType.AccessArea, 
            DNAHardwareType.UserCommand 
        };

        public static readonly DNAHardwareType[] DoorTypes = { 
            DNAHardwareType.Door, 
            DNAHardwareType.Contact, 
            DNAHardwareType.Rex1, 
            DNAHardwareType.Rex2, 
            DNAHardwareType.Strike, 
            DNAHardwareType.Relay 
        };

        /// <summary>
        /// Given a packed address for some piece of hardware this will convert it to the packed address for the associated SSP
        /// </summary>
        /// <param name="packed"></param>
        public static int ToControllerAddress(int packed)
        {
            return packed & 0x7FFE0000 | 1;
        }

        public static bool IsExtended(int packed)
        {
            return ((packed & 0x80000000) >> 31) == 1;
        }

        /// <summary>
        /// Change just the type of a given Packed Address to a particular type
        /// </summary>
        /// <param name="packed"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static long SetType(int packed, DNAHardwareType type)
        {
            return (packed & 0xFFFFFFE0) & (int)type;
        }

        public static long Encode(DNAHardwareType type, int siteNo, int controller, int subcontroller, int point)
        {
#if NET20
            if (Array.Exists(NoPointTypes, p => p == type))
#else
            if (NoPointTypes.Contains(type))
#endif                
               return (siteNo << 25) | (controller << 17) | (subcontroller << 5) | (int)type;
            return (siteNo << 25) | (controller << 17) | (subcontroller) << 10 | (point << 5) | (int)type;
        }

        public static void Decode(int packed, out DNAHardwareType type, out int site, out int controller, out int subcontroller, out int point)
        {
            type = (DNAHardwareType)(packed & 0x1F);
            site = (packed >> 25) & 0x3F;
            controller = (packed >> 17) & 0xFF;
#if NET20
            var hardwareType = type;
            if (Array.Exists(NoPointTypes, p => p == hardwareType))
#else
            if (NoPointTypes.Contains(type))
#endif
            {
                subcontroller = (packed >> 5) & 0xFFF;
                point = 0;
            }
            else
            {
                subcontroller = (packed >> 10) & 0x7F;
                point = (packed >> 5) & 0x1F;
            }
        }

        public static string ToString(int packed)
        {
            Decode(packed, out var type, out var siteNo, out var sspNo, out var sioNo, out var pointNo);

            switch (type)
            {
                case DNAHardwareType.Site:
                    return $"Site {siteNo}";
                case DNAHardwareType.Controller:
                case DNAHardwareType.ControllerTamper:
                case DNAHardwareType.Card:
                    return $"{siteNo}.{sspNo}";
                case DNAHardwareType.SubController:
                case DNAHardwareType.SubControllerCabTamper:
                case DNAHardwareType.SubControllerPowerTamper:
                    return $"{siteNo}.{sspNo}.{sioNo}";
                case DNAHardwareType.MonitorPoint:
                    return $"{siteNo}.{sspNo}.{sioNo}.I{pointNo}";
                case DNAHardwareType.Strike:
                case DNAHardwareType.Relay:
                case DNAHardwareType.ControlPoint:
                    return $"{siteNo}.{sspNo}.{sioNo}.O{pointNo}";
                case DNAHardwareType.Door:
                case DNAHardwareType.Contact:
                case DNAHardwareType.Rex1:
                case DNAHardwareType.Rex2:
#if NET20
                    var doorType = (Array.Exists(DoorTypes, p => p == type) && pointNo == 1) ? "E" : "D";
#else
                    var doorType = (DoorTypes.Contains(type) && pointNo == 1) ? "E" : "D";
#endif
                    return $"{siteNo}.{sspNo}.{doorType}{sioNo}";
                case DNAHardwareType.TimeSchedule:
                    return $"{siteNo}.{sspNo}.TS{sioNo}";
                case DNAHardwareType.Macro:
                    return $"{siteNo}.{sspNo}.M{sioNo}";
                case DNAHardwareType.Trigger:
                    return $"{siteNo}.{sspNo}.T{sioNo}";
                case DNAHardwareType.TriggerVariable:
                    return $"{siteNo}.{sspNo}.TV{sioNo}";
                case DNAHardwareType.MonitorPointGroup:
                    return $"{siteNo}.{sspNo}.MPG{sioNo}";
                case DNAHardwareType.AccessArea:
                    return $"{siteNo}.{sspNo}.A{sioNo}";
                case DNAHardwareType.Reader:
                    return $"{siteNo}.{sspNo}.R{sioNo}";
                case DNAHardwareType.Station:
                    return $"Station {sioNo}";
                default:
                    return "0.0.0.0";
            }
        }

        public static DNAHardwareType ToHardwareType(int packed)
        {
            return (DNAHardwareType)(packed & 0x1F);
        }

        public static long EncodeCameraAddress(int serverIndex, int cameraNumber, DVRType type)
        {
            return 0x80000000 | (serverIndex << 21) | cameraNumber << 10 | ((int)type << 5) | 10;
        }

        public static void DecodeCameraAddress(long cameraAddress, out int serverIndex, out int cameraNumber, out DVRType type)
        {
            serverIndex = (int)(cameraAddress >> 21) & 0x3FF;
            cameraNumber = (int)(cameraAddress >> 10) & 0x7FF;
            type = (DVRType)((cameraAddress >> 5) & 0x1F);
        }

        public static int DecodeCameraNumber(long cameraAddress)
        {
            if (cameraAddress == -1 || cameraAddress == 0)
                return -1;

            DecodeCameraAddress(cameraAddress, out var server, out var camera, out var dvr);

            if (dvr == DVRType.MileStone || dvr == DVRType.MileStoneXPCO || dvr == DVRType.ExacqVision)
                return camera;
            return -1;
        }
    }
}