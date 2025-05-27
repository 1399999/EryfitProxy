using System.Collections.Generic;

namespace EryfitProxy.Kernel.Clipboard
{
    public class CopyPayload
    {
        public CopyPayload(List<ExchangeData> exchanges, List<ConnectionData> connections)
        {
            Exchanges = exchanges;
            Connections = connections;
        }

        public List<ExchangeData> Exchanges { get; }

        public List<ConnectionData> Connections { get; }
    }
}