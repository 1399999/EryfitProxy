

using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Clients.Headers
{
    public class HeaderAlterationDelete : HeaderAlteration
    {
        public HeaderAlterationDelete(string headerName)
        {
            HeaderName = headerName;
        }

        public string HeaderName { get; }

        public override void Apply(Header header)
        {
            header.AltDeleteHeader(HeaderName);
        }
    }
}
