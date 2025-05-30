namespace EryfitProxy.Kernel.Core.Pcap.Pcapng.Structs
{
    internal readonly ref struct EnhancedPacketBlock
    {
        public const uint BlockTypeValue = 0x00000006;

        private readonly string? _comment;

        public EnhancedPacketBlock(
            int interfaceId,
            uint timestampHigh, uint timestampLow,
            int capturedLength, int originalLength,
            string? comment = null)
        {
            _comment = comment;
            InterfaceId = interfaceId;
            TimestampHigh = timestampHigh;
            TimestampLow = timestampLow;
            CapturedLength = capturedLength;
            OriginalLength = originalLength;

            var packetPaddedLength = capturedLength + (4 - capturedLength % 4) % 4;

            BlockTotalLength = 32 + packetPaddedLength;

            if (!string.IsNullOrWhiteSpace(comment)) {
                BlockTotalLength += OptionHelper.GetOnWireLength(comment);
                BlockTotalLength += 4; // EndOfOption
            }
        }

        public uint BlockType => BlockTypeValue;

        public int BlockTotalLength { get; }

        public int InterfaceId { get; }

        public uint TimestampHigh { get; }

        public uint TimestampLow { get; }

        public int CapturedLength { get; }

        public int OriginalLength { get; }

        public int Write(Span<byte> buffer, ReadOnlySpan<byte> payload)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, BlockType);
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4), BlockTotalLength);
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8), InterfaceId);
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(12), TimestampHigh);
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(16), TimestampLow);
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(20), CapturedLength);
            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(24), OriginalLength);

            payload.CopyTo(buffer.Slice(28));

            var offset = 28 + CapturedLength + (4 - payload.Length % 4) % 4;

            if (!string.IsNullOrWhiteSpace(_comment)) {
                offset += StringOptionBlock.Write(buffer.Slice(offset), OptionBlockCode.Opt_Comment, _comment);
                offset += EndOfOption.DirectWrite(buffer.Slice(offset));
            }

            BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(offset), BlockTotalLength);

            return offset + 4; // Should be block total length 
        }
    }
}
