

using System.Collections.Generic;
using System.Linq;

namespace EryfitProxy.Kernel.Rules
{
    public class RuleSet
    {
        public RuleSet(params Rule[] rules)
        {
            Rules = rules.GroupBy(g => g.Filter.Identifier)
                         .Select(s => new RuleConfigContainer(s.First().Filter) {
                             Actions = s.Select(sm => sm.Action).ToList()
                         }).ToList();
        }

        public List<RuleConfigContainer> Rules { get; set; }
    }
}
