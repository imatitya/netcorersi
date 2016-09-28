using System.Net;

namespace NETCoreRemoteServices.Core.Channel.Consumers
{
    /// <summary>
    /// Channel consumer API
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Address of server to consume
        /// </summary>
        IPAddress ServerIp { get; }

        /// <summary>
        /// Port of server to consume
        /// </summary>
        int ServerPort { get; }

        /// <summary>
        /// Consume service from the server
        /// </summary>
        /// <param name="data">Consumer request represented by byte array</param>
        /// <returns>Server response represented by byte array</returns>
        byte[] ConsumeService(byte[] data);
    }
}