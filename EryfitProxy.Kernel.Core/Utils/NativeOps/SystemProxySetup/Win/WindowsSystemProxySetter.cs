

using System.Threading.Tasks;
using EryfitProxy.Kernel.Core.Proxy;

namespace EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup.Win
{
    internal class WindowsSystemProxySetter : ISystemProxySetter
    {
        public Task ApplySetting(SystemProxySetting value)
        {
            WindowsProxyHelper.SetProxySetting(value);
            return Task.CompletedTask;
        }

        public Task<SystemProxySetting> ReadSetting()
        {
            return Task.FromResult(WindowsProxyHelper.GetSetting());
        }
    }
}
