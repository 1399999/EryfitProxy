

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Core
{
    internal static class ConnectionErrorPageHelper
    {
        private static readonly string ErrorHeaderHtml =
            "HTTP/1.1 {0}\r\n" +
            "x-eryfit: Eryfit error\r\n" +
            "Content-length: {1}\r\n" +
            "Content-type: text/html; charset: utf-8\r\n" +
            "Connection : close\r\n\r\n";

        private static readonly string ErrorHeaderText =
            "HTTP/1.1 {0}\r\n" +
            "x-eryfit: Eryfit error\r\n" +
            "x-eryfit-error-type: {2}\r\n" +
            "Content-length: {1}\r\n" +
            "Content-type: text/plain; charset: utf-8\r\n" +
            "Connection : close\r\n\r\n";

        private static string BodyTemplate { get;  }

        static ConnectionErrorPageHelper()
        {
            var bodyTemplatePath = Environment.GetEnvironmentVariable("EryfitErrorPageTemplatePath");

            if (!string.IsNullOrEmpty(bodyTemplatePath)) {
                BodyTemplate = File.ReadAllText(bodyTemplatePath);
            }
            else {
                BodyTemplate = FileStore.error;
            }
        }

        public static (string FlatHeader, byte[] BodyContent) GetPrettyErrorPage(
            Authority authority, 
            IEnumerable<ClientError> clientErrors, Exception? originalException)
        {
            // TODO: This block section is slow due to multiple string concatenation
            // TODO: Consider using a string builder instead

            var headerStatus = "528 Eryfit error";
            var rawStatusCode = "528"; 

            if (!EryfitSharedSetting.Use528) {
                headerStatus = "502 Bad Gateway";
                rawStatusCode = "502";
            }

            var errorMessage = string.Join("<br/><br/>",
                clientErrors.Select(s => s.Message).Where(s => !string.IsNullOrWhiteSpace(s)));

            if (string.IsNullOrEmpty(errorMessage)) {
                errorMessage = originalException?.Message ?? "Internal eryfit error.";
            }

            var bodyTemplate = BodyTemplate;

            bodyTemplate = bodyTemplate.Replace("@@error-status-code@@", rawStatusCode);
            bodyTemplate = bodyTemplate.Replace("@@error-host@@", authority.ToString());
            bodyTemplate = bodyTemplate.Replace("@@error-message@@", errorMessage);

            var body = Encoding.UTF8.GetBytes(bodyTemplate);
            var header = string.Format(ErrorHeaderHtml, headerStatus, body.Length);
            return (header, body);
        }

        public static (string FlatHeader, byte[] BodyContent) GetSimplePlainTextResponse(
            Authority authority, string messageText, string errorTypeText)
        {
            var statusLine = "502 Eryfit Configuration Error";
            var header = string.Format(ErrorHeaderText, statusLine, messageText.Length, errorTypeText);
            var body = Encoding.UTF8.GetBytes(messageText);
            return (header, body);
        }
    }
}
