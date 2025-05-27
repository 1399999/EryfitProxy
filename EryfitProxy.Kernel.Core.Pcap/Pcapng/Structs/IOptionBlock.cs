namespace EryfitProxy.Kernel.Core.Pcap.Pcapng.Structs
{
    internal interface IOptionBlock
    {
        int OnWireLength { get;  }

        int Write(Span<byte> buffer); 
    }
}
