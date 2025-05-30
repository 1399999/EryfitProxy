

using System;
using System.Collections.Generic;
using System.Linq;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Formatters.Producers.Requests;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription = "Search for a cookie value present in a `set-cookie` header response." +
                          "If cookie name is not defined or empty, the filter will returns any cookie having the value."
    )]
    public class HasSetCookieOnResponseFilter : StringFilter
    {
        public HasSetCookieOnResponseFilter(string pattern)
            : base(pattern, StringSelectorOperation.Contains)
        {

        }

        public override FilterScope FilterScope => FilterScope.ResponseHeaderReceivedFromRemote;

        [FilterDistinctive(Description = "Cookie name. Leave empty to match any cookies. This field is case sensitive")]
        public string Name { get; set; } = string.Empty; 

        public override string AutoGeneratedName {
            get
            {
                if (string.IsNullOrEmpty(Pattern))
                    return "Has Set-Cookie header";
                
                return $"Has Set-Cookie header ({Pattern})";
            }
        }

        public override string GenericName => "Has Set-Cookie header";

        public override string ShortName => "set-cookie";

        protected override IEnumerable<string> GetMatchInputs(ExchangeContext? exchangeContext,
            IAuthority authority, IExchange? exchange)
        {
            // Parse cookie names 

            if (exchange == null)
                yield break; 

            var responseHeaders = exchange.GetResponseHeaders();

            if (responseHeaders == null)
                yield break;

            var setCookieHeaders = responseHeaders
                .Where(r => r.Name.Span.Equals("set-cookie", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!setCookieHeaders.Any())
                yield break;

            var name = exchangeContext == null ? Name : Name.EvaluateVariable(exchangeContext);
            
            // Let's parse that cookie 

            var setCookieItems = HttpHelper.ReadResponseCookies(setCookieHeaders).AsEnumerable();

            if (!string.IsNullOrEmpty(name)) {
                setCookieItems = setCookieItems.Where(r => r.Name.Equals(name, StringComparison.Ordinal)); 
            }

            foreach (var setCookieItem in setCookieItems) {
                yield return setCookieItem.Name;
            }
        }

        public override bool Apply(
            ExchangeContext? exchangeContext, IAuthority authority, IExchange? exchange, IFilteringContext? filteringContext)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                Pattern = "";
                Operation = StringSelectorOperation.Contains;
            }

            return base.Apply(exchangeContext, authority, exchange, filteringContext);
        }

        public override IEnumerable<FilterExample> GetExamples()
        {
            yield break;
        }

    }
}
