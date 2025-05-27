

namespace EryfitProxy.Kernel.Core.Breakpoints
{
    public interface IBreakPoint
    {
        BreakPointLocation Location { get; }

        void Continue();

        bool? Running { get; }
    }
}
