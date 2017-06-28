using System;
using HardwareEvents;

namespace OpenOptions.dnaFusion.Flex.V1
{
    public partial class WhoHasAccessToDoor : ICardType, ICardFlags
    {
        public string LastNameFirstFullName
        {
            get
            {
                if (UserId == -1)
                    return "New";

                string result;
                if (LastName.IsEmpty() && !FirstName.IsEmpty())
                    result = FirstName;
                else
                    if (!LastName.IsEmpty() && FirstName.IsEmpty())
                    result = LastName;
                else
                    result = $"{LastName}, {FirstName}";
                return result;
            }
        }

        public bool IsCardEvent => !LegacyCardNumber.IsEmpty() && LegacyCardNumber != "0";

        /// <summary>
        /// CARD IS ON THE WATCH PILE!
        /// </summary>
        public bool Watched => BitFields.GetDWordBits((uint)CardFlags, 0x0001) == 1; // 1 bits at offset 0;

        /// <summary>
        /// if this card is used (anywhere), it is an ALARM!
        /// </summary>
        public bool AlarmCard => BitFields.GetDWordBits((uint)CardFlags, 0x0101) == 1; // 1 bit at offset 1;

        /// <summary>
        /// used in conjunction with biAlarmCard!
        /// </summary>
        public int Priority => (int)BitFields.GetDWordBits((uint)CardFlags, 0x0204); // 4 bits at offset 2;

        /// <summary>
        /// this card has a note or reminder...
        /// </summary>
        public bool Note => BitFields.GetDWordBits((uint)CardFlags, 0x0601) == 1; // 1 bit at offset 6;

        /// <summary>
        /// the OTHER flag is set, see info for details
        /// </summary>
        public bool Other => BitFields.GetDWordBits((uint)CardFlags, 0x0701) == 1; // 1 bit at offset 7;

        /// <summary>
        /// this person or this card is a temp!
        /// </summary>
        public bool Temp => BitFields.GetDWordBits((uint)CardFlags, 0x0801) == 1; // 1 bit at offset 8;

        /// <summary>
        /// this card belongs to a contractor
        /// </summary>
        public bool Contract => BitFields.GetDWordBits((uint)CardFlags, 0x0901) == 1; // 1 bit at offset 9;

        /// <summary>
        /// a "visitor"
        /// </summary>
        public bool Visitor => BitFields.GetDWordBits((uint)CardFlags, 0x0A01) == 1; // 1 bit at offset 10;

        /// <summary>
        /// has one or more photos!
        /// </summary>
        public bool Photos => BitFields.GetDWordBits((uint)CardFlags, 0x0B01) == 1; // 1 bit at offset 11;

        /// <summary>
        /// if type is > 31 then stored here
        /// </summary>
        public int CardTypeEx => (int)BitFields.GetDWordBits((uint)CardFlags, 0x0C08); // 8 bits at offset 12;

        /// <summary>
        /// Exempt from download on demand, (always download)!
        /// </summary>
        public bool NoDemand => BitFields.GetDWordBits((uint)CardFlags, 0x1401) == 1; // 1 bit at offset 20;

        public int PerType => (int)BitFields.GetDWordBits((uint)CardFlags, 0x1501); // 5 bits at offset 21;

        public int GetCardType()
        {
            return CardType > 31 ? CardTypeEx : CardType;
        }
    }
}
