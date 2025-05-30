

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Rules.Extensions;

namespace EryfitProxy.Kernel.Rules.Filters.ResponseFilters
{
    /// <summary>
    ///     Select exchanges according to response header values.
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select exchanges according to response header values."
    )]
    public class ResponseHeaderFilter : HeaderFilter
    {
        public ResponseHeaderFilter(string pattern, string headerName)
            : base(pattern, headerName)
        {
        }

        [JsonConstructor]
        public ResponseHeaderFilter(string pattern, StringSelectorOperation operation, string headerName)
            : base(pattern, operation, headerName)
        {
        }

        public override FilterScope FilterScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public override string ShortName => "resp head.";

        public override string GenericName => "Filter by response header";

        public override string AutoGeneratedName => $"Response header `{Pattern}`";

        public override IEnumerable<FilterExample> GetExamples()
        {
            // strict-transport-security

            yield return new FilterExample(
                "Retains only exchanges with a strict-transport-security response header",
                new ResponseHeaderFilter(@".*",
                    StringSelectorOperation.Regex, "strict-transport-security"));
        }

        protected override IEnumerable<string> GetMatchInputs(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange)
        {
            return exchange?.GetResponseHeaders()?.Where(e =>
                               e.Name.Span.Equals(HeaderName.AsSpan(), StringComparison.InvariantCultureIgnoreCase))
                           .Select(s => s.Value.ToString()) ?? Array.Empty<string>();
        }
    }

    public static class ResponseHeaderFilterExtensions
    {
        public static IConfigureActionBuilder WhenResponseHeaderMatch(
            this IConfigureFilterBuilder builder, string headerName, string headerValue,
            StringSelectorOperation headerValueOperation = StringSelectorOperation.Exact)
        {
            return builder.When(new ResponseHeaderFilter(headerValue, headerValueOperation, headerName));
        }

        public static IConfigureActionBuilder WhenResponseHeaderExists(
            this IConfigureFilterBuilder builder, string headerName)
        {
            return builder.When(new ResponseHeaderFilter("", StringSelectorOperation.Contains, headerName));
        }
    }
}
