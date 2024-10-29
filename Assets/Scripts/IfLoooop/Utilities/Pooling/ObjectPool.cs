using System.Collections.Generic;
using IfLoooop.Utilities.Pooling.Wrappers;

namespace IfLoooop.Utilities.Pooling
{
    /// <summary>
    /// Represents a pool of reusable objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to be pooled.</typeparam>
    public sealed class ObjectPool<T> : ObjectPoolBase<Queue<T>, T> where T : PoolWrapperBase<T>, new()
    {
        #region Constructors
        /// <summary>
        /// <see cref="ObjectPool{T}"/>.
        /// </summary>
        /// <param name="_InitialCapacity">The amount of objects to create, when this <see cref="ObjectPool{T}"/> is created.</param>
        /// <param name="_MaxCapacity"><see cref="ObjectPoolBase{C,T}.MaxCapacity"/>.</param>
        public ObjectPool(int _InitialCapacity, int _MaxCapacity = int.MaxValue) : base(_MaxCapacity)
        {
            base.CheckInitialCapacity(_InitialCapacity, out var _initialCapacity);
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _initialCapacity; i++)
            {
                base.ObjectPool.Enqueue(this.CreateNew());
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a new instance of the pooled object.
        /// </summary>
        /// <returns>A new instance of type <typeparamref name="T"/> with its <c>ObjectPool</c> property set to this object pool.</returns>
        private T CreateNew()
        {
            return new T
            {
                ObjectPool = this
            };
        }

        /// <summary>
        /// Retrieves an object from the pool. If the pool is empty, a new object is created and returned.
        /// </summary>
        /// <returns>An object of type <typeparamref name="T"/> from the pool.</returns>
        public T Get()
        {
            return base.ObjectPool.TryDequeue(out var _object) ? _object : this.CreateNew();
        }

        /// <summary>
        /// Returns an object of type <typeparamref name="T"/> to the pool.
        /// </summary>
        /// <param name="_Object">The object to return to the pool.</param>
        public void Return(T _Object)
        {
            if (base.ObjectPool.Count < base.MaxCapacity)
            {
                base.ObjectPool.Enqueue(_Object);
            }
        }
        #endregion
    }
}