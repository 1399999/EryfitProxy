

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Rules.Extensions;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    /// <summary>
    ///     Select exchanges according to host. The host is taken from the Host header (HTTP/1.1) or the :authority header
    ///     (H2).
    /// </summary>
    [FilterMetaData(
        LongDescription =
            "Select exchanges according to hostname (excluding port). To select authority (combination of host:port), use <goto>AuthorityFilter</goto>."
    )]
    public class HostFilter : StringFilter
    {
        public HostFilter(string pattern)
            : this(pattern, StringSelectorOperation.Exact)
        {
        }

        [JsonConstructor]
        public HostFilter(string pattern, StringSelectorOperation operation)
            : base(pattern, operation)
        {
        }

        public override FilterScope FilterScope => FilterScope.OnAuthorityReceived;

        public override string? ShortName => "host";

        public override string AutoGeneratedName => $"Hostname `{Pattern}`";

        public override string GenericName => "Filter by host";

        public override bool Common { get; set; } = true;

        public override IEnumerable<FilterExample> GetExamples()
        {
            yield return new FilterExample(
                "Retains only exchanges with the exact host",
                new HostFilter("www.eryfit.io", StringSelectorOperation.Exact));

            yield return new FilterExample(
                "Retains only exchanges with a host matching the regex",
                new HostFilter(@"^www\.eryfit\.io$", StringSelectorOperation.Regex));
        }

        protected override IEnumerable<string> GetMatchInputs(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange)
        {
            var hostName = authority?.HostName ?? exchange?.KnownAuthority;

            if (hostName != null)
                yield return hostName;
        }
    }

    public static class HostFilterExtensions
    {
        /// <summary>
        ///  Chain an HostFilter to a ConfigureFilterBuilder
        /// </summary>
        /// <param name="filterBuilder"></param>
        /// <param name="pattern"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenHostMatch(this IConfigureFilterBuilder filterBuilder, string pattern, StringSelectorOperation operation = StringSelectorOperation.Regex)
            => filterBuilder.When(new HostFilter(pattern, operation));

        /// <summary>
        /// Adds a filter that matches the request when the host ends with any of the specified host suffixes.
        /// </summary>
        /// <param name="filterBuilder">The filter builder.</param>
        /// <param name="hosts">The host suffixes to match against.</param>
        /// <returns>
        /// An instance of <see cref="IConfigureActionBuilder"/> with the filter added.
        /// </returns>
        public static IConfigureActionBuilder WhenHostEndsWith(
            this
                IConfigureFilterBuilder filterBuilder, params string[] hosts)
        {
            if (hosts.Length == 1)
                return filterBuilder.WhenHostMatch(hosts[0], StringSelectorOperation.EndsWith);

            return filterBuilder.WhenAny(hosts.Select(h => new HostFilter(h, StringSelectorOperation.EndsWith)).OfType<Filter>().ToArray());
        }

        /// <summary>
        /// Filters requests based on the specified hosts.
        /// </summary>
        /// <param name="filterBuilder">The filter builder instance.</param>
        /// <param name="hosts">The array of host patterns to match.</param>
        /// <returns>A configured action builder for further configuration.</returns>
        public static IConfigureActionBuilder WhenHostIn(
            this
                IConfigureFilterBuilder filterBuilder, params string[] hosts)
        {
            if (hosts.Length == 1)
                return filterBuilder.WhenHostMatch(hosts[0], StringSelectorOperation.Exact);

            return filterBuilder.WhenAny(hosts.Select(h => new HostFilter(h, StringSelectorOperation.Exact)).OfType<Filter>().ToArray());
        }

        /// <summary>
        /// Adds a filter to the filter builder that is only applied when the hosts contain any of the specified values.
        /// </summary>
        /// <param name="filterBuilder">The filter builder.</param>
        /// <param name="hosts">The list of hosts to check.</param>
        /// <returns>The configure action builder.</returns>
        public static IConfigureActionBuilder WhenHostContain(
            this
                IConfigureFilterBuilder filterBuilder, params string[] hosts)
        {
            if (hosts.Length == 1)
                return filterBuilder.WhenHostMatch(hosts[0], StringSelectorOperation.Contains);

            return filterBuilder.WhenAny(hosts.Select(h => new HostFilter(h, StringSelectorOperation.Contains)).OfType<Filter>().ToArray());
        }
    }
}
