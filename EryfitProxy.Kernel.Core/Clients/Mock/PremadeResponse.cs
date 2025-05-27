

using System.IO;
using EryfitProxy.Kernel.Core;

namespace EryfitProxy.Kernel.Clients.Mock
{
    public abstract class PreMadeResponse
    {
        public abstract string GetFlatH11Header(Authority authority, ExchangeContext? exchangeContext);

        public abstract Stream ReadBody(Authority authority);
    }
}
