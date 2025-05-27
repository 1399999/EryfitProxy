

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Skip validating remote certificate.
    /// </summary>
    [ActionMetadata(
        "Skip validating remote certificate. Eryfit will ignore any validation errors on the server certificate.")]
    public class SkipRemoteCertificateValidationAction : Action
    {
        public override FilterScope ActionScope => FilterScope.OnAuthorityReceived;

        public override string DefaultDescription => "Skip certificate validation";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.SkipRemoteCertificateValidation = true;

            return default;
        }
    }
}
