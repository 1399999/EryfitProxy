

using System;

namespace EryfitProxy.Kernel.Core
{
    public interface ITcpConnectionProvider : IAsyncDisposable
    {
        public static ITcpConnectionProvider Default { get; } = new DefaultTcpConnectionProvider();

        ITcpConnection Create(string dumpFileName);

        void TryFlush(); 
    }
}
