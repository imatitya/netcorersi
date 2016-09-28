using System;

namespace NETCoreRemoteServices.Core.Reflection
{
    internal class JResponse
    {
        /// <summary>
        /// Response content
        /// </summary>
        public JParameterInfo ReturnValue { get; set; }

        /// <summary>
        /// Response type
        /// </summary>
        public ResponseType ResponseType { get; set; }

        /// <summary>
        /// Initialize an empty JResponse object.
        /// </summary>
        public JResponse() : this(null, ResponseType.Empty) { }

        /// <summary>
        /// Initialize an empty JResponse object.
        /// </summary>
        public JResponse(Exception e) : this(new JParameterInfo(new JException(e)), ResponseType.Fault) { }

        /// <summary>
        /// Initialize an empty JResponse object.
        /// </summary>
        public JResponse(object obj) : this(new JParameterInfo(obj), ResponseType.Success) { }

        /// <summary>
        /// Initialize an empty JResponse object.
        /// </summary>
        public JResponse(JParameterInfo jParameterInfo, ResponseType responseType)
        {
            ReturnValue = jParameterInfo;
            ResponseType = responseType;
        }
    }
}