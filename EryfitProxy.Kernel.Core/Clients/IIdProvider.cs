

using System.Threading;

namespace EryfitProxy.Kernel.Clients
{
    public interface IIdProvider
    {
        public static IIdProvider FromZero => new FromIndexIdProvider(0, 0);

        int NextExchangeId();

        int NextConnectionId();
    }

    public class FromIndexIdProvider : IIdProvider
    {
        private volatile int _connectionIdStart;
        private volatile int _exchangeIdStart;

        public FromIndexIdProvider(int exchangeIdStart, int connectionIdStart)
        {
            _exchangeIdStart = exchangeIdStart;
            _connectionIdStart = connectionIdStart;
        }

        public int NextExchangeId()
        {
            return Interlocked.Increment(ref _exchangeIdStart);
        }

        public int NextConnectionId()
        {
            return Interlocked.Increment(ref _connectionIdStart);
        }

        /// <summary>
        ///     Set next exchange id to this value
        /// </summary>
        /// <param name="value"></param>
        public void SetNextExchangeId(int value)
        {
            if (_exchangeIdStart < value)
                _exchangeIdStart = value;
        }

        /// <summary>
        ///     Set next exchange id to this value
        /// </summary>
        /// <param name="value"></param>
        public void SetNextConnectionId(int value)
        {
            if (_connectionIdStart < value)
                _connectionIdStart = value;
        }
    }
}
