

using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Clients.Headers
{
    public class HeaderAlterationReplace : HeaderAlteration
    {
        public HeaderAlterationReplace(string headerName, string headerValue, bool addIfMissing)
        {
            HeaderName = headerName;
            HeaderValue = headerValue;
            AddIfMissing = addIfMissing;
        }

        public string HeaderName { get; }

        public string HeaderValue { get; }

        public bool AddIfMissing { get; }

        public string ? AppendSeparator { get; set; }

        public override void Apply(Header header)
        {
            header.AltReplaceHeaders(HeaderName, HeaderValue, AddIfMissing, AppendSeparator);
        }
    }
}
