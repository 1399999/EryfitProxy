

using System.Collections.Generic;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription = "Select exchanges with GET method"
    )]
    public class GetFilter : MethodFilter
    {
        public GetFilter()
            : base("GET")
        {

        }

        public override string GenericName => "GET only";

        public override string ShortName => "patch";

        public override bool PreMadeFilter => true;

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample();

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
