

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using EryfitProxy.Kernel.Extensions;
using EryfitProxy.Kernel.Misc;
using MessagePack;

namespace EryfitProxy.Kernel.Readers
{
    /// <summary>
    ///  An archive reader that reads from a eryfit archive file
    /// </summary>
    public class EryfitArchiveReader : IArchiveReader
    {
        private readonly ZipArchive _zipFile;

        public EryfitArchiveReader(string filePath)
        {
            try {
                _zipFile = ZipFile.OpenRead(filePath);
            }
            catch (Exception ex) {
                throw new Exception("Invalid eryfit archive file", ex);
            }
        }

        public EryfitArchiveReader(Stream stream)
        {
            _zipFile = new ZipArchive(stream, ZipArchiveMode.Read);
        }

        public ArchiveMetaInformation ReadMetaInformation()
        {
            var metaEntry = _zipFile.Entries.FirstOrDefault(e => e.FullName.EndsWith("meta.json"));

            if (metaEntry == null) {
                return new ArchiveMetaInformation();
            }

            using var metaStream = metaEntry.Open();

            return JsonSerializer.Deserialize<ArchiveMetaInformation>(metaStream,
                GlobalArchiveOption.DefaultSerializerOptions)!;
        }

        public IEnumerable<ExchangeInfo> ReadAllExchanges()
        {
            return _zipFile.Entries.Where(e =>
                               e.FullName.StartsWith("exchanges")
                               && e.FullName.EndsWith(".mpack"))
                           .Select(s => {
                               using var stream = s.Open();

                               var result = MessagePackSerializer.Deserialize<ExchangeInfo>(stream,
                                   GlobalArchiveOption.MessagePackSerializerOptions);

                               return result;
                           })
                           .Where(t => t != null)
                           .Select(t => t!);
        }

        public ExchangeInfo? ReadExchange(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetExchangePath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            using var stream = entry.Open();

            return MessagePackSerializer.Deserialize<ExchangeInfo>(stream,
                GlobalArchiveOption.MessagePackSerializerOptions);
        }

        public IEnumerable<ConnectionInfo> ReadAllConnections()
        {
            return _zipFile.Entries.Where(e =>
                               e.FullName.StartsWith("connections")
                               && e.FullName.EndsWith(".mpack"))
                           .Select(s => {
                               using var stream = s.Open();

                               return MessagePackSerializer.Deserialize<ConnectionInfo>(
                                   stream,
                                   GlobalArchiveOption.MessagePackSerializerOptions);
                           })
                           .Where(t => t != null)
                           .Select(t => t!);
        }

        public ConnectionInfo? ReadConnection(int connectionId)
        {
            var path = DirectoryArchiveHelper.GetConnectionPath(string.Empty, connectionId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            using var stream = entry.Open();

            return MessagePackSerializer.Deserialize<ConnectionInfo>(
                stream,
                GlobalArchiveOption.MessagePackSerializerOptions);
        }

        public IReadOnlyCollection<DownstreamErrorInfo> ReaderAllDownstreamErrors()
        {
            var path = DirectoryArchiveHelper.GetErrorPath(string.Empty);
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return Array.Empty<DownstreamErrorInfo>();
            }

            using var stream = entry.Open();

            return stream.DeserializeMultiple<DownstreamErrorInfo>(GlobalArchiveOption.MessagePackSerializerOptions)
                         .ToList();
        }

        public Stream? GetRawCaptureStream(int connectionId)
        {
            var path = Path.Combine("captures", $"{connectionId}.pcapng").Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public Stream? GetRawCaptureKeyStream(int connectionId)
        {
            var path = Path.Combine("captures", $"{connectionId}.nsskeylog").Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public Stream? GetRequestBody(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentRequestPath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public long GetRequestBodyLength(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentRequestPath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return -1;
            }

            return entry.Length;
        }

        public long GetResponseBodyLength(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentResponsePath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return 0;
            }

            return entry.Length;
        }

        public Stream? GetRequestWebsocketContent(int exchangeId, int messageId)
        {
            var path = DirectoryArchiveHelper.GetWebsocketContentRequestPath(string.Empty, exchangeId, messageId)
                                             .Replace("\\", "/");

            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public Stream? GetResponseWebsocketContent(int exchangeId, int messageId)
        {
            var path = DirectoryArchiveHelper.GetWebsocketContentResponsePath(string.Empty, exchangeId, messageId)
                                             .Replace("\\", "/");

            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public bool HasRequestBody(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentRequestPath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            return entry != null;
        }

        public Stream? GetResponseBody(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentResponsePath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);
            
            if (entry == null) {
                return null;
            }

            return entry.Open();
        }

        public Stream? GetDecodedRequestBody(int exchangeId)
        {
            var exchangeInfo = ReadExchange(exchangeId);

            if (exchangeInfo == null)
                throw new InvalidOperationException($"Exchange {exchangeId} not found on this archive");

            var originalStream = GetRequestBody(exchangeInfo.Id);

            if (originalStream == null)
                return null;

            return exchangeInfo.GetDecodedRequestBodyStream(originalStream, out _);
        }

        public Stream? GetDecodedResponseBody(int exchangeId)
        {
            var exchangeInfo = ReadExchange(exchangeId);

            if (exchangeInfo == null)
                throw new InvalidOperationException($"Exchange {exchangeId} not found on this archive");

            var originalStream = GetResponseBody(exchangeInfo.Id);

            if (originalStream == null)
                return null;

            return exchangeInfo.GetDecodedResponseBodyStream(originalStream, out _, true);
        }

        public bool HasResponseBody(int exchangeId)
        {
            var path = DirectoryArchiveHelper.GetContentResponsePath(string.Empty, exchangeId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            return entry != null;
        }

        public bool HasCapture(int connectionId)
        {
            var path = DirectoryArchiveHelper.GetCapturePath(string.Empty, connectionId).Replace("\\", "/");
            var entry = _zipFile.Entries.FirstOrDefault(e => e.FullName == path);

            return entry != null;
        }

        public IEnumerable<ArchiveAsset> GetAssetsByExchange(int exchangeId)
        {
            var matchingEntries = _zipFile.Entries.Where(e =>
                e.Name.StartsWith($"res-{exchangeId}.")
                || e.Name.StartsWith($"res-{exchangeId}-")
                || e.Name.StartsWith($"req-{exchangeId}.")
                || e.Name.StartsWith($"req-{exchangeId}-")
                || e.Name.StartsWith($"ex-{exchangeId}.")
            );

            foreach (var entry in matchingEntries) {
                yield return new ArchiveAsset(entry.FullName, entry.Length, null, () => entry.Open());
            }
        }

        public IEnumerable<ArchiveAsset> GetAssetsByConnection(int connectionId)
        {
            var matchingEntries = _zipFile.Entries.Where(e =>
                e.Name.StartsWith($"con-{connectionId}.")
                || e.Name.StartsWith($"{connectionId}.pcap")
                || e.Name.StartsWith($"{connectionId}.nsskeylog")
            );

            foreach (var entry in matchingEntries)
            {
                yield return new ArchiveAsset(entry.FullName, entry.Length, null, () => entry.Open());
            }
        }

        public void Dispose()
        {
            _zipFile?.Dispose();
        }
    }
}
