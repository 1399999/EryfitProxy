namespace EryfitProxy.Kernel.Core.Pcap
{
    internal class SyncWriterQueue : IDisposable
    {
        private ConcurrentDictionary<long, IRawCaptureWriter> _writers = new();

        private static readonly string ApplicationName = $"eryfit {EryfitSharedSetting.RunningVersion} - https://www.eryfit.io";

        public IRawCaptureWriter GetOrAdd(long key)
        {
            lock (this) {
                return _writers.GetOrAdd(key,
                    (k) => new PcapngWriter(k, ApplicationName)); ;
            }
        }

        public bool TryGet(long key, out IRawCaptureWriter? writer)
        {
            return _writers.TryGetValue(key, out writer);
        }

        public bool TryRemove(long key, out IRawCaptureWriter? writer)
        {
            return _writers.TryRemove(key, out writer);
        }

        public void FlushAll()
        {
            foreach (var writer in _writers.Values)
            {
                writer.Flush();
            }
        }

        public void ClearAll()
        {
            var oldWriter = _writers;
            _writers = new ConcurrentDictionary<long, IRawCaptureWriter>();

            foreach (var writer in oldWriter) {
                try {
                    writer.Value.Dispose();
                }
                catch {
                    // We ignore file closing exception 
                }
            }

            oldWriter.Clear();
        }

        public void Dispose()
        {
            foreach (var writer in _writers.Values) {
                writer.Dispose();
            }
        }
    }
}