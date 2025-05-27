

namespace EryfitProxy.Kernel.Readers
{
    /// <summary>
    ///  Defines the expected behavior of an archive import engine
    /// </summary>
    public interface IImportEngine
    {
        bool IsFormat(string fileName);

        void WriteToDirectory(string fileName, string directory); 
    }
}
