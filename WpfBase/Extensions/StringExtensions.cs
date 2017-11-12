using System;
using System.IO;
using System.Linq;

namespace WpfBase.Extensions
{
    public static class StringExtensions
    {
        public static string Quoted(this string value)
        {
            return String.Join(String.Empty, "\"", value, "\"");
        }
        
        public static bool ContainsInvalidPathChars(this string path)
        {
            return String.Join(String.Empty, path.Split(Path.GetInvalidPathChars())).Count() != path.Count();
        }
    }
}
