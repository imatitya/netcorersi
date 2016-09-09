using System;
using System.Net;

namespace NETCoreRemoveServices.Core.Hosting
{
    /// <summary>
    /// Server listener
    /// </summary>
    internal interface IServerListener
    {
        /// <summary>
        /// Handler callback.
        /// Each handler will pass the client byte[] request to this callback.
        /// The callback return value byte[] will be passed back to the client.
        /// </summary>
        Func<byte[], byte[]> HandlerCallback { get; }

        /// <summary>
        /// Start the server
        /// </summary>
        /// <param name="ip">IP Address</param>
        /// <param name="port">port</param>
        void Start(IPAddress ip, int port);

        /// <summary>
        /// Close the server
        /// </summary>
        void Close();
    }
}