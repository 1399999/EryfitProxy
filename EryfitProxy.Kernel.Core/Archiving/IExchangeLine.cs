

using System.Text.Json.Serialization;

namespace EryfitProxy.Kernel
{
    public interface IExchangeLine
    {
        public int Id { get; }

        public int ConnectionId { get; }

        public string Method { get; }

        public string Path { get; }

        public string KnownAuthority { get; }

        public int KnownPort { get; }

        public bool Secure { get; }

        public int StatusCode { get; }

        public string? Comment { get; }

        bool Pending { get; }

        string? ContentType { get; }

        long Received { get;  }

        long Sent { get;  }

    }
}
