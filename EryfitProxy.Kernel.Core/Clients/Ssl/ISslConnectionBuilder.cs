

using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Clients.Ssl
{
    public interface ISslConnectionBuilder
    {
        Task<SslConnection> AuthenticateAsClient(
            Stream innerStream,
            SslConnectionBuilderOptions sslOptions,
            Action<string> onKeyReceived,
            CancellationToken token);
    }

    public class SslConnection : IDisposable
    {
        public SslConnection(Stream stream, SslInfo sslInfo, SslApplicationProtocol applicationProtocol,
            NetworkStream? underlyingBcStream = null, DisposeEventNotifierStream ? eventNotifierStream = null)
        {
            Stream = stream;
            SslInfo = sslInfo;
            ApplicationProtocol = applicationProtocol;
            UnderlyingBcStream = underlyingBcStream;
            EventNotifierStream = eventNotifierStream;
        }

        public Stream Stream { get; }

        public SslInfo SslInfo { get; }

        public SslApplicationProtocol ApplicationProtocol { get; }

        public NetworkStream? UnderlyingBcStream { get; }

        public DisposeEventNotifierStream? EventNotifierStream { get; }

        public string? NssKey { get; set; }

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
