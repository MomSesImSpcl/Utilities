namespace MomSesImSpcl.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// Represents a base class for wrapper objects that are managed within an object pool.
    /// </summary>
    /// <typeparam name="T">The type of the pool wrapper. Must inherit from <see cref="PoolWrapperBase{T}"/> and have a parameterless constructor.</typeparam>
    public abstract class PoolWrapperBase<T> where T : PoolWrapperBase<T>, new()
    {
        #region Properties
        /// <summary>
        /// Represents an object pool that manages a queue of objects.
        /// </summary>
        /// <typeparam name="T">
        /// The type of objects managed by the pool. Must be a class that inherits from <see cref="PoolWrapperBase{T}"/> and has a parameterless constructor.
        /// </typeparam>
        public CustomPool<T> CustomPool { get; init; } = null!;
        #endregion

        #region Methods
        /// <summary>
        /// Returns this instance back to its associated object pool.
        /// </summary>
        protected void Return()
        {
            this.CustomPool.Return((T)this);
        }
        #endregion
    }
}
