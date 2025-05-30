

using System;
using System.Linq;

namespace EryfitProxy.Kernel.Formatters.Producers.Requests
{
    public class AuthorizationProducer : IFormattingProducer<AuthorizationResult>
    {
        public string ResultTitle => "Authorization Header";

        public AuthorizationResult? Build(ExchangeInfo exchangeInfo, ProducerContext context)
        {
            var headers = exchangeInfo.GetRequestHeaders()?.ToList();

            if (headers == null)
                return null;

            var targetHeader =
                headers.FirstOrDefault(h =>
                    h.Name.Span.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            if (targetHeader == null)
                return null;

            var value = targetHeader.Value.Span.Trim().ToString();

            return new AuthorizationResult(ResultTitle, value);
        }
    }

    public class AuthorizationResult : FormattingResult
    {
        public AuthorizationResult(string title, string value)
            : base(title)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
