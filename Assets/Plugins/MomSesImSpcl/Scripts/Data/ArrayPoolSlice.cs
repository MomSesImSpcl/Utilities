using System;
using System.Buffers;
using MomSesImSpcl.Extensions;
using MomSesImSpcl.Utilities.Pooling.Wrappers;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains an <see cref="System.Array"/> that will be returned to the <see cref="ArrayPool{T}.Shared"/> <see cref="ArrayPool{T}"/> on dispose. <br/>
    /// Can be used to get a slice from an <see cref="ArrayPool{T}"/> <see cref="Array"/> that contains only the used elements.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the <see cref="System.Array"/>.</typeparam>
    public readonly struct ArrayPoolSlice<T> : IDisposable
    {
        #region Fields
        /// <summary>
        /// If <c>true</c>, the <see cref="Array"/> will be cleared on <see cref="Dispose"/>.
        /// </summary>
        private readonly bool clearOnDispose;
        #endregion
        
        #region Properties
        /// <summary>
        /// The <see cref="System.Array"/> of this <see cref="ArrayPoolSlice{T}"/>. <br/>
        /// <b>Might contain unused elements if this was created through <see cref="ArrayPool{T}"/>.</b>
        /// </summary>
        public T[] Array { get; }
        /// <summary>
        /// The size of the used elements in <see cref="Array"/>. <br/>
        /// <i>Can differ from the <see cref="Array.Length"/>.</i>
        /// </summary>
        public int Size { get; }
        #endregion
        
        #region Indexer
        /// <summary>
        /// Index accessor for <see cref="Array"/>.
        /// </summary>
        /// <param name="_Index">The index in <see cref="Array"/> to access.</param>
        public T this[int _Index] => this.Array[_Index];
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly returns the <see cref="Array"/> of the given <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ArrayPoolSlice"><see cref="ArrayPoolSlice{T}"/>.</param>
        /// <returns><see cref="Array"/>.</returns>
        public static implicit operator T[](ArrayPoolSlice<T> _ArrayPoolSlice) => _ArrayPoolSlice.Array;
        /// <summary>
        /// Implicitly returns the <see cref="Size"/> of the given <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ArrayPoolSlice"><see cref="ArrayPoolSlice{T}"/>.</param>
        /// <returns><see cref="Size"/>.</returns>
        public static implicit operator int(ArrayPoolSlice<T> _ArrayPoolSlice) => _ArrayPoolSlice.Size;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_Array"><see cref="Array"/>.</param>
        /// <param name="_Size"><see cref="Size"/>.</param>
        /// <param name="_ClearOnDispose"><see cref="clearOnDispose"/>.</param>
        public ArrayPoolSlice(T[] _Array, int _Size, bool _ClearOnDispose = false)
        {
            this.Array = _Array;
            this.Size = _Size;
            this.clearOnDispose = _ClearOnDispose;
        }
        
        /// <summary>
        /// <see cref="ArrayPoolSlice{T}"/>.
        /// </summary>
        /// <param name="_ArrayTuple">(<see cref="Array"/>, <see cref="Size"/>).</param>
        /// <param name="_ClearOnDispose"><see cref="clearOnDispose"/>.</param> 
        public ArrayPoolSlice((T[] Array, int Size) _ArrayTuple, bool _ClearOnDispose = false)
        {
            this.Array = _ArrayTuple.Array;
            this.Size = _ArrayTuple.Size;
            this.clearOnDispose = _ClearOnDispose;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a slice of <see cref="Array"/> from <c>0</c> to <see cref="Size"/>.
        /// </summary>
        /// <returns>A slice of <see cref="Array"/> from <c>0</c> to <see cref="Size"/>.</returns>
        public Span<T> GetSlice()
        {
            return this.GetSlice(0, this.Size);
        }
        
        /// <summary>
        /// Creates a slice of <see cref="Array"/> from <c>0</c> to <see cref="Size"/>.
        /// </summary>
        /// <param name="_StartIndex">The index in <see cref="Array"/> to start the slice at.</param>
        /// <param name="_Length">The number of elements to include in the slice.</param>
        /// <returns>A slice of <see cref="Array"/> from <c>0</c> to <see cref="Size"/>.</returns>
        public Span<T> GetSlice(int _StartIndex, int _Length)
        {
            return this.Array.AsSpan(_StartIndex, _Length);
        }

        /// <summary>
        /// Returns <c>true</c> if <see cref="Array"/> is not <c>null</c> and <see cref="Size"/> is greater then <c>0</c>:
        /// </summary>
        /// <returns><c>true</c> if <see cref="Array"/> is not <c>null</c> and <see cref="Size"/> is greater then <c>0</c>:</returns>
        public bool Any()
        {
            return this.Array is not null && this.Size > 0;
        }
        
        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(this.Array, this.clearOnDispose);
        }
        
        public override string ToString()
        {
            return StringBuilderPoolWrapper.Append(this.Array.ToString(), " | ", "Size: ", this.Size.Bold(), " | ", "Length: ", this.Array.Length.Bold());
        }
        #endregion
    }
}
