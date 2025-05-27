

using System;
using EryfitProxy.Kernel.Clients.H2.Encoder;
using EryfitProxy.Kernel.Misc.ResizableBuffers;

namespace EryfitProxy.Kernel.Clients.H2
{
    internal interface IHeaderEncoder
    {
        HPackEncoder Encoder { get; }

        HPackDecoder Decoder { get; }

        /// <summary>
        ///     InternalApply header + hpack to headerbuffer
        /// </summary>
        /// <param name="encodingJob"></param>
        /// <param name="destinationBuffer"></param>
        /// <param name="endStream"></param>
        /// <returns></returns>
        ReadOnlyMemory<byte> Encode(HeaderEncodingJob encodingJob, RsBuffer destinationBuffer, bool endStream);

        /// <summary>
        ///     Remove hpack
        /// </summary>
        /// <param name="encodedBuffer"></param>
        /// <param name="destinationBuffer"></param>
        /// <returns></returns>
        ReadOnlyMemory<char> Decode(ReadOnlyMemory<byte> encodedBuffer, Memory<char> destinationBuffer);
    }
}
