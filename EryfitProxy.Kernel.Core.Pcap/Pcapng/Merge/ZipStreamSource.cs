namespace EryfitProxy.Kernel.Core.Pcap.Pcapng.Merge
{
    public class ZipStreamSource : IStreamSource
    {
        private readonly ZipArchiveEntry _entry;

        public ZipStreamSource(ZipArchiveEntry entry)
        {
            _entry = entry;
        }

        public Stream Open()
        {
            return _entry.Open();
        }
    }
}