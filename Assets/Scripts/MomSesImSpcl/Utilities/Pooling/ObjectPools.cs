using System.Text;
using MomSesImSpcl.Utilities.Pooling.Wrappers;

namespace MomSesImSpcl.Utilities.Pooling
{
    /// <summary>
    /// Provides access to object pools for various types of poolable objects.
    /// </summary>
    public static class ObjectPools
    {
        #region Properties
        /// <summary>
        /// A static property providing access to a pool of <see cref="StringBuilderPoolWrapper"/> instances.
        /// This pool allows for the reuse of <see cref="StringBuilder"/> objects, improving performance
        /// by reducing the overhead associated with frequent allocations and deallocations of these objects.
        /// </summary>
        public static ObjectPool<StringBuilderPoolWrapper> StringBuilderPool { get; } = new(1);
        /// <summary>
        /// Provides access to a thread-safe object pool for <see cref="ConcurrentStringBuilderPoolWrapper"/> instances.
        /// </summary>
        /// <remarks>
        /// This pool is designed to manage reusable instances of <see cref="ConcurrentStringBuilderPoolWrapper"/> in a concurrent environment,
        /// aiming to minimize the overhead of creating and destroying <see cref="StringBuilder"/> objects, thus improving performance in scenarios
        /// where these objects are frequently used.
        /// </remarks>
        public static ConcurrentObjectPool<ConcurrentStringBuilderPoolWrapper> ConcurrentStringBuilderPool { get; } = new(1);
        #endregion
    }
}