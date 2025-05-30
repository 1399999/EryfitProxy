

using System;
using System.Linq;
using System.Text;

namespace EryfitProxy.Kernel.Formatters.Producers.Requests
{
    public class AuthorizationBasicProducer : IFormattingProducer<AuthorizationBasicResult>
    {
        public string ResultTitle => "Basic auth";

        public AuthorizationBasicResult? Build(ExchangeInfo exchangeInfo, ProducerContext context)
        {
            var headers = exchangeInfo.GetRequestHeaders()?.ToList();

            if (headers == null)
                return null;

            var targetHeader =
                headers.FirstOrDefault(h =>
                    h.Name.Span.Equals("Authorization", StringComparison.OrdinalIgnoreCase)
                    && h.Value.Span.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase));

            if (targetHeader == null)
                return null;

            var base64Value = targetHeader.Value.Span.Slice("Basic ".Length).Trim().ToString();

            try {
                var tab = Encoding.UTF8.GetString(Convert.FromBase64String(base64Value))
                                  .Split(new[] { ":" }, StringSplitOptions.None);

                var clientId = tab.First();

                return new AuthorizationBasicResult(ResultTitle, clientId, string.Join(":", tab.Skip(1)));
            }
            catch (FormatException) {
                var errorMessage = "Basic value was not a valid base64 encoded string";

                return new AuthorizationBasicResult(ResultTitle, base64Value, null) {
                    ErrorMessage = errorMessage
                };
            }
        }
    }

    public class AuthorizationBasicResult : FormattingResult
    {
        public AuthorizationBasicResult(string title, string clientId, string? clientSecret)
            : base(title)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; }

        public string? ClientSecret { get; }
    }
}
