

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Headers;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    /// <summary>
    ///     Remove response headers. This actions remove <b>every</b> occurrence of the header from the response.
    /// </summary>
    [ActionMetadata(
        "Remove response headers. This action removes <b>every</b> occurrence of the header from the response.")]
    public class DeleteResponseHeaderAction : Action
    {
        public DeleteResponseHeaderAction(string headerName)
        {
            HeaderName = headerName;
        }

        /// <summary>
        ///     Header name
        /// </summary>
        [ActionDistinctive]
        public string HeaderName { get; set; }

        public override FilterScope ActionScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public override string DefaultDescription => $"Remove response header {HeaderName}".Trim();

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.ResponseHeaderAlterations.Add(new HeaderAlterationDelete(HeaderName.EvaluateVariable(context)!));

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample("Remove every Set-Cookie header from response",
                new DeleteResponseHeaderAction("Set-Cookie"));
        }
    }
}
