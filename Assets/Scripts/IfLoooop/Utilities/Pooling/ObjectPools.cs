using System.Text;
using IfLoooop.Utilities.Pooling.Wrappers;

namespace IfLoooop.Utilities.Pooling
{
    /// <summary>
    /// Contains all kinds of object pools.
    /// </summary>
    public static class ObjectPools
    {
        #region Properties
        /// <summary>
        /// <see cref="ObjectPool{T}"/> for <see cref="StringBuilder"/>.
        /// </summary>
        internal static ObjectPool<StringBuilderPoolWrapper> StringBuilderPool { get; } = new(1);
        #endregion
    }
}