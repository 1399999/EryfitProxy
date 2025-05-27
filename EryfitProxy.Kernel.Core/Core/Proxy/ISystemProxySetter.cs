

using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Core.Proxy
{
    public interface ISystemProxySetter
    {
        Task ApplySetting(SystemProxySetting value);

        Task<SystemProxySetting> ReadSetting();
    }
}
