

using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Core
{
    public class DefaultTcpConnectionProvider : ITcpConnectionProvider
    {
        public ITcpConnection Create(string _)
        {
            return new DefaultTcpConnection();
        }

        public void TryFlush()
        {
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
