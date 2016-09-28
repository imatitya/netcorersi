using System;
using System.Collections.Generic;
using System.Reflection;

namespace NETCoreRemoteServices.Core.Reflection
{
    /// <summary>
    /// JSON serializeable method info object
    /// </summary>
    internal class JMethodInfo
    {
        /// <summary>
        /// Method name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type that declares this method.
        /// </summary>
        public JTypeInfo DeclaringType { get; set; }

        /// <summary>
        /// Method parameters.
        /// </summary>
        public JParameterInfo[] Parameters { get; set; }

        /// <summary>
        /// Initialize an empty JMethodInfo object.
        /// </summary>
        public JMethodInfo() { }

        /// <summary>
        /// Initialize new instance of JMethodInfo using the provided information.
        /// </summary>
        /// <param name="methodInfo"><see cref="System.Reflection.MethodInfo"/> object to refer</param>
        /// <param name="parametersValue">parameters value</param>
        public JMethodInfo(MethodInfo methodInfo, IReadOnlyList<object> parametersValue)
        {
            Name = methodInfo.Name;
            DeclaringType = new JTypeInfo(methodInfo.DeclaringType);
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if(parameters.Length != parametersValue.Count)
            {
                throw new InvalidOperationException("Amount of parameters value should be equal to the amount of the parameters as declared in method info object");
            }

            Parameters = new JParameterInfo[parameters.Length];
            for(int i=0; i< Parameters.Length; ++i)
            {
                Parameters[i] = new JParameterInfo(parameters[i].Name, new JTypeInfo(parameters[i].ParameterType), parametersValue[i]);
            }
        }
    }
}