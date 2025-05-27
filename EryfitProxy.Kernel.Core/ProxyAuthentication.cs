

namespace EryfitProxy.Kernel
{
    public class ProxyAuthentication
    {
        public ProxyAuthentication(ProxyAuthenticationType type)
        {
            Type = type;
        }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public ProxyAuthenticationType Type { get;  }

        public static ProxyAuthentication Basic(string username, string password) 
            => new(ProxyAuthenticationType.Basic)
        {
            Username = username,
            Password = password
        };

        public static ProxyAuthentication None() => new(ProxyAuthenticationType.None);
    }
}
