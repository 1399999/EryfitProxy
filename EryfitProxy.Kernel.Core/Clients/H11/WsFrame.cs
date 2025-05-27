

namespace EryfitProxy.Kernel.Clients.H11
{
    public struct WsFrame
    {
        public long PayloadLength { get; set; }

        public WsOpCode OpCode { get; set; }

        public bool FinalFragment { get; set; }

        public uint MaskedPayload { get; set; }
    }
}
