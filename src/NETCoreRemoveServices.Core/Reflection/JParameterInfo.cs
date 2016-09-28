namespace NETCoreRemoteServices.Core.Reflection
{
    /// <summary>
    /// JSON serializeable parameter info object
    /// </summary>
    internal class JParameterInfo
    {
        /// <summary>
        /// Parameter type.
        /// </summary>
        public JTypeInfo ParameterType { get; set; }

        /// <summary>
        /// Parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Initialize an empty JParameterInfo object.
        /// </summary>
        public JParameterInfo() { }


        /// <summary>
        /// Initialize new instance of JParameterInfo from instance of an object
        /// </summary>
        /// <param name="instance"></param>
        public JParameterInfo(object instance) : this(string.Empty, new JTypeInfo(instance.GetType()), instance) { }

        /// <summary>
        /// Initialize new instance of JParameterInfo 
        /// </summary>
        /// <param name="name">parameter name</param>
        /// <param name="parameterType">parameter type</param>
        /// <param name="value">parameter value</param>
        public JParameterInfo(string name, JTypeInfo parameterType, object value)
        {
            Name = name;
            ParameterType = parameterType;
            Value = value;
        }
    }
}