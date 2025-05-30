

using System;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;
using EryfitProxy.Kernel.Rules.Filters;
using YamlDotNet.Serialization;

namespace EryfitProxy.Kernel.Rules
{
    public class Rule
    {
        public Rule(Action action, Filter filter)
        {
            Filter = filter;
            Action = action;
        }

        [YamlIgnore]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }

        public Filter Filter { get; set; }

        public Action Action { get; set; }

        public int Order { get; set; }

        [YamlIgnore]
        public bool InScope => Filter.FilterScope <= Action.ActionScope;

        public ValueTask Enforce(
            ExchangeContext context,
            Exchange? exchange,
            Connection? connection,
            FilterScope filterScope,
            BreakPointManager breakPointManager)
        {
            context.VariableBuildingContext = new VariableBuildingContext(context, exchange, connection, filterScope);

            if (!context.FilterEvaluationResult.TryGetValue(Filter, out var result))
            {
                try {
                    result = Filter.Apply(context, context.Authority, exchange, null);
                    context.FilterEvaluationResult[Filter] = result;
                }
                catch (Exception e) {
                    if (e is RuleExecutionFailureException)
                        throw;
                    throw new RuleExecutionFailureException(e.Message, Filter, e);
                }
            }

            try {
                if (result)
                    return Action.Alter(context, exchange, connection, filterScope, breakPointManager);
            }
            catch (Exception e) {
                if (e is RuleExecutionFailureException)
                    throw;

                throw new RuleExecutionFailureException(e.Message, Action, e);
            }

            return default;
        }

        public override string ToString()
        {
            return $"Action : {Action.FriendlyName} / Filter : {Filter.FriendlyName}";
        }
    }
}
