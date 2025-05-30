

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Headers;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Append a response header.
    ///     <strong>Note</strong> Headers that alter the connection behaviour will be ignored.
    /// </summary>
    [ActionMetadata("Append a response header. H2 pseudo header will be ignored.")]
    public class AddResponseHeaderAction : Action
    {
        public AddResponseHeaderAction(string headerName, string headerValue)
        {
            HeaderName = headerName;
            HeaderValue = headerValue;
        }

        /// <summary>
        ///     Header name
        /// </summary>
        [ActionDistinctive]
        public string HeaderName { get; set; }

        /// <summary>
        ///     Header value
        /// </summary>
        [ActionDistinctive]
        public string HeaderValue { get; set; }

        public override FilterScope ActionScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public override string DefaultDescription =>
            string.IsNullOrWhiteSpace(HeaderName)
                ? "Add response header"
                : $"Add response header ({HeaderName}, {HeaderValue})";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (string.IsNullOrWhiteSpace(HeaderName))
                throw new RuleExecutionFailureException("Header name cannot be empty", this);

            context.ResponseHeaderAlterations.Add(new HeaderAlterationAdd(HeaderName.EvaluateVariable(context) ?? string.Empty,
                HeaderValue.EvaluateVariable(context) ?? string.Empty));

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample("Add a `content-security-policy` header on response",
                new AddResponseHeaderAction("content-security-policy", "default-src 'none'"));
        }
    }
}
