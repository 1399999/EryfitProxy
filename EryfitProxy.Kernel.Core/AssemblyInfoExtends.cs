

using System.ComponentModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EryfitProxy.Kernel")]
[assembly: InternalsVisibleTo("Eryfit.Desktop.Services")]
[assembly: InternalsVisibleTo("Eryfit.Desktop.Ui")]
[assembly: InternalsVisibleTo("Eryfit.Encoding.Tests")]
[assembly: InternalsVisibleTo("Eryfit.Bulk.BcCli")]
[assembly: InternalsVisibleTo("EryfitProxy.Kernel.Tests")]
[assembly: InternalsVisibleTo("EryfitProxy.Kernel.Core.Pcap.Cli")]
[assembly: InternalsVisibleTo("Eryfit.Kernel.Core.Pcap.Cli")]
[assembly: InternalsVisibleTo("Eryfit.Kernel.Core.Pcap")]
[assembly: InternalsVisibleTo("Eryfit.Exec")]

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit
    {
    }
}
