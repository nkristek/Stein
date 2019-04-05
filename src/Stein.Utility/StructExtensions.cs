﻿namespace Stein.Utility
{
    public static class StructExtensions
    {
        public static bool IsDefault<T>(this T value) where T : struct
        {
            return value.Equals(default(T));
        }
    }
}
