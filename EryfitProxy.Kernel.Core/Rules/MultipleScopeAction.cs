

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules
{
    /// <summary>
    ///     Defines a class of Action that can be run several times during the same exchange.
    ///     The RunScope property allowed to set the specific scope when the action should be run.
    /// </summary>
    public abstract class MultipleScopeAction : Action
    {
        [ActionDistinctive(Description =
            "When RunScope is defined. The action is only evaluated when the value of the scope occured.")]
        public FilterScope? RunScope { get; set; } = null;

        public override FilterScope ActionScope { get; } = FilterScope.CopySibling;

        public abstract ValueTask MultiScopeAlter(
            ExchangeContext context,
            Exchange? exchange,
            Connection? connection,
            FilterScope scope,
            BreakPointManager breakPointManager);

        public override ValueTask InternalAlter(
            ExchangeContext context,
            Exchange? exchange,
            Connection? connection,
            FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (RunScope != null && RunScope != scope)
                return default;

            return MultiScopeAlter(context, exchange, connection, scope, breakPointManager);
        }
    }
}
