

using System;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("Write text to standard error. Captured variable are interpreted."
        , NonDesktopAction = true)]
    public class StdErrAction : MultipleScopeAction
    {
        public StdErrAction(string? text)
        {
            Text = text;
        }

        [ActionDistinctive]
        public string? Text { get; set; }

        public override string DefaultDescription => "Write to stderr";

        public override ValueTask MultiScopeAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (Text != null)
                Console.Error.Write(Text.EvaluateVariable(context));

            return default;
        }
    }
}
