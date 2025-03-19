namespace MomSesImSpcl.Utilities.Pooling.Wrappers
{
    /// <summary>
    /// Provides a base class for wrappers that belong to a concurrent object pool.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the concurrent pool wrapper. Must be a class that inherits from
    /// <see cref="ConcurrentPoolWrapperBase{T}"/> and has a parameterless constructor.
    /// </typeparam>
    public abstract class ConcurrentPoolWrapperBase<T> where T : ConcurrentPoolWrapperBase<T>, new()
    {
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="ConcurrentCustomPool{T}"/> associated with this wrapper.
        /// </summary>
        /// <remarks>
        /// This property is initialized when the wrapper object is created and associates
        /// the wrapper with its parent object pool. It is used internally to manage the
        /// return of the object back to the pool for reuse.
        /// </remarks>
        public ConcurrentCustomPool<T> CustomPool { get; init; } = null!;
        #endregion

        #region Methods
        /// <summary>
        /// Returns the current instance to its associated object pool.
        /// </summary>
        protected void Return()
        {
            this.CustomPool.Return((T)this);
        }
        #endregion
    }
}