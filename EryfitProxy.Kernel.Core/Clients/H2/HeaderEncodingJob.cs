

using System;

namespace EryfitProxy.Kernel.Clients.H2
{
    public readonly ref struct HeaderEncodingJob
    {
        public HeaderEncodingJob(ReadOnlyMemory<char> data, int streamIdentifier, int streamDependency)
        {
            Data = data;
            StreamIdentifier = streamIdentifier;
            StreamDependency = streamDependency;
        }

        public ReadOnlyMemory<char> Data { get; }

        public int StreamIdentifier { get; }

        public int StreamDependency { get; }
    }
}
