namespace EryfitProxy.Kernel.Core.Pcap
{
    /// <summary>
    /// A capture check using Pcap instance 
    /// </summary>
    public class CaptureAvailabilityChecker : ICaptureAvailabilityChecker
    {
        public bool IsAvailable {
            get
            {
                try {
                    return CaptureDeviceList.Instance.OfType<PcapDevice>().Any();
                }
                catch {
                    // We ignore pcap device listing error 
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieve the current instance
        /// </summary>
        public static CaptureAvailabilityChecker Instance { get; } = new CaptureAvailabilityChecker();
    }
}
