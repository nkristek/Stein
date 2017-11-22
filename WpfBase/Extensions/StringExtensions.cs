using System;
using System.IO;
using System.Linq;

namespace WpfBase.Extensions
{
    public static class StringExtensions
    {
        public static string Quoted(this string value)
        {
            return String.Concat("\"", value, "\"");
        }
        
        public static bool ContainsInvalidPathChars(this string path)
        {
            return String.Concat(path.Split(Path.GetInvalidPathChars())).Count() != path.Count();
        }

        public static void ReplaceInvalidPathChars(this string path, char replacement)
        {
            foreach (var invalidChar in Path.GetInvalidPathChars())
                path.Replace(invalidChar, replacement);
        }

        public static bool ContainsInvalidFileNameChars(this string fileName)
        {
            return String.Concat(fileName.Split(Path.GetInvalidFileNameChars())).Count() != fileName.Count();
        }

        public static void ReplaceInvalidFileNameChars(this string fileName, char replacement)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                fileName.Replace(invalidChar, replacement);
        }
    }
}
