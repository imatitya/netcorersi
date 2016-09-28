using System;
using System.Reflection;

namespace NETCoreRemoteServices.Core.Reflection
{
    /// <summary>
    /// JSON serializeable type info object
    /// </summary>
    internal class JTypeInfo
    {
        /// <summary>
        /// Full type name.
        /// </summary>
        public string FullName { get; set; }

        public string AssemblyQualifiedName { get; set; }
        /// <summary>
        /// Initialize an empty JTypeInfo object.
        /// </summary>
        public JTypeInfo() { }

        /// <summary>
        /// Hosting assembly information.
        /// </summary>
        public JAssemblyInfo Assembly { get; set; }

        /// <summary>
        /// Initialize JTypeInfo object using instance of <see cref="System.Type"/>
        /// </summary>
        /// <param name="type">Type to represent.</param>
        public JTypeInfo(Type type)
        {
            FullName = type.FullName;
            AssemblyQualifiedName = type.AssemblyQualifiedName;
            Assembly = new JAssemblyInfo(type.GetTypeInfo().Assembly.FullName);
        }
    }
}