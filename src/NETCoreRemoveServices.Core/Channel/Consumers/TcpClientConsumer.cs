using System.Net;
using System.Net.Sockets;

namespace NETCoreRemoveServices.Core.Channel.Consumers
{
    /// <summary>
    /// TCP Based client consumer
    /// </summary>
    public class TcpClientConsumer : IConsumer
    {
        public IPAddress ServerIp { get; }
        public int ServerPort { get; }

        /// <summary>
        /// Initialize new instance of TCP Client Consumer
        /// </summary>
        /// <param name="ip">IP address of the remote server</param>
        /// <param name="port">port</param>
        public TcpClientConsumer(IPAddress ip, int port)
        {
            ServerIp = ip;
            ServerPort = port;
        }

        public byte[] ConsumeService(byte[] data)
        {
            // Create a TcpClient.
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
            client.ConnectAsync(ServerIp, ServerPort).Wait();

            // Get a client stream for reading and writing.
            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            // Read the TcpServer response bytes.
            byte[] response = new byte[8096];
            stream.Read(response, 0, response.Length);

            // Dispose.
            stream.Dispose();
            client.Dispose();

            return response;
        }
    }
}