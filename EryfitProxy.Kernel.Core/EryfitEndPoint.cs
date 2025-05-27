

using System;
using System.Net;
using System.Text.Json.Serialization;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// A eryfit bound endpoint
    /// </summary>
    public class EryfitEndPoint : IEquatable<EryfitEndPoint>
    {
        /// <summary>
        /// Create from a provided address and port
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        [JsonConstructor]
        public EryfitEndPoint(string address, int port)
        {
            Address = address;
            Port = port;
        }

        /// <summary>
        /// Create from an IPEndPoint
        /// </summary>
        /// <param name="endPoint"></param>
        public EryfitEndPoint(IPEndPoint endPoint)
            : this(endPoint.Address.ToString(), endPoint.Port)
        {
        }

        /// <summary>
        /// End point address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Endpoint port 
        /// </summary>
        public int Port { get; set; }

        public bool Equals(EryfitEndPoint? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Address == other.Address && Port == other.Port;
        }

        public IPEndPoint ToIpEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }

        public static implicit operator IPEndPoint(EryfitEndPoint d)
        {
            return d.ToIpEndPoint();
        }

        public static implicit operator EryfitEndPoint(IPEndPoint d)
        {
            return new(d);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((EryfitEndPoint) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Address, Port);
        }
    }
}
