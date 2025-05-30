

using System;
using System.Collections.Generic;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Misc;

namespace EryfitProxy.Kernel.Rules.Filters
{
    /// <summary>
    ///     Select exchanges that are websocket communication
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select websocket exchange."
    )]
    public class IsWebSocketFilter : Filter
    {
        public override Guid Identifier => $"{Inverted}{GetType()}".GetMd5Guid();

        public override FilterScope FilterScope => FilterScope.RequestHeaderReceivedFromClient;

        public override string GenericName => "Websocket";

        public override string? ShortName => "ws";

        public override string? Description { get; set; } = "Websocket exchange";

        public override bool PreMadeFilter => true;

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            return exchange?.IsWebSocket ?? false;
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample();

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
