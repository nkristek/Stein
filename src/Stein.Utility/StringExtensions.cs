using System;
using System.Linq;

namespace Stein.Utility
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines if the <paramref name="value"/> contains at least one character contained in <paramref name="chars"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to check if it contains any character contained in <paramref name="chars"/>.</param>
        /// <param name="chars">The characters to check for.</param>
        /// <returns>If the <paramref name="value"/> contains at least one character contained in <paramref name="chars"/>.</returns>
        public static bool Contains(this string? value, params char[] chars)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            if (chars == null)
                return false;
            return value!.Split(chars).Count() > 1;
        }

        /// <summary>
        /// Removes all occurences of characters contained in <paramref name="chars"/>.
        /// </summary>
        /// <param name="value">The string of which the characters should be removed.</param>
        /// <param name="chars">The characters that should be removed.</param>
        /// <returns><paramref name="value"/> without any character contained in <paramref name="chars"/>.</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull("value")]
        public static string? Remove(this string? value, params char[] chars)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            if (chars == null)
                return value;
            return String.Concat(value!.Split(chars));
        }

        /// <summary>
        /// Removes all occurences of characters contained in <paramref name="charsToReplace"/> with the given <paramref name="replacement"/>.
        /// </summary>
        /// <param name="value">The string of which the characters should be replaced.</param>
        /// <param name="replacement">A <see langword="string"/> which will be used as a replacement for each occurence of a character contained in <paramref name="charsToReplace"/>.</param>
        /// <param name="charsToReplace">The characters which should be replaced.</param>
        /// <returns><paramref name="value"/> where all characters contained in <paramref name="charsToReplace"/> are replaced by the given <paramref name="replacement"/>.</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull("value")]
        public static string? Replace(this string? value, string? replacement, params char[] charsToReplace)
        {
            if (String.IsNullOrEmpty(value))
                return value;
            if (charsToReplace == null)
                return value;
            return String.Join(replacement, value!.Split(charsToReplace));
        }
    }
}
