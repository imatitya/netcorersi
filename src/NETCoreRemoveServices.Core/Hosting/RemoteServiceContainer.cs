using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using NETCoreRemoveServices.Core.Reflection;
using System.Linq;

namespace NETCoreRemoveServices.Core.Hosting
{
    public class RemoteServiceContainer : IRemoteServiceContainer
    {
        private readonly IDictionary<Type, object> m_RegisteredServices;
        private IServerListener m_ServerListener;

        /// <summary>
        /// Initialize new instance of RemoteServiceContainer
        /// </summary>
        public RemoteServiceContainer()
        {
            m_RegisteredServices = new ConcurrentDictionary<Type, object>();
        }

        public void RegisterService(Type interfaceType, Type implementationType)
        {
            RegisterService(interfaceType, Activator.CreateInstance(implementationType));
        }

        public void RegisterService(Type interfaceType, object instance)
        {
            //TODO validate type is interface and assignable from provider instance is  from this type
            m_RegisteredServices[interfaceType] = instance;
        }


        public void Open()
        {
            Open(IPAddress.Loopback);
        }

        public void Open(IPAddress ip)
        {
            Open(ip, 8000);
        }

        public void Open(IPAddress ip, int port)
        {
            if(m_ServerListener != null)
            {
                throw new InvalidOperationException("Server was already started.");
            }
            m_ServerListener = new TcpServerListener(OnServiceRequest);
            m_ServerListener.Start(ip, port);

        }

        public void Dispose()
        {
            m_ServerListener?.Close();
        }

        /// <summary>
        /// Server listener callback
        /// </summary>
        /// <param name="buffer">buffer that contains the client request data</param>
        /// <returns>server response represented by byte array</returns>
        private byte[] OnServiceRequest(byte[] buffer)
        {
            try
            {
                return InnerOnServiceRequest(buffer);
            }
            catch (TargetInvocationException e)
            {
                return e.InnerException != null
                    ? Serializer.SerializeObject(new JResponse(e.InnerException))
                    : Serializer.SerializeObject(new JResponse(e));
            }
            catch (Exception e)
            {
                return Serializer.SerializeObject(new JResponse(e));
            }
            
        }

        private byte[] InnerOnServiceRequest(byte[] buffer)
        {
            // Translate data bytes to a JMethodInfo object.
            JMethodInfo jMethodInfo = Serializer.DeserializeObject<JMethodInfo>(buffer);

            // Get register service
            object instance = m_RegisteredServices.FirstOrDefault(x => x.Key.FullName == jMethodInfo.DeclaringType.FullName).Value;
            if (instance == null)
            {
                throw new InvalidOperationException($"Cannot find requested service {jMethodInfo.DeclaringType.FullName}");
            }

            // Get method parameters and parameters value
            Type[] parameterTypes = jMethodInfo.Parameters.Select(p => Type.GetType(p.ParameterType.AssemblyQualifiedName)).ToArray();
            object[] parameters = jMethodInfo.Parameters.Select(p => p.Value).ToArray();

            // Make sure all parameters represented in the expected type
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].GetType() != parameterTypes[i])
                {
                    parameters[i] = Serializer.DeserializeObject(parameters[i], parameterTypes[i]);
                }
            }

            // Get service method to invoke
            MethodInfo methodToInvoke = instance.GetType().GetMethod(jMethodInfo.Name, parameterTypes);
            if (methodToInvoke == null)
            {
                throw new InvalidOperationException($"Cannot find requested method {jMethodInfo.Name}");
            }

            // Invoke service method
            object returnValue = methodToInvoke.Invoke(instance, parameters);

            // return method response as byte array or empty array in the return type is void
            return returnValue == null ? Serializer.SerializeObject(new JResponse()) : Serializer.SerializeObject(new JResponse(returnValue));
        }
    }
}