

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///    Instruct eryfit to use an upstream proxy.
    /// </summary>
    [ActionMetadata("Use an upstream proxy.", NonDesktopAction = true)]
    public class UpStreamProxyAction : Action
    {
        public UpStreamProxyAction(string host, int port)
        {
            Host = host;
            Port = port;
        }

        [ActionDistinctive]
        public string Host { get; set; }

        [ActionDistinctive]
        public int Port { get; set; }

        [ActionDistinctive]
        public string? ProxyAuthorizationHeader { get; set; }

        public override FilterScope ActionScope => FilterScope.OnAuthorityReceived;

        public override string DefaultDescription => $"Upstream proxy to {Host}:{Port}"; 

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (Host != null! && Port != 0)
                context.ProxyConfiguration = new ProxyConfiguration(Host, Port, ProxyAuthorizationHeader);

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample("Use an upstream proxy to 192.168.1.9 on port 8080",
                new UpStreamProxyAction("192.168.1.9", 8080));

            yield return new ActionExample("Use an upstream proxy to 192.168.1.9 on port 8080 with basic auth" +
                                           " login: leeloo , password: multipass",
                new UpStreamProxyAction("192.168.1.9", 8080) {
                    ProxyAuthorizationHeader = "Basic bGVlbG9vOm11bHRpcGFzcw=="
                });
        }
    }
}
