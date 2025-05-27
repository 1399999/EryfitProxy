

using System.Collections.Generic;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription = "Select exchanges with PATCH method"
    )]
    public class PatchFilter : MethodFilter
    {
        public PatchFilter()
            : base("PATCH")
        {
        }

        public override string GenericName => "PATCH only";

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
