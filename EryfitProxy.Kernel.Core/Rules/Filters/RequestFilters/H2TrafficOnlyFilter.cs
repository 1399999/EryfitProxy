

using System.Collections.Generic;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    /// <summary>
    ///     Select HTTP/2.0 traffic only
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select H2 exchanges only."
    )]
    public class H2TrafficOnlyFilter : Filter
    {
        public override FilterScope FilterScope => FilterScope.RequestHeaderReceivedFromClient;

        public override string GenericName => "HTTP/2 only";

        public override string AutoGeneratedName { get; } = "HTTP/2 only";

        public override string ShortName => "h2";

        public override bool PreMadeFilter => true;

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            return exchange?.HttpVersion == "HTTP/2";
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample();

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
