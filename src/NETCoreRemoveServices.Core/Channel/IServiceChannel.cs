namespace NETCoreRemoveServices.Core.Channel
{
    /// <summary>
    /// Provides a proxy object for remote service hosted by <see cref="NETCoreRemoveServices.Core.Hosting.IRemoteServiceContainer"/> 
    /// </summary>
    public interface IServiceChannel
    {
        /// <summary>
        /// Get proxy for remote service
        /// </summary>
        /// <typeparam name="T">Contract of the desired service</typeparam>
        /// <returns>Service proxy</returns>
       T GetRemoteService<T>(); 
    }
}