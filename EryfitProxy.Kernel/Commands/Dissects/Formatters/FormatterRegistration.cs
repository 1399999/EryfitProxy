using System.Collections.Generic;
using System.Linq;

namespace EryfitProxy.Kernel.Cli.Commands.Dissects.Formatters
{
    internal static class FormatterRegistration
    {
        public static readonly IReadOnlyCollection<IDissectionFormatter<EntryInfo>>
            Formatters = new List<IDissectionFormatter<EntryInfo>> {
                new UrlFormatter(),
                new MethodFormatter(),
                new StatusCodeFormatter(),
                new ContentTypeFormatter(),
                new AuthorityFormatter(),
                new PathFormatter(),
                new HostFormatter(),
                new IdFormatter(),
                new HttpVersionFormatter(),
                new SchemeFormatter(),
                new RequestBodyLengthFormatter(),
                new ResponseBodyLengthFormatter(),
                new ResponseBodyFormatter(),
                new RequestBodyFormatter(),
                new PcapFormatter(),
                new PcapRawFormatter()
            };

        public static IReadOnlyCollection<string> Indicators { get; } = Formatters.Select(s => s.Indicator)
            .ToList();
    }
}
