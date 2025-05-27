

using System;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Misc.ResizableBuffers;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Clients.Mock
{
    public class MockedConnectionPool : IHttpConnectionPool
    {
        private readonly PreMadeResponse _preMadeResponse;

        public MockedConnectionPool(Authority authority, PreMadeResponse preMadeResponse)
        {
            Authority = authority;
            _preMadeResponse = preMadeResponse;
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public Authority Authority { get; }

        public bool Complete { get; private set; }

        public void Init()
        {
        }

        public ValueTask<bool> CheckAlive()
        {
            return new ValueTask<bool>(Complete);
        }

        public async ValueTask Send(
            Exchange exchange,
            IDownStreamPipe downStreamPipe, RsBuffer buffer, ExchangeScope __,
            CancellationToken cancellationToken = default)
        {
            exchange.Metrics.RequestHeaderSending = ITimingProvider.Default.Instant();
            exchange.Metrics.TotalSent = 0;

            if (exchange.Request.Body != null) {
                await exchange.Request.Body.DrainAsync().ConfigureAwait(false); // We empty request body stream 
            }

            exchange.Metrics.RequestHeaderSent = ITimingProvider.Default.Instant();

            exchange.Response.Header = new ResponseHeader(
                _preMadeResponse.GetFlatH11Header(Authority, exchange.Context).AsMemory(),
                exchange.Authority.Secure, true);

            exchange.Metrics.ResponseHeaderStart = ITimingProvider.Default.Instant();
            exchange.Metrics.ResponseHeaderEnd = ITimingProvider.Default.Instant();

            var bodyStream = _preMadeResponse.ReadBody(Authority);

            exchange.Response.Body =
                new MetricsStream(_preMadeResponse.ReadBody(Authority),
                    () => { exchange.Metrics.ResponseBodyStart = ITimingProvider.Default.Instant(); },
                    length => {
                        exchange.Metrics.ResponseBodyEnd = ITimingProvider.Default.Instant();
                        exchange.Metrics.TotalReceived += length;
                        exchange.ExchangeCompletionSource.TrySetResult(false);
                    },
                    exception => {
                        exchange.Metrics.ResponseBodyEnd = ITimingProvider.Default.Instant();
                        exchange.ExchangeCompletionSource.SetException(exception);
                    },
                    cancellationToken
                );

            if (bodyStream.CanSeek && bodyStream.Length == 0) {
                exchange.Metrics.ResponseBodyEnd = ITimingProvider.Default.Instant();
                exchange.ExchangeCompletionSource.TrySetResult(false);
            }

            Complete = true;
        }

        public void Dispose()
        {
        }
    }
}
