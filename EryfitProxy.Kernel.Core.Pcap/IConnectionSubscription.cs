namespace EryfitProxy.Kernel.Core.Pcap
{
    public interface IConnectionSubscription : IAsyncDisposable
    {
        long Key { get; }
    }
}
