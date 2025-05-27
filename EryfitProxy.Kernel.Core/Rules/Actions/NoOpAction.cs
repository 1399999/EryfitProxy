

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("An action doing no operation.")]
    public class NoOpAction : Action
    {
        public override FilterScope ActionScope => FilterScope.RequestBodyReceivedFromClient;

        public override string DefaultDescription => "No operation";

        public override string? Description { get; set; } = "No operation";

        public override string FriendlyName => "NoOperation"; 

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            return default; 
        }
    }
}
