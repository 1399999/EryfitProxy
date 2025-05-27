using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Rules.Extensions
{
    /// <summary>
    ///  Helper to build alteration rules in a fluent way
    /// </summary>
    public interface IConfigureFilterBuilder
    {
        /// <summary>
        ///  The current eryfit setting
        /// </summary>
        EryfitSetting EryfitSetting { get; }

        /// <summary>
        ///  Create a rule that will be applied when the filter passes
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        IConfigureActionBuilder When(Filter filter);

        /// <summary>
        ///  Create a rule that will be applied when any of the filters passes. If no filters are provided, the rule will be applied always.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        IConfigureActionBuilder WhenAny(params Filter[] filters);

        /// <summary>
        ///  Create a rule that will be applied when all of the filters passes
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        IConfigureActionBuilder WhenAll(params Filter[] filters);
    }
}