

using System;
using System.Net;
using System.Text.Json.Serialization;

namespace EryfitProxy.Kernel
{
    public class ProxyBindPoint : IEquatable<ProxyBindPoint>
    {
        [JsonConstructor]
        public ProxyBindPoint(EryfitEndPoint endPoint, bool @default)
        {
            EndPoint = endPoint;
            Default = @default;
        }

        public ProxyBindPoint(IPEndPoint endPoint, bool @default)
        {
            EndPoint = endPoint;
            Default = @default;
        }

        /// <summary>
        ///     Combination of an IP address and port number
        /// </summary>
        public EryfitEndPoint EndPoint { get; }

        /// <summary>
        ///     Whether this setting is the default bound address port. When true,
        ///     this setting will be choose as system proxy
        /// </summary>
        public bool Default { get; set; }

        public bool Equals(ProxyBindPoint? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(EndPoint, other.EndPoint) && Default == other.Default;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ProxyBindPoint) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EndPoint, Default);
        }
    }
}
