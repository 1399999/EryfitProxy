

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Core
{
    internal class DefaultTcpConnectionConnectResult : ITcpConnectionConnectResult
    {
        public DefaultTcpConnectionConnectResult(DisposeEventNotifierStream stream)
        {
            Stream = stream;
        }

        public DisposeEventNotifierStream Stream { get; }

        public void ProcessNssKey(string nssKey)
        {
        }
    }

    internal class DefaultTcpConnection : ITcpConnection
    {
        public async Task<ITcpConnectionConnectResult> ConnectAsync(IPAddress address, int port)
        {
            try {
                var client = new TcpClient();
                client.NoDelay = true;
                await client.ConnectAsync(address, port).ConfigureAwait(false);
                var stream = new DisposeEventNotifierStream(client, null);
                return new DefaultTcpConnectionConnectResult(stream);
            }
            catch (Exception ex) {
                if (ex is AggregateException aggregateException && aggregateException.InnerExceptions.Any()) {
                    throw aggregateException.InnerExceptions.First();
                }

                throw;
            }
        }
        
        public void OnKeyReceived(string nssKey)
        {
            // Ignore
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
