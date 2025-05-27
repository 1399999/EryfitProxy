

using System.Net;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Clients
{
    public interface IDnsSolver
    {
        Task<IPAddress> SolveDns(string hostName);
    }
}
