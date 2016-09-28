using NETCoreRemoteServices.Core.Channel.Consumers;
using System.Net;
using Castle.DynamicProxy;

namespace NETCoreRemoteServices.Core.Channel
{
    public class ServiceChannel : IServiceChannel
    {
        private readonly ChannelInterceptor m_RemoteInvocationInterceptor;

        /// <summary>
        /// Initialize new instance of ServiceChannel  
        /// </summary>
        /// <param name="ip">IP Address</param>
        /// <param name="port">port</param>
        public ServiceChannel(IPAddress ip, int port)
        {
            m_RemoteInvocationInterceptor = new ChannelInterceptor(new TcpClientConsumer(ip, port));
        }

        public T GetRemoteService<T>()
        {
            // Initialize proxy generator
            var generator = new ProxyGenerator();

            // Create service proxy
            return (T)generator.CreateInterfaceProxyWithoutTarget(typeof(T), m_RemoteInvocationInterceptor);

            //TODO consider validate listener using inner service
        }
    }
}