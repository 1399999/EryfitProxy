

using System;

namespace EryfitProxy.Kernel.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public class ExchangeException : Exception
    {
        public ExchangeException(string message, Exception? innerException = null)
            :
            base(message, innerException)
        {
        }
    }
}
