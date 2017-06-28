
namespace System
{
    public static class BitFields
    {
        public static uint GetDWordBits(uint bits, int index)
        {
            var result = (bits >> (index >> 8)) // Offset
                         & ((1 << (byte)index) - 1);  // Mask
            return (uint)result;
        }
    }
}
