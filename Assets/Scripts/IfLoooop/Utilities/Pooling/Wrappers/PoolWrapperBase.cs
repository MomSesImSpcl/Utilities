namespace IfLoooop.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// Wrapper for objects in <see cref="ObjectPool{T}"/>.
    /// </summary>
    /// <typeparam name="T">Must have a parameterless constructor.</typeparam>
    public abstract class PoolWrapperBase<T> where T : PoolWrapperBase<T>, new()
    {
        #region Properties
        /// <summary>
        /// The <see cref="ObjectPool{T}"/> this <see cref="PoolWrapperBase{T}"/> object belongs to.
        /// </summary>
        public ObjectPool<T> ObjectPool { get; init; } = null!;
        #endregion

        #region Methods
        /// <summary>
        /// Returns this <see cref="PoolWrapperBase{T}"/> object to its <see cref="ObjectPool"/>.
        /// </summary>
        protected void Return()
        {
            this.ObjectPool.Return((T)this);
        }
        #endregion
    }
}