

namespace EryfitProxy.Kernel.Clients.H11
{
    public readonly struct HeaderBlockReadResult
    {
        public HeaderBlockReadResult(int headerLength, int totalReadLength, bool closeNotify)
        {
            HeaderLength = headerLength;
            TotalReadLength = totalReadLength;
            CloseNotify = closeNotify;
        }

        public int HeaderLength { get; }

        public int TotalReadLength { get; }

        public bool CloseNotify { get;  }
    }
}
