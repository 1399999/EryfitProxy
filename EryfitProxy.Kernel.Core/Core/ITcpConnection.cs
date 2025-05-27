

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Core
{
    public interface ITcpConnection : IAsyncDisposable 
    {
        Task<ITcpConnectionConnectResult> ConnectAsync(IPAddress address, int port);
    }

    public interface ITcpConnectionConnectResult
    {
        DisposeEventNotifierStream Stream { get; }

        void ProcessNssKey(string nssKey);
    }

}
