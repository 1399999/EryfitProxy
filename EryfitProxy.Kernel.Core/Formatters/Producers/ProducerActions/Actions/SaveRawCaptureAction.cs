

using System.IO;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Formatters.Producers.ProducerActions.Actions
{
    public class SaveRawCaptureAction
    {
        private readonly IArchiveReaderProvider _archiveReaderProvider;

        public SaveRawCaptureAction(IArchiveReaderProvider archiveReaderProvider)
        {
            _archiveReaderProvider = archiveReaderProvider;
        }

        public async Task<bool> Do(int connectionId, string filePath)
        {
            var archiveReader = await _archiveReaderProvider.Get();

            if (archiveReader == null)
                return false;

            await using var rawFileStream = archiveReader.GetRawCaptureStream(connectionId);

            if (rawFileStream == null)
                return false;

            await using var outStream = File.Create(filePath);

            await rawFileStream.CopyToAsync(outStream);

            return true;
        }
    }
}
