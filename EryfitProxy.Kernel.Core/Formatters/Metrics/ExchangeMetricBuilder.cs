

using EryfitProxy.Kernel.Readers;

namespace EryfitProxy.Kernel.Formatters.Metrics
{
    public class ExchangeMetricBuilder
    {
        public ExchangeMetricInfo? Get(int exchangeId, IArchiveReader reader)
        {
            var exchange = reader.ReadExchange(exchangeId);

            if (exchange == null)
                return null;

            var connectionInfo = reader.ReadConnection(exchange.ConnectionId);

            return new ExchangeMetricInfo(exchangeId, exchange.Metrics, connectionInfo);
        }
    }
}
