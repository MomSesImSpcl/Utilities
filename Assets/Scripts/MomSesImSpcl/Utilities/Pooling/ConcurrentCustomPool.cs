using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MomSesImSpcl.Utilities.Pooling.Wrappers;

namespace MomSesImSpcl.Utilities.Pooling
{
    /// <summary>
    /// Represents a thread-safe object pool for managing reusable objects derived from <see cref="ConcurrentPoolWrapperBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to be pooled, which must derive from <see cref="ConcurrentPoolWrapperBase{T}"/> and have a parameterless constructor.</typeparam>
    public sealed class ConcurrentCustomPool<T> : CustomPoolBase<ConcurrentQueue<T>, T> where T : ConcurrentPoolWrapperBase<T>, new()
    {
        #region Fields
        /// <summary>
        /// A semaphore used to control access to the pooled objects, ensuring that no more than a specified
        /// number of threads can access the pool concurrently.
        /// </summary>
        private readonly SemaphoreSlim semaphoreSlim;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ConcurrentCustomPool{T}"/>.
        /// </summary>
        /// <param name="_InitialCapacity">The amount of objects to create, when the <see cref="ConcurrentCustomPool{T}"/> is created.</param>
        /// <param name="_MaxCapacity">The maximum number of objects <see cref="CustomPoolBase{C,T}.ObjectPool"/> can hold at a time.</param>
        public ConcurrentCustomPool(int _InitialCapacity, int _MaxCapacity = int.MaxValue) : base(_MaxCapacity)
        {
            base.CheckInitialCapacity(_InitialCapacity, out var _initialCapacity);
            this.semaphoreSlim = new SemaphoreSlim(base.MaxCapacity, base.MaxCapacity);
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _initialCapacity; i++)
            {
                base.ObjectPool.Enqueue(this.CreateNew());
            }
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ConcurrentCustomPool()
        {
            this.semaphoreSlim.Dispose();
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a new instance of the object type defined in the pool.
        /// </summary>
        /// <returns>A new instance of type <typeparamref name="T"/>.</returns>
        private T CreateNew()
        {
            return new T
            {
                CustomPool = this
            };
        }

        /// <summary>
        /// Asynchronously retrieves an object from the <see cref="ConcurrentCustomPool{T}"/>.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an object from the pool.
        /// If the pool has no available objects, a new object is created and returned.
        /// </returns>
        public async Task<T> GetAsync()
        {
            await this.semaphoreSlim.WaitAsync();
            return base.ObjectPool.TryDequeue(out var _object) ? _object : this.CreateNew();
        }

        /// <summary>
        /// Returns an instance of type <typeparamref name="T"/> to the pool for future reuse.
        /// </summary>
        /// <param name="_Object">The instance of <typeparamref name="T"/> to be returned to the pool.</param>
        public void Return(T _Object)
        {
            base.ObjectPool.Enqueue(_Object);
            this.semaphoreSlim.Release();
        }
        #endregion
    }
}