

using System.Threading.Tasks;
using EryfitProxy.Kernel.Readers;

namespace EryfitProxy.Kernel.Utils.Curl
{
    public interface IRequestReplayManager
    {
        Task<bool> Replay(IArchiveReader archiveReader, ExchangeInfo exchangeInfo, bool runInLiveEdit = false);
    }
}
