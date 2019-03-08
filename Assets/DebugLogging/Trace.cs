using System;
using System.Diagnostics;

namespace DebugLogging
{
    public class Trace
    {
        private readonly Stopwatch stopwatch;

        public long PerformanceMS => stopwatch.ElapsedMilliseconds;

        public Trace()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
        }
    }

}
