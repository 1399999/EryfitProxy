

using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Dns;
using EryfitProxy.Kernel.Clients.DotNetBridge;
using EryfitProxy.Kernel.Clients.H11;
using EryfitProxy.Kernel.Clients.Ssl;
using EryfitProxy.Kernel.Clients.Ssl.BouncyCastle;
using EryfitProxy.Kernel.Clients.Ssl.SChannel;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Clients.H2
{
    public static class ConnectionBuilder
    {
        public static async Task<H2ConnectionPool> CreateH2(
            string hostName,
            int port = 443,
            H2StreamSetting? setting = default,
            CancellationToken token = default)
        {
            var tcpClient = new TcpClient();

            tcpClient.ReceiveBufferSize = 1024 * 128;

            await tcpClient.ConnectAsync(hostName, port).ConfigureAwait(false);

            var sslStream = new SslStream(tcpClient.GetStream());

            var sslAuthenticationOption = new SslClientAuthenticationOptions {
                TargetHost = hostName,
                ApplicationProtocols = new List<SslApplicationProtocol> {
                    SslApplicationProtocol.Http2
                }
            };

            await sslStream.AuthenticateAsClientAsync(sslAuthenticationOption,
                token).ConfigureAwait(false);

            if (sslStream.NegotiatedApplicationProtocol != SslApplicationProtocol.Http2)
                throw new NotSupportedException($"Remote ({hostName}:{port}) cannot talk HTTP2");

            var authority = new Authority(hostName, port, true);

            var connectionPool = new H2ConnectionPool(sslStream, setting ?? new H2StreamSetting(),
                authority, new Connection(authority, IIdProvider.FromZero), _ => { });

            connectionPool.Init();

            return connectionPool;
        }

        public static async Task<Http11ConnectionPool> CreateH11(
            Authority authority, SslProvider provider,
            CancellationToken token = default)
        {
            var sslProvider = provider == SslProvider.BouncyCastle
                ? (ISslConnectionBuilder)new BouncyCastleConnectionBuilder()
                : new SChannelConnectionBuilder();

            var dnsSolver = new DefaultDnsResolver();
            var timingProvider = new ITimingProvider.DefaultTimingProvider();
            var result = await DnsUtility.ComputeDns(authority, timingProvider, dnsSolver);

            var connectionPool = new Http11ConnectionPool(authority,
                new RemoteConnectionBuilder(ITimingProvider.Default,
                    sslProvider),
                ITimingProvider.Default, ProxyRuntimeSetting.CreateDefault, null!, result);

            connectionPool.Init();

            return connectionPool;
        }
    }
}
