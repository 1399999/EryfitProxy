

using System.Collections.Generic;

namespace EryfitProxy.Kernel.Rules.Filters.RequestFilters
{
    [FilterMetaData(
        LongDescription = "Select exchanges with DELETE method"
    )]
    public class DeleteFilter : MethodFilter
    {
        public DeleteFilter()
            : base("DELETE")
        {
        }

        public override string GenericName => "DELETE only";

        public override string ShortName => "del";

        public override bool PreMadeFilter => true;

        public override IEnumerable<FilterExample> GetExamples()
        {
            var defaultSample = GetDefaultSample(); 

            if (defaultSample != null)
                yield return defaultSample;
        }
    }
}
