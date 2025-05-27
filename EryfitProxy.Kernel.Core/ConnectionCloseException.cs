

using System;

namespace EryfitProxy.Kernel
{
    public class ConnectionCloseException : EryfitException
    {
        public ConnectionCloseException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
