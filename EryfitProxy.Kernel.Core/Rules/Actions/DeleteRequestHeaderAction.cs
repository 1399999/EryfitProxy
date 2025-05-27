

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Headers;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Remove request headers. This actions remove <b>every</b> occurrence of the header from the request
    /// </summary>
    [ActionMetadata(
        "Remove request headers. This action removes <b>every</b> occurrence of the header from the request.")]
    public class DeleteRequestHeaderAction : Action
    {
        public DeleteRequestHeaderAction(string headerName)
        {
            HeaderName = headerName;
        }

        /// <summary>
        ///     Header name
        /// </summary>
        [ActionDistinctive]
        public string HeaderName { get; set; }

        public override FilterScope ActionScope => FilterScope.RequestHeaderReceivedFromClient;

        public override string DefaultDescription => $"Remove header {HeaderName}".Trim();

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.RequestHeaderAlterations.Add(
                new HeaderAlterationDelete(HeaderName.EvaluateVariable(context) ?? string.Empty));

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample("Remove every Cookie header from request",
                new DeleteRequestHeaderAction("Cookie"));
        }
    }
}
