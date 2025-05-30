

using System;
using System.Buffers;

namespace EryfitProxy.Kernel.Misc.ResizableBuffers
{
    public class RsBuffer : IDisposable
    {
        private RsBuffer(byte[] buffer)
        {
            Buffer = buffer;
        }

        public byte[] Buffer { get; private set; }

        public Memory<byte> Memory => new(Buffer);

        public static RsBuffer Allocate(int size)
        {
            var rawBuffer = ArrayPool<byte>.Shared.Rent(size);

            var result = new RsBuffer(rawBuffer);

            return result;
        }

        public void Multiply(int size)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size)); 

            Extend(Buffer.Length * size - Buffer.Length);
        }

        public void Extend(int extensionLength)
        {
            if (extensionLength < 0)
                throw new ArgumentOutOfRangeException(nameof(extensionLength));

            if (extensionLength == 0)
                return;

            var forecastLength = Buffer.Length + extensionLength;

            if (forecastLength > EryfitSharedSetting.MaxProcessingBuffer)
                throw new ArgumentOutOfRangeException(nameof(extensionLength), 
                    $@"{nameof(EryfitSharedSetting.MaxProcessingBuffer)} was reached");

            var newBuffer = ArrayPool<byte>.Shared.Rent(forecastLength);

            System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Buffer.Length);
            ArrayPool<byte>.Shared.Return(Buffer);

            Buffer = newBuffer;
        }

        public void Dispose()
        {
            ArrayPool<byte>.Shared.Return(Buffer);
        }
    }
}
