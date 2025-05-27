

using System.IO;

namespace EryfitProxy.Kernel.Core
{
    internal class ExchangeSourceInitResult 
    {
        public ExchangeSourceInitResult(IDownStreamPipe downStreamPipe, Exchange provisionalExchange)
        {
            DownStreamPipe = downStreamPipe;
            ProvisionalExchange = provisionalExchange;
        }

        public Exchange ProvisionalExchange { get; }

        public IDownStreamPipe DownStreamPipe { get; }
    }
}
