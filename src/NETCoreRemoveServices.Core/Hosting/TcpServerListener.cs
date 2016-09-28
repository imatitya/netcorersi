using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NETCoreRemoteServices.Core.Hosting
{
    /// <summary>
    /// TCP Async server listener
    /// </summary>
    internal class TcpServerListener : IServerListener
    {
        /// <summary>
        /// Amount of client handlers that run in parallel
        /// </summary>
        private readonly int m_NumOfParallelHandlers;

        /// <summary>
        /// Handlers cancellation flag
        /// </summary>
        private bool m_IsCancellationRequested;
        private TcpListener m_TcpListener;
        
        public Func<byte[], byte[]> HandlerCallback { get; }

        /// <summary>
        /// Initialize new instance of TCP Server Listener
        /// </summary>
        /// <param name="handlerCallback">Inject handler callback</param>
        /// <param name="numOfParallelHandlers">Amount of handlers to start in parallel. default value is 10.</param>
        public TcpServerListener(Func<byte[], byte[]> handlerCallback, int numOfParallelHandlers = 10)
        {
            HandlerCallback = handlerCallback;
            m_NumOfParallelHandlers = numOfParallelHandlers;
        }

                public void Start(IPAddress ip, int port)
        {
            // Initialize new instance of TCP Listener
            m_TcpListener = new TcpListener(ip, port);

            // Start listening for client requests.
            m_TcpListener.Start();

            // Get client requests.
            for(int i=0; i< m_NumOfParallelHandlers; ++i)
            {
                m_TcpListener.AcceptTcpClientAsync().ContinueWith(ClientHandler);
            }
        }

                public void Close()
        {
            m_IsCancellationRequested = true;
            m_TcpListener?.Stop();
        }

        /// <summary>
        /// Client handler
        /// </summary>
        /// <param name="task">Associated TcpClient task</param>
        private void ClientHandler(Task<TcpClient> task)
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Server closed
                return;
            }
            TcpClient client = task.Result;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            // TODO this is the max number of request size! consider extract this value to configuration file \ inject via C'tor \ Configure using contract attribute
            byte[] buffer = new byte[8096];

            // Loop to receive all the data sent by the client.
            do
            {
                stream.Read(buffer, 0, buffer.Length);
            }
            while (stream.DataAvailable);

            // Pass the request to the handler callback and get the response
            byte[] res = HandlerCallback(buffer);

            // Write the response to client stream
            stream.Write(res, 0, res.Length);

            // Shutdown and end connection
            client.Dispose();

            // Start new handler for next client request
            if(!m_IsCancellationRequested)
            {
                m_TcpListener.AcceptTcpClientAsync().ContinueWith(ClientHandler);
            }
        }
    }
}