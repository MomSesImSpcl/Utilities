using System;
using System.Collections.Generic;
using IfLoooop.Utilities.Pooling.Wrappers;

namespace IfLoooop.Utilities.Pooling
{
    /// <summary>
    /// Object pool with a <see cref="Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of the objects in <see cref="ObjectPoolBase{C,T}.ObjectPool"/>.
    /// <i>Must be a class and be able to be created with the <c>new()</c> keyword. </i>
    /// </typeparam>
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
        /// Creates a new object of <see cref="Type"/> <c>T</c>.
        /// </summary>
        /// <returns>The created object of <see cref="Type"/> <c>T</c>.</returns>
        private T CreateNew()
        {
            return new T
            {
                ObjectPool = this
            };
        }
        
        /// <summary>
        /// Retrieves an object from <see cref="ObjectPoolBase{C,T}.ObjectPool"/>.
        /// </summary>
        /// <returns>An object of <see cref="Type"/> <c>T</c>.</returns>
        public T Get()
        {
            return base.ObjectPool.TryDequeue(out var _object) ? _object : this.CreateNew();
        }

        /// <summary>
        /// Returns the given <c>_Object</c> to <see cref="ObjectPoolBase{C,T}.ObjectPool"/>, if it is not currently full.
        /// </summary>
        /// <param name="_Object">The object to return to <see cref="ObjectPoolBase{C,T}.ObjectPool"/>.</param>
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