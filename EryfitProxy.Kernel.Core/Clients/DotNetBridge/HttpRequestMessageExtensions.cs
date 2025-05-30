

using System.Net.Http;
using System.Text;

namespace EryfitProxy.Kernel.Clients.DotNetBridge
{
    /// <summary>
    /// Request message extensions
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Returns a flat HTTP/1.1 string representation of the request message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToHttp11String(this HttpRequestMessage message)
        {
            var builder = new StringBuilder();

            builder.Append($"{message.Method} {message.RequestUri} HTTP/1.1\r\n");
            builder.Append($"Host: {message.RequestUri!.Authority}\r\n");

            foreach (var header in message.Headers) {
                foreach (var value in header.Value) {
                    builder.Append(header.Key);
                    builder.Append(": ");
                    builder.Append(value);
                    builder.Append("\r\n");
                }
            }

            // Do not remove that line because evaluating ContentLength is necessary
            var clAsk = message?.Content?.Headers.ContentLength;

            if (message!.Content?.Headers != null) {
                foreach (var header in message.Content.Headers) {
                    foreach (var value in header.Value) {
                        builder.Append(header.Key);
                        builder.Append(": ");
                        builder.Append(value);
                        builder.Append("\r\n");
                    }
                }
            }

            var s = message.ToString();

            builder.Append("\r\n");
            var yo = builder.ToString();

            return yo;
        }
    }
}
