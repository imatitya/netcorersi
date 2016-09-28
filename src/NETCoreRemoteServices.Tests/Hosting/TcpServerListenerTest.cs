using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NETCoreRemoteServices.Core.Hosting;
using Xunit;

namespace NETCoreRemoteServices.Tests.Hosting
{
    /// <summary>
    /// Responsible for TCP Server Listener Tests
    /// </summary>
    public class TcpServerListenerTest : IDisposable
    {
        private readonly IPAddress m_Ip = IPAddress.Loopback;
        private const int Port = 8000;
        private readonly TcpServerListener m_Target;

        public TcpServerListenerTest()
        {
            // Start the server with a simple 'Increase by one' handler
            m_Target = new TcpServerListener(request =>
            {
                // Increase each byte by one
                for (int i = 0; i < request.Length; ++i)
                {
                    ++request[i];
                }
                return request;
            });
            m_Target.Start(IPAddress.Loopback, 8000);
        }

        /// <summary>
        /// Handle single request test
        /// </summary>
        [Fact]
        public void TcpServerListenerTest_SingleRequest()
        {
            // Arrange
            byte[] request = { 6, 1, 2 };
            byte[] expectedResponse = { 7, 2, 3 };
            byte[] response = new byte[3];
            TcpClient client = new TcpClient();
            client.ConnectAsync(m_Ip, Port).Wait(200);

            // Verify connection
            Assert.True(client.Connected, "Failed to connect to the server");

            // Act
            client.GetStream().Write(request, 0, request.Length);

            // Assert
            client.GetStream().Read(response, 0, response.Length);
            Assert.Equal(expectedResponse, response);
        }

        /// <summary>
        /// Handle multiple requests in parallel test
        /// </summary>
        [Fact]
        public void TcpServerListenerTest_MultipleRequests()
        {
            // Arrange

            byte[] request1 = { 6, 1, 2 };
            byte[] request2 = { 0 };
            byte[] request3 = { 244, 2, 4, 50, 64, 22, 123, 36, 92 };

            // Act
            Task task1 = StartSingleRequestAsync(request1);
            Task task2 = StartSingleRequestAsync(request2);
            Task task3 = StartSingleRequestAsync(request3);

            // Assert
            Task.WaitAll(task1, task2, task3);
        }

        /// <summary>
        /// Start TCP Client request async
        /// </summary>
        /// <param name="request">byte[] to write to stream</param>
        /// <returns></returns>
        private Task StartSingleRequestAsync(byte[] request)
        {
            Task task = new Task(() =>
            {
                // Arrange
                byte[] expectedResponse = new byte[request.Length];
                byte[] response = new byte[request.Length];

                // Prepare the expected result
                for (int i = 0; i < request.Length; ++i)
                {
                    expectedResponse[i] = (byte)(request[i] + 1);
                }

                TcpClient client = new TcpClient();
                client.ConnectAsync(m_Ip, Port).Wait(200);

                // Verify connection
                Assert.True(client.Connected, "Failed to connect to the server");

                // Act
                client.GetStream().Write(request, 0, request.Length);

                // Assert
                client.GetStream().Read(response, 0, response.Length);
                Assert.Equal(expectedResponse, response);
            });

            task.Start();
            return task;
        }

        public void Dispose()
        {
            m_Target.Close();
        }
    }
}