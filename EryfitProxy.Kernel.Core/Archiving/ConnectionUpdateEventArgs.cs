

using System;

namespace EryfitProxy.Kernel
{
    /// <summary>
    /// Connection update event arguments
    /// </summary>
    public class ConnectionUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Create a new instance from a ConnectionInfo
        /// </summary>
        /// <param name="connection"></param>
        public ConnectionUpdateEventArgs(ConnectionInfo connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// The updated connection
        /// </summary>
        public ConnectionInfo Connection { get; }
    }
}
