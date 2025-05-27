

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// The default eryfit archive packager
    /// </summary>
    [PackagerInformation("eryfit", "The eryfit archive format", ".fxzy", ".fzy", ".eryfit")]
    public class FxzyDirectoryPackager : DirectoryPackager
    {
        public override bool ShouldApplyTo(string fileName)
        {
            return
                fileName.EndsWith(".fxzy", StringComparison.CurrentCultureIgnoreCase) ||
                fileName.EndsWith(".eryfit", StringComparison.CurrentCultureIgnoreCase) ||
                fileName.EndsWith(".fxzy.zip", StringComparison.CurrentCultureIgnoreCase) ||
                fileName.EndsWith(".eryfit.zip", StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task Pack(string directory, string outputFileName, HashSet<int>? exchangeIds = null)
        {
            using var outputStream = new FileStream(outputFileName, FileMode.Create);
            await Pack(directory, outputStream, exchangeIds);
        }

        public override async Task Pack(string directory, Stream outputStream, HashSet<int>? exchangeIds)
        {
            var baseDirectory = new DirectoryInfo(directory);

            var packableFiles =
                GetPackableFileInfos(baseDirectory, exchangeIds);

            await ZipHelper.CompressWithFileInfos(baseDirectory, outputStream, packableFiles.Select(s => s.File));
        }

        public async Task UnpackAsync(Stream inputStream, string directoryOutput)
        {
            await ZipHelper.DecompressAsync(inputStream, new DirectoryInfo(directoryOutput));
        }

        public void Unpack(Stream inputStream, string directoryOutput)
        {
             ZipHelper.Decompress(inputStream, new DirectoryInfo(directoryOutput));
        }
    }
}
