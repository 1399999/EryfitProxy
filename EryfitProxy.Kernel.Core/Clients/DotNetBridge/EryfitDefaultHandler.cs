

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Dns;
using EryfitProxy.Kernel.Clients.Ssl;
using EryfitProxy.Kernel.Clients.Ssl.BouncyCastle;
using EryfitProxy.Kernel.Clients.Ssl.SChannel;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Misc.ResizableBuffers;
using EryfitProxy.Kernel.Writers;

namespace EryfitProxy.Kernel.Clients.DotNetBridge
{
    /// <summary>
    ///  An HttpMessageHandler that uses eryfit internals to send requests.
    ///  Unless you know what you are doing, you should not use this class directly instead of HttpClientHandler.
    /// </summary>
    public class EryfitDefaultHandler : HttpMessageHandler
    {
        private readonly IIdProvider _idProvider;
        private readonly PoolBuilder _poolBuilder;
        private readonly ProxyRuntimeSetting _runtimeSetting;

        private readonly IReadOnlyCollection<IAsyncDisposable>? _disposables;

        private readonly ExchangeScope _exchangeScope = new();

        public EryfitDefaultHandler(
            SslProvider sslProvider,
            ITcpConnectionProvider? connectionProvider = null, 
            RealtimeArchiveWriter? writer = null,
            IReadOnlyCollection<IAsyncDisposable>? disposables = null)
        {
            _disposables = disposables;

            var provider = sslProvider == SslProvider.BouncyCastle
                ? (ISslConnectionBuilder) new BouncyCastleConnectionBuilder()
                : new SChannelConnectionBuilder();
            
            _poolBuilder = new PoolBuilder(
                new RemoteConnectionBuilder(ITimingProvider.Default, provider),
                ITimingProvider.Default, new EventOnlyArchiveWriter(), new DefaultDnsResolver());

            _idProvider = IIdProvider.FromZero;

            _runtimeSetting = ProxyRuntimeSetting.CreateDefault;

            if (connectionProvider != null)
                _runtimeSetting.TcpConnectionProvider = connectionProvider;

            if (writer != null)
                _runtimeSetting.ArchiveWriter = writer;

            _runtimeSetting.Init();
        }

        public List<SslApplicationProtocol>? Protocols { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authority = new Authority(request.RequestUri!.Host, request.RequestUri.Port,
                true);

            var reqHttpString = request.ToHttp11String();

            var exchange = new Exchange(_idProvider, authority, reqHttpString.AsMemory(), null, DateTime.Now);

            if (Protocols != null)
                exchange.Context.SslApplicationProtocols = Protocols;

            var connection = await _poolBuilder.GetPool(exchange, _runtimeSetting, cancellationToken).ConfigureAwait(false);
            
            await connection.Send(exchange, null!, RsBuffer.Allocate(32 * 1024), _exchangeScope,
                cancellationToken).ConfigureAwait(false);

            return new EryfitHttpResponseMessage(exchange);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (_disposables != null) {
                    foreach (var disposable in _disposables)
                        disposable.DisposeAsync(); // uhh
                }
            }

            base.Dispose(disposing);

            _exchangeScope.Dispose();
        }
    }
}
