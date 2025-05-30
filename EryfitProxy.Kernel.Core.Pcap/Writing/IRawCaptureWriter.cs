namespace EryfitProxy.Kernel.Core.Pcap.Writing
{
    internal interface IRawCaptureWriter : IDisposable
    {
        long Key { get; }

        bool Faulted { get; }

        void Flush();

        void Register(string outFileName);

        void Write(ref PacketCapture packetCapture);

        void StoreKey(string nssKey);
    }
}
