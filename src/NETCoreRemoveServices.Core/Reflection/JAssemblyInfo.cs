using System.Reflection;

namespace NETCoreRemoteServices.Core.Reflection
{
    /// <summary>
    /// JSON serializeable assembly info object
    /// </summary>
    internal class JAssemblyInfo
    {
        /// <summary>
        /// Assembly full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Initialize an empty JAssemblyInfo object.
        /// </summary>
        public JAssemblyInfo() { }

        /// <summary>
        /// Initialize JAssemblyInfo object using instance of <see cref="System.Reflection.Assembly"/>
        /// </summary>
        /// <param name="assembly">Initialized assembly object</param>
        public JAssemblyInfo(Assembly assembly) : this(assembly.FullName) { }

        /// <summary>
        /// Initialize JAssemblyInfo object using assembly full name
        /// </summary>
        /// <param name="fullName">Assembly full name.</param>
        public JAssemblyInfo(string fullName)
        {
            FullName = fullName;
        }
    }
}