

using System;

namespace EryfitProxy.Kernel.Core
{
    public class ClientErrorException : Exception
    {
        public ClientErrorException(int errorCode, string message, string? innerMessageException = null,
            Exception ? innerException = null)
            : base(message, innerException)
        {
            ClientError = new ClientError(errorCode, message) {
                ExceptionMessage = innerMessageException
            };
        }

        public ClientError ClientError { get; }
    }
}
