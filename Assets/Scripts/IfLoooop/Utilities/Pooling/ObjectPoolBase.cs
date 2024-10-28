using System;
using System.Collections.Generic;

namespace IfLoooop.Utilities.Pooling
{
    /// <summary>
    /// Base class for all object pools.
    /// </summary>
    /// <typeparam name="C">
    /// The <see cref="Type"/> of the collection that holds all objects in <see cref="ObjectPoolBase{C,T}.ObjectPool"/>. <br/>
    /// <i>Must implement the <see cref="IReadOnlyCollection{T}"/> interface, and be able to be created with the <c>new()</c> keyword.</i>
    /// </typeparam>
    /// <typeparam name="T">The <see cref="Type"/> of the objects in <see cref="ObjectPoolBase{C,T}.ObjectPool"/>.</typeparam>
    public abstract class ObjectPoolBase<C, T> where C : IReadOnlyCollection<T>, new()
    {
        #region Properties
        /// <summary>
        /// A collection that holds all objects in this pool.
        /// </summary>
        protected C ObjectPool { get; }
        /// <summary>
        /// Maximum amount of objects that can be in <see cref="ObjectPool"/> at a time.
        /// </summary>
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
        /// <summary>
        /// Makes sure the given <c>_InitialCapacity</c> is not bigger than <see cref="MaxCapacity"/>.
        /// </summary>
        /// <param name="_InitialCapacity">Amount of objects to create.</param>
        /// <param name="_initialCapacity">Adjusted <c>_InitialCapacity</c>.</param>
        // ReSharper disable once InconsistentNaming
        protected void CheckInitialCapacity(int _InitialCapacity, out int _initialCapacity)
        {
            _initialCapacity = _InitialCapacity > this.MaxCapacity ? this.MaxCapacity : _InitialCapacity;
        }
        #endregion
    }
}