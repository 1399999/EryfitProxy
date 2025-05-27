

using System;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Misc.ResizableBuffers;

namespace EryfitProxy.Kernel.Clients
{
    /// <summary>
    ///     Represents a connection pool to the same authority, using the same .
    /// </summary>
    public interface IHttpConnectionPool : IAsyncDisposable
    {
        Authority Authority { get; }

        bool Complete { get; }

        void Init();

        ValueTask<bool> CheckAlive();

        ValueTask Send(Exchange exchange, IDownStreamPipe downstreamPipe, RsBuffer buffer, ExchangeScope exchangeScope, CancellationToken cancellationToken = default);
    }
}
