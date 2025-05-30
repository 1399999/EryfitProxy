

using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc;
using EryfitProxy.Kernel.Readers;

namespace EryfitProxy.Kernel.Utils.Curl
{
    public class CurlRequestReplayManager : IRequestReplayManager
    {
        private readonly CurlRequestConverter _requestConverter;
        private readonly IRunningProxyProvider _runningProxyProvider;

        public CurlRequestReplayManager(
            CurlRequestConverter requestConverter, IRunningProxyProvider runningProxyProvider)
        {
            _requestConverter = requestConverter;
            _runningProxyProvider = runningProxyProvider;
        }

        public async Task<bool> Replay(IArchiveReader archiveReader, ExchangeInfo exchangeInfo, bool runInLiveEdit = false)
        {
            var configuration = await _runningProxyProvider.GetConfiguration().ConfigureAwait(false);

            var curlCommandResult =
                _requestConverter.BuildCurlRequest(archiveReader, exchangeInfo, configuration, runInLiveEdit);

            var args = curlCommandResult.GetProcessCompatibleArgs();

            if (!ProcessUtils.IsCommandAvailable("curl"))
                return false;

            try {
                return await CurlUtility.RunCurl(args, null).ConfigureAwait(false);
            }
            catch {
                // We just ignore all run errors 

                return false;
            }
        }
    }
}
