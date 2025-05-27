

using System.Buffers;
using System.Linq;
using System.Text;
using EryfitProxy.Kernel.Clients.H2.Encoder.Utils;

namespace EryfitProxy.Kernel.Formatters.Producers.Requests
{
    public class RawRequestHeaderProducer : IFormattingProducer<RawRequestHeaderResult>
    {
        public string ResultTitle => "Raw header (H11 style)";

        public RawRequestHeaderResult? Build(ExchangeInfo exchangeInfo, ProducerContext context)
        {
            var requestHeaders = exchangeInfo.GetRequestHeaders().ToList();
            var stringBuilder = new StringBuilder();

            var charBuffer = ArrayPool<char>.Shared.Rent(context.Settings.MaxHeaderLength);

            try {
                var spanRes = Http11Parser.Write(requestHeaders, charBuffer);

                stringBuilder.Append(spanRes);
            }
            finally {
                ArrayPool<char>.Shared.Return(charBuffer);
            }

            if (context.RequestBodyText != null)
                stringBuilder.Append(context.RequestBodyText);

            return new RawRequestHeaderResult(ResultTitle, stringBuilder.ToString());
        }
    }

    public class RawRequestHeaderResult : FormattingResult
    {
        public RawRequestHeaderResult(string title, string rawHeader)
            : base(title)
        {
            RawHeader = rawHeader;
        }

        public string RawHeader { get; }
    }
}
