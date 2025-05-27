

using System.Threading.Tasks;
using EryfitProxy.Kernel.Readers;

namespace EryfitProxy.Kernel.Formatters
{
    public interface IArchiveReaderProvider
    {
        Task<IArchiveReader?> Get();
    }
}
