
namespace System
{
    public static class StringExtensions
    {
        /// <summary>Checks whether a string value is null or empty.</summary>
        /// <returns>true if <paramref name="value" /> is null or is a zero-length string (""); otherwise, false.</returns>
        /// <param name="value">The string value to test.</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
