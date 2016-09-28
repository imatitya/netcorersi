using System;
using System.Net;

namespace NETCoreRemoteServices.Core.Hosting
{
    /// <summary>
    /// The container which hosts all registered services (contracts) that will be available for remote invocation calls.
    /// </summary>
    public interface IRemoteServiceContainer : IDisposable
    {
        //TODO implement (look for implementation in loaded assembly) void RegisterService(Type interfaceType);

        /// <summary>
        /// Register the provided service
        /// </summary>
        /// <param name="interfaceType">Service contract type</param>
        /// <param name="instance">Service implementation to register</param>
        void RegisterService(Type interfaceType, object instance);

        /// <summary>
        /// Register the provided service
        /// </summary>
        /// <param name="interfaceType">Service contract type</param>
        /// <param name="implementationType">Service implementation type to register.</param>
        void RegisterService(Type interfaceType, Type implementationType);

        /// <summary>
        /// Start hosting services with the provided IP. 
        /// Localhost + Port 8000 will be set by default.
        /// </summary>
        void Open();

        /// <summary>
        /// Start hosting services with the provided IP. 
        /// Port 8000 will be set by default.
        /// </summary>
        /// <param name="ip">IP Address</param>
        void Open(IPAddress ip);

        /// <summary>
        /// Start hosting services with the provided IP and port. 
        /// </summary>
        /// <param name="ip">IP Address</param>
        /// <param name="port">port</param>
        void Open(IPAddress ip, int port);
    } 
}