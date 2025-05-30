

using System.Collections.Generic;
using System.Linq;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Rules.Extensions;

namespace EryfitProxy.Kernel.Rules.Filters.ResponseFilters
{
    /// <summary>
    ///     Select exchanges according to HTTP status code
    /// </summary>
    [FilterMetaData(
        LongDescription = "Select exchanges according to HTTP status code."
    )]
    public class StatusCodeFilter : Filter
    {
        [FilterDistinctive(Description = "List of status code")]
        public List<int> StatusCodes { get; set; } = new();

        public override FilterScope FilterScope => FilterScope.ResponseHeaderReceivedFromRemote;

        public override string AutoGeneratedName => StatusCodes.Count == 1 ? 
            $"Status code {StatusCodes.First()}" 
            : $"Status code among {string.Join(", ", StatusCodes.Select(s => s.ToString()))}";


        public override string GenericName => "Filter by status code";

        public override string ShortName => new(string.Join(",", StatusCodes.Select(s => s.ToString()))
                                                      .Take(6).ToArray());

        public override bool PreMadeFilter => false;

        protected override bool InternalApply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange,
            IFilteringContext? filteringContext)
        {
            if (exchange == null)
                return false;

            return StatusCodes.Contains(exchange.StatusCode);
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            // This filter should be removed
            yield break;
        }
    }

    public static class StatusCodeFilterExtensions
    {
        public static IConfigureActionBuilder WhenStatusCode(this IConfigureFilterBuilder builder, params int [] statusCodes)
        {
            return builder.When(new StatusCodeFilter { StatusCodes = statusCodes.ToList()});
        }

        /// <summary>
        ///   
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenSuccess(this IConfigureFilterBuilder builder)
        {
            return builder.When(new StatusCodeSuccessFilter());
        }

        /// <summary>
        ///   When remote returnx 3XX
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenRedirect(this IConfigureFilterBuilder builder)
        {
            return builder.When(new StatusCodeRedirectionFilter());
        }

        /// <summary>
        ///  When remote returnx 4XX
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenClientError(this IConfigureFilterBuilder builder)
        {
            return builder.When(new StatusCodeClientErrorFilter());
        }

        /// <summary>
        ///  When remote returnx 5XX
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenServerError(this IConfigureFilterBuilder builder)
        {
            return builder.When(new StatusCodeServerErrorFilter());
        }
    }


}
