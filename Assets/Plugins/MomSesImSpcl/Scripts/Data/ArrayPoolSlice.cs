using System;
using System.Buffers;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains an <see cref="Array"/> that will be returned to the <see cref="ArrayPool{T}.Shared"/> <see cref="ArrayPool{T}"/> on dispose.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Array"/>.</typeparam>
    public readonly struct ArrayPoolSlice<T> : IDisposable
    {
        #region Fields
        /// <summary>
        /// The <see cref="Array"/> of this <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// The size of the used elements in <see cref="array"/>. <br/>
        /// <i>Can differ from the <see cref="Array.Length"/>.</i>
        /// </summary>
        private readonly int size;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_Array"><see cref="array"/>.</param>
        /// <param name="_Size"><see cref="size"/>.</param>
        public ArrayPoolSlice(T[] _Array, int _Size)
        {
            this.array = _Array;
            this.size = _Size;
        }
        
        /// <summary>
        /// <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ArrayTuple">(<see cref="array"/>, <see cref="size"/>).</param>
        public ArrayPoolSlice((T[] Array, int Size) _ArrayTuple)
        {
            this.array = _ArrayTuple.Array;
            this.size = _ArrayTuple.Size;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a slice of <see cref="array"/> from <c>0</c> to <see cref="size"/>.
        /// </summary>
        /// <returns>A slice of <see cref="array"/> from <c>0</c> to <see cref="size"/>.</returns>
        public Span<T> GetSlice()
        {
            return this.array.AsSpan(0, this.size);
        }
        
        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(this.array, true);
        }
        #endregion
    }
}