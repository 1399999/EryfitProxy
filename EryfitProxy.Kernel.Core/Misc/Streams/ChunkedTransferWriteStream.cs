

using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EryfitProxy.Kernel.Misc.Streams
{
    /// <summary>
    ///     A stream that write chunked transfer encoding
    /// </summary>
    public class ChunkedTransferWriteStream : Stream
    {
        private static readonly byte[] ChunkTerminator =
            { (byte) '0', (byte) '\r', (byte) '\n', (byte) '\r', (byte) '\n' };

        private static readonly byte[] LineTerminator = { (byte) '\r', (byte) '\n' };
        private readonly Stream _innerStream;

        private bool _eof;

        public ChunkedTransferWriteStream(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => !_eof;

        public override long Length => throw new NotSupportedException();

        public override long Position {
            get => throw new NotSupportedException();

            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var poolBuffer = ArrayPool<byte>.Shared.Rent(64);

            try {
                var cs = Encoding.ASCII.GetBytes($"{count:X}\r\n", poolBuffer);
                _innerStream.Write(poolBuffer, 0, cs);
                _innerStream.Write(buffer, offset, count);
                _innerStream.Write(LineTerminator);
            }
            finally {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).ConfigureAwait(false);
        }

        public override async ValueTask WriteAsync(
            ReadOnlyMemory<byte> buffer,
            CancellationToken cancellationToken = new())
        {
            var poolBuffer = ArrayPool<byte>.Shared.Rent(64);

            try {
                var cs = Encoding.ASCII.GetBytes($"{buffer.Length:X}\r\n", poolBuffer);
                await _innerStream.WriteAsync(new ReadOnlyMemory<byte>(poolBuffer, 0, cs), cancellationToken).ConfigureAwait(false);
                await _innerStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
                await _innerStream.WriteAsync(new ReadOnlyMemory<byte>(LineTerminator), cancellationToken).ConfigureAwait(false);
            }
            finally {
                ArrayPool<byte>.Shared.Return(poolBuffer);
            }
        }

        public ValueTask WriteEof()
        {
            if (!_eof) {
                _eof = true;

                return _innerStream.WriteAsync(ChunkTerminator);
            }

            return default;
        }
    }
}
