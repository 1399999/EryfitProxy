

using System.Collections.Generic;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Rules.Filters
{
    [FilterMetaData(
        LongDescription = "Select exchanges having a specific connection id.",
        NotSelectable = true
    )]
    public class ConnectionFilter : Filter
    {
        public ConnectionFilter(int connectionId)
        {
            ConnectionId = connectionId;
        }

        public override FilterScope FilterScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public int ConnectionId { get; set; }

        public override string ShortName => $"con. #{ConnectionId}";

        public override string AutoGeneratedName => $"Same connection ##{ConnectionId}";

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            if (exchange == null)
                return false;

            if (exchange is Exchange originalExchange)
                return originalExchange.Connection?.Id == ConnectionId;

            if (exchange is ExchangeInfo exchangeInfo)
                return exchangeInfo.ConnectionId == ConnectionId;

            return false;
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            yield break;
        }
    }
}
