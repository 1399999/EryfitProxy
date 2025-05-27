

using System;

namespace EryfitProxy.Kernel.Clients.H2.Encoder.HPack
{
    public class HPackCodecException : Exception
    {
        public HPackCodecException(string message)
            : base(message)
        {
        }
    }
}
