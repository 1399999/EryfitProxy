

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Formatters.Producers.Requests;
using EryfitProxy.Kernel.Formatters.Producers.Responses;

namespace EryfitProxy.Kernel.Formatters
{
    public class ProducerFactory
    {
        private static readonly List<IFormattingProducer<FormattingResult>> RequestProducers = new() {
            new RequestJsonBodyProducer(),
            new MultipartFormContentProducer(),
            new FormUrlEncodedProducer(),
            new QueryStringProducer(),
            new RequestBodyAnalysis(),
            new AuthorizationBasicProducer(),
            new AuthorizationBearerProducer(),
            new RequestCookieProducer(),
            new RequestTextBodyProducer(),
            new AuthorizationProducer(),
            new RawRequestHeaderProducer()
        };

        private static readonly List<IFormattingProducer<FormattingResult>> ResponseProducers = new() {
            new ResponseBodySummaryProducer(),
            new WsMessageProducer(),
            new ImageResultProducer(),
            new ResponseBodyJsonProducer(),
            new SetCookieProducer(),
            new ResponseTextContentProducer(),
        };

        private readonly IArchiveReaderProvider _archiveReaderProvider;
        private readonly FormatSettings _formatSettings;

        public ProducerFactory(IArchiveReaderProvider archiveReaderProvider, FormatSettings formatSettings)
        {
            _archiveReaderProvider = archiveReaderProvider;
            _formatSettings = formatSettings;
        }

        public async Task<ProducerContext?> GetProducerContext(int exchangeId)
        {
            var archiveReader = await _archiveReaderProvider.Get().ConfigureAwait(false);

            if (archiveReader == null)
                return null;

            var exchangeInfo = archiveReader.ReadExchange(exchangeId);

            if (exchangeInfo == null)
                return null;

            return new ProducerContext(exchangeInfo, archiveReader, _formatSettings);
        }

        public IEnumerable<FormattingResult> GetRequestFormattedResults(
            int exchangeId,
            ProducerContext formattingProducerContext)
        {
            foreach (var producer in RequestProducers) {
                var result = producer.Build(formattingProducerContext.Exchange, formattingProducerContext);

                if (result != null)
                    yield return result;
            }
        }

        public IEnumerable<FormattingResult> GetResponseFormattedResults(
            int exchangeId,
            ProducerContext formattingProducerContext)
        {
            if (formattingProducerContext.Exchange.StatusCode == 528)
                yield break; // No formatters for transport level error

            foreach (var producer in ResponseProducers) {
                var result = producer.Build(formattingProducerContext.Exchange, formattingProducerContext);

                if (result != null)
                    yield return result;
            }
        }
    }
}
