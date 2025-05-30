

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("Add a latency to the exchange.")]
    public class DelayAction : Action
    {
        public DelayAction(int duration)
        {
            Duration = duration;
        }

        [ActionDistinctive(Description = "Duration in milliseconds")]
        public int Duration { get; set;  }

        [ActionDistinctive(Description = "Define when the delay is applied")]
        public FilterScope Scope { get; set; } = FilterScope.ResponseHeaderReceivedFromRemote;

        public override FilterScope ActionScope => Scope;

        public override string DefaultDescription  => "Add a delay".Trim();

        public override async ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            await Task.Delay(Duration).ConfigureAwait(false);
        }
    }
}
