

using System;

namespace EryfitProxy.Kernel
{
    /// <summary>
    ///    Base exception for Eryfit connect error to a remote endpoint 
    /// </summary>
    public class EryfitException : Exception
    {
        public EryfitException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// The target host if any 
        /// </summary>
        public string?  TargetHost { get; set; }
    }
}
