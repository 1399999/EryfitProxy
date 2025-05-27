

using System;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Core
{
    internal class SystemProxyRegistration : IAsyncDisposable
    {
        private readonly SystemProxyRegistrationManager _instance;
        internal SystemProxyRegistration(SystemProxyRegistrationManager instance)
        {
            _instance = instance;
        }
        
        public async ValueTask DisposeAsync()
        {
            await _instance.UnRegister().ConfigureAwait(false); 
        }
    }
}
