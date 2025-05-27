

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients;
using EryfitProxy.Kernel.Clients.H11;
using EryfitProxy.Kernel.Misc.ResizableBuffers;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Core
{
    /// <summary>
    /// An exchange source provider is responsible for reading exchanges from a stream.
    /// </summary>
    internal abstract class ExchangeSourceProvider
    {
        private readonly IIdProvider _idProvider;

        protected ExchangeSourceProvider(IIdProvider idProvider)
        {
            _idProvider = idProvider;
        }

        /// <summary>
        /// Called to init a first connection 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="localEndpoint"></param>
        /// <param name="token"></param>
        /// <param name="remoteEndPoint"></param>
        /// <returns></returns>
        public abstract ValueTask<ExchangeSourceInitResult?> InitClientConnection(
            Stream stream, RsBuffer buffer, IPEndPoint localEndpoint, IPEndPoint remoteEndPoint,
            CancellationToken token);

    }
}
