

using System.Collections.Generic;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Rules.Filters
{
    /// <summary>
    ///     Select exchanges that contain comment
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select exchanges having comment."
    )]
    public class HasCommentFilter : Filter
    {
        public override FilterScope FilterScope => FilterScope.OutOfScope;

        public override string AutoGeneratedName => "Has any comment";

        public override bool PreMadeFilter => true;

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            return !string.IsNullOrWhiteSpace(exchange?.Comment);
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample();

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
