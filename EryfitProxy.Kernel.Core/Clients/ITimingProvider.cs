

using System;
using System.Diagnostics;

namespace EryfitProxy.Kernel.Clients
{
    public interface ITimingProvider
    {
        public static ITimingProvider Default { get; } = new DefaultTimingProvider();

        long InstantMillis { get; }

        DateTime Instant();

        class DefaultTimingProvider : ITimingProvider
        {
            private readonly DateTime _start;
            private readonly Stopwatch _watch = new();

            public DefaultTimingProvider()
            {
                _start = DateTime.UtcNow;
                _watch.Start();
            }

            public DateTime Instant()
            {
                return _start.Add(_watch.Elapsed);
            }

            public long InstantMillis => _watch.ElapsedMilliseconds;
        }
    }
}
