

using System.Collections.Generic;
using System.Linq;
using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Core.Breakpoints
{
    /// <summary>
    ///     The view model of the breakpoint status
    /// </summary>
    public class BreakPointState
    {
        public BreakPointState(List<BreakPointContextInfo> entries, List<Filter> activeFilters)
        {
            Entries = entries;
            ActiveFilters = activeFilters;
        }

        /// <summary>
        ///     Define is debugging window has to popup
        /// </summary>
        public bool HasToPop {
            get { return Entries.Any(e => e.CurrentHit != null); }
        }

        public int ActiveEntries => Entries.Count(e => e.CurrentHit != null); 

        public List<BreakPointContextInfo> Entries { get; }

        /// <summary>
        /// true, if anyfilter is present 
        /// </summary>
        public bool AnyEnabled => ActiveFilters.Any(f => f is AnyFilter);

        /// <summary>
        /// Is Catching 
        /// </summary>
        public bool IsCatching => ActiveFilters.Any(); 

        /// <summary>
        /// true, if any request is pending 
        /// </summary>
        public bool AnyPendingRequest => ActiveEntries > 0; 

        /// <summary>
        /// Filter enabling break point 
        /// </summary>
        public List<Filter> ActiveFilters { get; }

        public int [] PausedExchangeIds => Entries.Where(e => e.CurrentHit != null).Select(s => s.ExchangeId).ToArray();


        public static BreakPointState EmptyEntries { get; } = new(new List<BreakPointContextInfo>(), new());

    }
}
