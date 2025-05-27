

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;
using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Rules.Actions
{
    public class BreakPointAction : Action
    {
        [JsonIgnore]
        public ExchangeContext? ExchangeContext { get; private set; }

        public override FilterScope ActionScope => FilterScope.OutOfScope;

        public override string DefaultDescription { get; } = "Breakpoint";

        public override ValueTask InternalAlter(
            ExchangeContext context,
            Exchange? exchange,
            Connection? connection,
            FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (exchange == null || exchange.Id == 0) {
                return default;
            }

            if (context.BreakPointContext == null) {
                ExchangeContext = context;

                context.BreakPointContext = breakPointManager.GetOrCreate(exchange,
                    AnyFilter.Default, // Propage filter until action
                    scope);
            }

            return default;
        }
    }
}
