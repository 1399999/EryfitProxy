

using System.IO;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.H11;

namespace EryfitProxy.Kernel.Formatters.Producers.ProducerActions.Actions
{
    public class SaveWebSocketBodyAction
    {
        private readonly IArchiveReaderProvider _archiveReaderProvider;

        public SaveWebSocketBodyAction(IArchiveReaderProvider archiveReaderProvider)
        {
            _archiveReaderProvider = archiveReaderProvider;
        }

        public async Task<bool> Do(int exchangeId, int messageId, WsMessageDirection direction, string filePath)
        {
            var archiverReader = await _archiveReaderProvider.Get();

            if (archiverReader == null)
                return false;

            if (direction == WsMessageDirection.Sent) {
                using var stream =
                    archiverReader.GetRequestWebsocketContent(exchangeId, messageId);

                if (stream == null)
                    return false;

                using var outStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                await stream.CopyToAsync(outStream);

                return true;
            }

            if (direction == WsMessageDirection.Receive) {
                using var stream =
                    archiverReader.GetResponseWebsocketContent(exchangeId, messageId);

                if (stream == null)
                    return false;

                using var outStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                await stream.CopyToAsync(outStream);

                return true;
            }

            return false;
        }
    }
}
