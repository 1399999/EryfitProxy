

using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Clients.Headers
{
    public abstract class HeaderAlteration
    {
        public abstract void Apply(Header header);
    }
}
