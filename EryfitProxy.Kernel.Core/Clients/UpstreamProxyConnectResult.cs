

namespace EryfitProxy.Kernel.Clients
{
    public enum UpstreamProxyConnectResult
    {
        Ok,
        AuthenticationRequired,
        InvalidStatusCode,
        InvalidResponse = 99,
    }
}
