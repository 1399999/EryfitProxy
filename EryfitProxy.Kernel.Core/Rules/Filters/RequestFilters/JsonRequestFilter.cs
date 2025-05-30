

using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Rules.Extensions;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription =
            "Select request sending JSON body. Filtering is made by inspecting value of `Content-Type` header"
    )]
    public class JsonRequestFilter : RequestHeaderFilter
    {
        public JsonRequestFilter()
            : base("json", StringSelectorOperation.Contains, "content-type")
        {
        }

        public override FilterScope FilterScope => FilterScope.RequestBodyReceivedFromClient;

        public override string GenericName => "Has a request body JSON";

        public override string AutoGeneratedName { get; } = "JSON request body";

        public override string ShortName => "snt. json";

        public override bool PreMadeFilter => true;
    }

    public static class JsonRequestFilterExtensions
    {
        /// <summary>
        ///  When request has a JSON body. Filtering is made by inspecting value of `Content-Type` header"
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigureActionBuilder WhenRequestHasJsonBody(this IConfigureFilterBuilder builder)
        {
            return builder.When(new JsonRequestFilter());
        }
    }
}
