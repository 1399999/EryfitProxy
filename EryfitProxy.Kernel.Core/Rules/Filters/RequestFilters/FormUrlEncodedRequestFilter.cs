

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription =
            "Select request sending 'application/x-www-form-urlencoded' body. Filtering is made by inspecting value of `Content-Type` header"
    )] 
    public class FormUrlEncodedRequestFilter : RequestHeaderFilter
    {
        public FormUrlEncodedRequestFilter()
            : base("application/x-www-form-urlencoded", StringSelectorOperation.Contains, "content-type")
        {
        }
        
        public override FilterScope FilterScope => FilterScope.RequestHeaderReceivedFromClient;
        
        public override string GenericName => "Has a request body form url encoded";

        public override string AutoGeneratedName { get; } = "Form url encoded";

        public override string ShortName => "frm .url";

        public override bool PreMadeFilter => true;
    }
}
