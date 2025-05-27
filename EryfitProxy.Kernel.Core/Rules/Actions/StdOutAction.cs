

using System;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("Write text to standard output. Captured variable are interpreted."
        , NonDesktopAction = true)]
    public class StdOutAction : MultipleScopeAction
    {
        public StdOutAction(string? text)
        {
            Text = text;
        }

        [ActionDistinctive]
        public string? Text { get; set; }

        public override FilterScope ActionScope { get; } = FilterScope.OutOfScope;

        public override string DefaultDescription => "Write to stdout";

        public override ValueTask MultiScopeAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (Text != null)
                Console.Write(Text.EvaluateVariable(context));

            return default;
        }
    }
}
