

using System.Threading.Tasks;
using EryfitProxy.Kernel.Utils.Curl;

namespace EryfitProxy.Kernel.Utils
{
    public interface IRunningProxyProvider
    {
        Task<IRunningProxyConfiguration> GetConfiguration();
    }
}
