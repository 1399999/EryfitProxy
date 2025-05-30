

using MessagePack;
using System;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// A tag is an indexed string that can be attached to an exchange
    /// </summary>
    [MessagePackObject]
    public class Tag : IEquatable<Tag>
    {
        internal Tag(string value)
        {
            Identifier = Guid.NewGuid();
            Value = value;
        }

        public Tag(Guid identifier, string value)
        {
            Identifier = identifier;
            Value = value;
        }

        /// <summary>
        /// An unique identifier
        /// </summary>
        [Key(0)]
        public Guid Identifier { get; }

        /// <summary>
        /// The tag value 
        /// </summary>
        [Key(1)]
        public string Value { get; }

        public bool Equals(Tag? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Identifier.Equals(other.Identifier) && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Tag) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identifier, Value);
        }

        public override string ToString()
        {
            return $"{Value} ({Identifier})";
        }
    }
}
