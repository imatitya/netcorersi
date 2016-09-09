using System;
using System.Diagnostics;
using NETCoreRemoteServices.Tests.Implementation;
using NETCoreRemoveServices.Core.Channel;
using NETCoreRemoveServices.Core.Hosting;
using System.Net;
using System.Threading.Tasks;
using Contract;
using Xunit;

namespace NETCoreRemoteServices.Tests
{
    /// <summary>
    /// Responsible for client-server integration tests
    /// </summary>
    public class ClientServerTest : IDisposable
    {
        private readonly RemoteServiceContainer m_RemoteServiceContainer;
        private readonly IMyCustomService m_ProxyTarget;
        private readonly IMyCustomService m_LocalTarget;

        /// <summary>
        /// Class initialize
        /// </summary>
        public ClientServerTest()
        {
            // Configure IP and port
            IPAddress localhost = IPAddress.Loopback;
            int port = 8000;

            // Start server 
            m_RemoteServiceContainer = new RemoteServiceContainer();
            m_RemoteServiceContainer.RegisterService(typeof(IMyCustomService), new MyCustomService());
            m_RemoteServiceContainer.Open(localhost, port);

            // Get service proxy in client side
            var servicesChannel = new ServiceChannel(localhost, port);
            m_ProxyTarget = servicesChannel.GetRemoteService<IMyCustomService>();

            // Initialize the actual object locally. The tests may use this object as 'expected' value during assertion
            m_LocalTarget = new MyCustomService();
        }

        /// <summary>
        /// Class finalize
        /// </summary>
        public void Dispose()
        {
            m_RemoteServiceContainer.Dispose();
        }

        /// <summary>
        /// Make sure our server can handle multiple requests in parallel. 
        /// Otherwise, WaitAll will throw an exception
        /// </summary>
        [Fact]
        public void ClientServerTest_ParallelTest()
        {
            int numOfCalls = 4;
            Task[] tasks = new Task[numOfCalls];
            for (int i = 0; i < numOfCalls; ++i)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Assert.Equal(5, m_ProxyTarget.Sum(2, 3));
                });
            }
            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Make sure 100 calls are handled in less than 1 second.
        /// </summary>
        [Fact]
        public void ClientServerTest_PerformanceTest()
        {
            DateTime t = DateTime.Now;
            int numOfCalls = 100;
            for (int i = 0; i < numOfCalls; ++i)
            {
                m_ProxyTarget.DoSomething();
            }
            Assert.True(TimeSpan.FromSeconds(1).TotalMilliseconds > (DateTime.Now - t).TotalMilliseconds);
        }

        /// <summary>
        /// Make sure our server responded with the expected single line response
        /// </summary>
        [Fact]
        public void ClientServerTest_SingleLineString()
        {
            Assert.Equal(m_LocalTarget.GetMachineName(), m_ProxyTarget.GetMachineName());
        }

        /// <summary>
        /// Make sure our server responded with the expected multiple lines response
        /// </summary>
        [Fact]
        public void ClientServerTest_MultipleLines()
        {
            Assert.Equal(m_LocalTarget.GetEnvironmentVariables(), m_ProxyTarget.GetEnvironmentVariables());
        }

        /// <summary>
        /// Make sure our server supports method overloading
        /// </summary>
        [Fact]
        public void ClientServerTest_MethodOverload()
        {
            int x = 20;
            int y = 30;
            Assert.Equal(x + y, m_ProxyTarget.Sum(x, y));

            long longX = 20000000;
            long longY = 30000000;
            Assert.Equal(longX + longY, m_ProxyTarget.Sum(longX, longY));
        }

        /// <summary>
        /// Test Non primitive custom object (must be JSON serializeable)
        /// </summary>
        [Fact]
        public void ClientServerTest_CustomObject()
        {
            Assert.Equal(m_LocalTarget.GetAuthor(), m_ProxyTarget.GetAuthor());
        }

        /// <summary>
        /// Test Non primitive .NET object (must be JSON serializeable)
        /// </summary>
        [Fact]
        public void ClientServerTest_DotNetObject()
        {
            Assert.True(m_ProxyTarget.NonPrimitive(new ProcessStartInfo { FileName = MyCustomService.ExpectedFileName }));
        }

        /// <summary>
        /// Test Non primitive string representation of JObject.
        /// This test validates that there are no serialization issues with embedded JObject.
        /// </summary>
        [Fact]
        public void ClientServerTest_EmbeddedJObject()
        {
            Assert.Equal(m_LocalTarget.EmbeddedJObject(), m_ProxyTarget.EmbeddedJObject());
        }

        
    }
}
