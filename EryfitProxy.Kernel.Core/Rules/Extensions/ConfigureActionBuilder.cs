using System;
using System.Linq;
using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Rules.Extensions
{
    internal class ConfigureActionBuilder : IConfigureActionBuilder
    {
        public EryfitSetting Setting { get; }

        private readonly Filter _filter;

        public ConfigureActionBuilder(EryfitSetting setting, Filter filter)
        {
            Setting = setting;
            _filter = filter;
        }
        
        public IConfigureFilterBuilder Do(Action action, params Action [] actions)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            Setting.AddAlterationRules(new Rule(action, _filter));
            Setting.AddAlterationRules(actions.Select(a => new Rule(a, _filter)));
            
            return new ConfigureFilterBuilderBuilder(Setting);
        }
    }
}