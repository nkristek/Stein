using System;

namespace Stein.Helpers.Tests
{
    internal static class GCHelper
    {
        public static void TriggerGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
