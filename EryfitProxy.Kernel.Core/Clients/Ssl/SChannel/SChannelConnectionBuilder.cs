

using System;
using System.IO;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Clients.Ssl.SChannel
{
    public class SChannelConnectionBuilder : ISslConnectionBuilder
    {
        public async Task<SslConnection> AuthenticateAsClient(
            Stream innerStream,
            SslConnectionBuilderOptions builderOptions, Action<string> onKeyReceived, CancellationToken token)
        {
            var sslStream = new SslStream(innerStream, false);

            var sslOptions = builderOptions.GetSslClientAuthenticationOptions();

            await sslStream.AuthenticateAsClientAsync(sslOptions, token).ConfigureAwait(false);

            var sslInfo = new SslInfo(sslStream);

            return new SslConnection(sslStream, sslInfo, sslStream.NegotiatedApplicationProtocol);
        }
    }
}
