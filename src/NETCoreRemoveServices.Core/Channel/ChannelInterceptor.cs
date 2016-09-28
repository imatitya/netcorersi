using System;
using System.Reflection;
using Castle.DynamicProxy;
using NETCoreRemoteServices.Core.Channel.Consumers;
using NETCoreRemoteServices.Core.Reflection;

namespace NETCoreRemoteServices.Core.Channel
{
    /// <summary>
    /// Channel interceptor. 
    /// Responsible for serializing the request in client context, 
    /// pass it to consumer and deserialize the server response provided by the consumer.
    /// </summary>
    internal class ChannelInterceptor : IInterceptor
    {
        private readonly IConsumer m_Consumer;

        /// <summary>
        /// Initialize new instance of Channel Interceptor
        /// </summary>
        /// <param name="consumer">Instance of the consumer</param>
        public ChannelInterceptor(IConsumer consumer)
        {
            m_Consumer = consumer;
        }

        public void Intercept(IInvocation invocation)
        {
            // Create JSON representation for MethodInfo object
            JMethodInfo info = new JMethodInfo(invocation.Method, invocation.Arguments);

            // Serialize the JSON to byte array
            byte[] data = Serializer.SerializeObject(info);

            // Consume service using the injected consumer
            byte[] response = m_Consumer.ConsumeService(data);

            // Get the JSON representation for the server response
            JResponse jResponse = Serializer.DeserializeObject<JResponse>(response);
            switch (jResponse.ResponseType)
            {
                case ResponseType.Empty:
                    return;
                case ResponseType.Success:
                    // If expected ReturnType is not void, assign the return value
                    if (invocation.Method.ReturnType != typeof(void))
                    {
                        // Make sure return type is equal to response value type
                        if (invocation.Method.ReturnType != jResponse.ReturnValue.Value.GetType())
                        {
                            // if not, deserialize the value to this type explicitly 
                            jResponse.ReturnValue.Value = Serializer.DeserializeObject(jResponse.ReturnValue.Value, invocation.Method.ReturnType);
                        }

                        // Set the return value
                        invocation.ReturnValue = jResponse.ReturnValue.Value;
                    }
                    break;
                case ResponseType.Fault:
                    JException jException = Serializer.DeserializeObject<JException>(jResponse.ReturnValue.Value);
                    if (jException != null)
                    {
                        throw jException;
                    }
                    throw new TargetInvocationException(null);
                default:
                    throw new ArgumentOutOfRangeException();
            }



        }
    }
}