

using System.Collections.Generic;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Rules.Filters.ResponseFilters
{
    /// <summary>
    ///     Select exchanges that response status code indicates a server error (5XX)
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select exchanges that HTTP status code indicates a server/intermediary error (5XX)."
    )]
    public class StatusCodeServerErrorFilter : Filter
    {
        public override FilterScope FilterScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public override string AutoGeneratedName => "Server errors (status code is 5XX)";

        public override string GenericName => "Status code 5XX only";

        public override string ShortName => "5XX";

        public override bool PreMadeFilter => true;

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            if (exchange == null)
                return false;

            var statusCode = exchange.StatusCode;

            return statusCode is >= 500 and < 600;
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample();

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
