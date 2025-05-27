

namespace EryfitProxy.Kernel
{
    /// <summary>
    ///    Check if the capture is available
    /// </summary>
    public interface ICaptureAvailabilityChecker
    {
        /// <summary>
        /// True if raw capture can be enable
        /// </summary>
        bool IsAvailable { get; }
    }
}
