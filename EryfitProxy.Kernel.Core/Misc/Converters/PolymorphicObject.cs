

using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace EryfitProxy.Kernel.Misc.Converters
{
    public abstract class PolymorphicObject
    {
        [JsonIgnore]
        [YamlIgnore]
        protected abstract string Suffix { get; }

        public string TypeKind => GetType().Name;
    }
}
