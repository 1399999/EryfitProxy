using System.Threading.Tasks;
using EryfitProxy.Kernel.Rules;

namespace EryfitProxy.Kernel.Core
{
    internal class ExchangeContextBuilder : IExchangeContextBuilder
    {
        private readonly ProxyRuntimeSetting _runtimeSetting;

        public ExchangeContextBuilder(ProxyRuntimeSetting runtimeSetting)
        {
            _runtimeSetting = runtimeSetting;
        }

        public ValueTask<ExchangeContext> Create(Authority authority, bool secure)
        {
            var result = new ExchangeContext(authority,
                _runtimeSetting.VariableContext, _runtimeSetting.StartupSetting, 
                _runtimeSetting.ActionMapping) {
                Secure = secure
            };

            return  _runtimeSetting.EnforceRules(result, FilterScope.OnAuthorityReceived); 
        }
    }
}