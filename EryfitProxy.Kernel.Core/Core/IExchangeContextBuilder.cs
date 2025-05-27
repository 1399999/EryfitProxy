using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Core
{
    internal interface IExchangeContextBuilder
    {
        ValueTask<ExchangeContext> Create(Authority authority, bool secure);
    }
}