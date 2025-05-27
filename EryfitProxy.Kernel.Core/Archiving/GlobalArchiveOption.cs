

using System.Text.Json;
using System.Text.Json.Serialization;
using EryfitProxy.Kernel.Archiving.MessagePack;
using EryfitProxy.Kernel.Misc.Converters;
using EryfitProxy.Kernel.Rules;
using EryfitProxy.Kernel.Rules.Filters;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// Provide serialization settings for producing default archive format
    /// </summary>
    public static class GlobalArchiveOption
    {
        /// <summary>
        /// Storage serializer 
        /// </summary>
        public static MessagePackSerializerOptions MessagePackSerializerOptions { get; } = new(
            CompositeResolver.Create(new IMessagePackFormatter[] { new MessagePackAddressFormatter() },
            new IFormatterResolver[] { StandardResolverAllowPrivate.Instance, ContractlessStandardResolver.Instance }));

        /// <summary>
        /// STJ default archive option 
        /// </summary>
        public static JsonSerializerOptions DefaultSerializerOptions { get; } = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters = {
                new ReadonlyMemoryCharConverter(),
                new BooleanConverter(),
                new JsonStringEnumConverter(),
                new IpAddressConverter(),
                new IpEndPointConverter(),
                new PolymorphicConverter<Filter>(),
                new PolymorphicConverter<Action>()
            },
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        /// <summary>
        /// STJ default archive option  for configuration file
        /// </summary>
        public static JsonSerializerOptions ConfigSerializerOptions { get; } = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            PropertyNameCaseInsensitive = false,
            Converters = {
                new ReadonlyMemoryCharConverter(),
                new BooleanConverter(),
                new JsonStringEnumConverter(),
                new IpAddressConverter(),
                new IpEndPointConverter(),
                new PolymorphicConverter<Filter>(),
                new PolymorphicConverter<Action>()
            },
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        /// <summary>
        /// HAR STJ archiving option, used by Har Packager
        /// </summary>
        public static JsonSerializerOptions HttpArchiveSerializerOptions { get; } = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new ReadonlyMemoryCharConverter(),
                new BooleanConverter(),
                new JsonStringEnumConverter()
            }
        };
        

        /// <summary>
        /// HAR STJ archiving option, used by Har Packager
        /// </summary>
        public static JsonSerializerOptions HttpArchivePrettySerializerOptions { get; } = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new ReadonlyMemoryCharConverter(),
                new BooleanConverter(),
                new JsonStringEnumConverter()
            }
        };
    }
}
