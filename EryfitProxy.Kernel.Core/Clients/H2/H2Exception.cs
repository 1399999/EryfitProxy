

using System;
using EryfitProxy.Kernel.Clients.H2.Frames;

namespace EryfitProxy.Kernel.Clients.H2
{
    public class H2Exception : Exception
    {
        public H2Exception(string message, H2ErrorCode errorCode, Exception? innerException = null)
            :
            base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        ///     Error code according to https://httpwg.org/specs/rfc7540.html#ErrorCodes
        /// </summary>
        public H2ErrorCode ErrorCode { get; }
    }
}
