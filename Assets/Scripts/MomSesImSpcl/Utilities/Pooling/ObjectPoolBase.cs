using System.Collections.Generic;

namespace MomSesImSpcl.Utilities.Pooling
{
    /// <summary>
    /// Provides an abstract base class for creating object pools.
    /// </summary>
    /// <typeparam name="C">The collection type used for internal object storage. Must implement <see cref="IReadOnlyCollection{T}"/> and have a parameterless constructor.</typeparam>
    /// <typeparam name="T">The type of objects to be pooled.</typeparam>
    public abstract class ObjectPoolBase<C, T> where C : IReadOnlyCollection<T>, new()
    {
        #region Properties
        /// <summary>
        /// Represents a pool that manages reusable objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to be pooled. Must inherit from <see cref="PoolWrapperBase{T}"/> and have a parameterless constructor.</typeparam>
        protected C ObjectPool { get; }
        /// <summary>
        /// Gets the maximum capacity of the object pool.
        /// </summary>
        /// <value>
        /// An integer representing the maximum number of objects that can be held in the pool.
        /// This value is set through the constructor and cannot be less than 1.
        /// </value>
        protected int MaxCapacity { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// <see cref="ObjectPoolBase{C,T}"/>.
        /// </summary>
        /// <param name="_MaxCapacity"><see cref="MaxCapacity"/>.</param>
        public ObjectPoolBase(int _MaxCapacity = int.MaxValue)
        {
            this.ObjectPool = new C();
            this.MaxCapacity = _MaxCapacity < 1 ? 1 : _MaxCapacity;
        }
        #endregion
        
        #region Methods
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Validates the given initial capacity against the maximum capacity of the pool.
        /// </summary>
        /// <param name="_InitialCapacity">The initial capacity to be checked.</param>
        /// <param name="_initialCapacity">The validated initial capacity which will be either the given initial capacity or the maximum capacity, whichever is lower.</param>
        protected void CheckInitialCapacity(int _InitialCapacity, out int InitialCapacity)
        {
            InitialCapacity = _InitialCapacity > this.MaxCapacity ? this.MaxCapacity : _InitialCapacity;
        }
        #endregion
    }
}