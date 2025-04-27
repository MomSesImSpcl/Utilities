using System;
using Unity.Collections;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="NativeArray{T}"/>
    /// </summary>
    public static class NativeArrayExtensions
    {
        #region Methods
        /// <summary>
        /// Populates this <see cref="NativeArray{T}"/> with elements from the given <c>_Factory</c>-method.
        /// </summary>
        /// <param name="_NativeArray">The <see cref="NativeArray{T}"/> to populate.</param>
        /// <param name="_Factory">Defines how the elements should be created.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="NativeArray{T}"/>.</typeparam>
        public static NativeArray<T> Populate<T>(this NativeArray<T> _NativeArray, Func<T> _Factory) where T : struct
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _NativeArray.Length; i++)
            {
                _NativeArray[i] = _Factory();
            }
            
            return _NativeArray;
        }
        #endregion
    }
}