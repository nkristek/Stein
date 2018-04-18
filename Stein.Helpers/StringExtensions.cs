﻿using System;
using System.IO;
using System.Linq;

namespace Stein.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Tests if the given <see cref="string"/> contains characters which are invalid for paths
        /// </summary>
        /// <param name="path">A path</param>
        /// <returns>True if the given string contains characters which are invalid for paths</returns>
        public static bool ContainsInvalidPathChars(this string path)
        {
            return String.Concat(path.Split(Path.GetInvalidPathChars())).Count() != path.Count();
        }

        /// <summary>
        /// Replaces all characters in the given <see cref="string"/> which are invalid for paths
        /// </summary>
        /// <param name="path">A path</param>
        /// <param name="replacement">Replacement character</param>
        public static string ReplaceInvalidPathChars(this string path, char replacement)
        {
            return Path.GetInvalidPathChars().Aggregate(path, (current, invalidChar) => current.Replace(invalidChar, replacement));
        }

        /// <summary>
        /// Tests if the given <see cref="string"/> contains characters which are invalid for file names
        /// </summary>
        /// <param name="filename">A file name</param>
        /// <returns>True if the given string contains characters which are invalid for file names</returns>
        public static bool ContainsInvalidFileNameChars(this string filename)
        {
            return String.Concat(filename.Split(Path.GetInvalidFileNameChars())).Count() != filename.Count();
        }

        /// <summary>
        /// Replaces all characters in the given <see cref="string"/> which are invalid for file names
        /// </summary>
        /// <param name="filename">A file name</param>
        /// <param name="replacement">Replacement character</param>
        public static string ReplaceInvalidFileNameChars(this string filename, char replacement)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, invalidChar) => current.Replace(invalidChar, replacement));
        }
    }
}
