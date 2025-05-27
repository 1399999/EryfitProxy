

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Instruct eryfit not to decrypt the current traffic. The associated filter  must be on OnAuthorityReceived scope
    ///     in order to make this action effective.
    /// </summary>
    [ActionMetadata(
        "Instructs eryfit to not decrypt the current traffic. " +
        "The associated filter  must be on OnAuthorityReceived scope in order to make this action effective. ")]
    public class SkipSslTunnelingAction : Action
    {
        public override FilterScope ActionScope => FilterScope.OnAuthorityReceived;

        public override string DefaultDescription => "Do not decrypt".Trim();

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.BlindMode = true;
            return default;
        }
    }
}
