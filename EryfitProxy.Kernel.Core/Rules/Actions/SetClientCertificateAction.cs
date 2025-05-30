

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Certificates;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Add a client certificate to the exchange. The client certificate can be retrieved from the default store (my) or
    ///     from a PKCS#12 file (.p12, pfx)
    ///     The actual certificate is not stored in any static eryfit settings and, therefore, must be available at runtime.
    /// </summary>
    [ActionMetadata(
        "Add a client certificate to the exchange. The client certificate will be used for establishing the mTLS authentication if the remote request it. " +
        "The client certificate can be retrieved from the default store (my) or from a PKCS#12 file (.p12, pfx). <br/>" +
        "The certificate will not be stored in eryfit settings and, therefore, must be available at runtime. ")]
    public class SetClientCertificateAction : Action
    {
        public SetClientCertificateAction(Certificate? clientCertificate)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            clientCertificate ??= new Certificate {
                RetrieveMode = CertificateRetrieveMode.FromPkcs12
            };
#pragma warning restore CS0618 // Type or member is obsolete

            ClientCertificate = clientCertificate;
        }

        /// <summary>
        ///     The certificate information
        /// </summary>
        [ActionDistinctive(Expand = true, Description = "Client certificate")]
        public Certificate ClientCertificate { get; set; }

        public override FilterScope ActionScope => FilterScope.OnAuthorityReceived;

        public override string DefaultDescription => "Set client certificate".Trim();

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.ClientCertificates ??= new ();
            context.ClientCertificates.Add(ClientCertificate);

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample(
                "Use a certificate with serial number `xxxxxx` retrieved from for local user " +
                "store to establish mTLS authentication",

#pragma warning disable CS0618 // Type or member is obsolete
                new SetClientCertificateAction(new Certificate {
                    RetrieveMode = CertificateRetrieveMode.FromUserStoreSerialNumber,
                    SerialNumber = "xxxxxx"
                }));
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
