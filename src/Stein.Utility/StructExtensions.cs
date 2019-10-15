namespace Stein.Utility
{
    public static class StructExtensions
    {
        /// <summary>
        /// Determines if the <paramref name="value"/> is equal to its <see langword="default"/> value.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/>.</typeparam>
        /// <param name="value">The value to check to its <see langword="default"/> value.</param>
        /// <returns>If the <paramref name="value"/> is equal to its <see langword="default"/> value.</returns>
        public static bool IsDefault<T>(this T value) 
            where T : struct
        {
            return value.Equals(default(T));
        }
    }
}
