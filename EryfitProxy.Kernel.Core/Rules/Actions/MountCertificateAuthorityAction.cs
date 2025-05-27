

using EryfitProxy.Kernel.Clients.Mock;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Certificates;
using EryfitProxy.Kernel.Core.Breakpoints;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("Reply with the default root certificate used by eryfit")]
    public class MountCertificateAuthorityAction : Action
    {
        public override FilterScope ActionScope => FilterScope.DnsSolveDone;

        public override string DefaultDescription => "Reply with CA";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (context.EryfitSetting?.CaCertificate != null) {
                var certificate = context.EryfitSetting.CaCertificate.GetX509Certificate();

                var bodyContent = Clients.Mock.BodyContent.CreateFromArray(certificate.ExportToPem());
                var mockedResponse = new MockedResponseContent(200,
                    bodyContent) {
                    Headers = {
                        new ("Content-Type", "application/x-x509-ca-cert")
                    }
                };

                mockedResponse.Headers.Add(new ("Content-Disposition", "attachment; filename=\"ca.crt\""));

                context.PreMadeResponse = mockedResponse; 
            }

            return default;
        }
    }
}
