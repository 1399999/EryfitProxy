

using System.Collections.Generic;

namespace EryfitProxy.Kernel
{
    public class CookieFlow
    {
        public CookieFlow(string name, string host, List<CookieTrackingEvent> events)
        {
            Name = name;
            Host = host;
            Events = events;
        }

        public string Name { get; }

        public string Host { get; }

        public List<CookieTrackingEvent> Events { get; }
    }
}
