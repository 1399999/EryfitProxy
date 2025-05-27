

using System;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// Downstream error event arguments
    /// </summary>
    public class DownstreamErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public DownstreamErrorEventArgs(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Error count
        /// </summary>
        public int Count { get; }
    }
}
