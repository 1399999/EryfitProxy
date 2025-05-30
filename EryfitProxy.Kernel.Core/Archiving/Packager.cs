

using System;
using System.IO;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Archiving.Har;
using EryfitProxy.Kernel.Archiving.Saz;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// An helper for quickly exporting a dump directory to a specific package file
    /// </summary>
    public static class Packager
    {
        /// <summary>
        /// Export a dump directory as HttpArchive
        /// </summary>
        /// <param name="directory">Dump directory</param>
        /// <param name="exportArchiveFilePath">The outputed archive file</param>
        /// <param name="savingSetting">Save settings</param>
        public static void ExportAsHttpArchive(string directory, string exportArchiveFilePath, HttpArchiveSavingSetting? savingSetting = null)
        {
            var packager = new HttpArchivePackager(savingSetting);

            using var fileStream = File.Create(exportArchiveFilePath);

            Task.Run(() => packager.Pack(directory, fileStream, null)).GetAwaiter().GetResult();
        }

		/// <summary>
		/// Export a dump directory to a eryfit file (.fxzy).
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="exportFilePath"></param>
		public static void Export(string directory, string exportFilePath)
		{
			var packager = new FxzyDirectoryPackager();

			using var fileStream = File.Create(exportFilePath)!;
			Task.Run(() => packager.Pack(directory, fileStream, null)).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Export a dump directory to a saz file. This is an experimental feature. 
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="exportedSazFilePath"></param>
		[Obsolete]
		public static void ExportAsSaz(string directory, string exportedSazFilePath)
		{
			var packager = new SazPackager();

			using var fileStream = File.Create(exportedSazFilePath);
			Task.Run(async () => await packager.Pack(directory, fileStream, null)).GetAwaiter().GetResult();
		}
    }
}
