using System;
using System.Linq;
using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Rules.Extensions
{
    internal class ConfigureFilterBuilderBuilder : IConfigureFilterBuilder
    {
        public ConfigureFilterBuilderBuilder(EryfitSetting eryfitSetting)
        {
            EryfitSetting = eryfitSetting;
        }

        public EryfitSetting EryfitSetting { get; }

        public IConfigureActionBuilder When(Filter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return new ConfigureActionBuilder(EryfitSetting, filter);
        }

        public IConfigureActionBuilder WhenAny(params Filter[] filters)
        {
            return new ConfigureActionBuilder(EryfitSetting,
                filters.Any() ? new FilterCollection(filters) { Operation = SelectorCollectionOperation.Or }: AnyFilter.Default);
        }

        public IConfigureActionBuilder WhenAll(params Filter[] filters)
        {
            return new ConfigureActionBuilder(EryfitSetting, filters.Any() ? 
                new FilterCollection(filters) { Operation = SelectorCollectionOperation.And } 
                : NoFilter.Default);
        }
    }
}