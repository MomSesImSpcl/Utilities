using System;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Wrapper class that contains a primitive <see cref="Type"/> to use it as a reference <see cref="Type"/>.
    /// </summary>
    public sealed class ReferenceType<T> where T : struct
    {
        #region Properties
        /// <summary>
        /// Contains the value of the primitive <see cref="Type"/>.
        /// </summary>
        public T Value { get; set; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly converts this <see cref="ReferenceType{T}"/> to the underlying <see cref="Value"/>.
        /// </summary>
        /// <param name="_ReferenceType">The <see cref="ReferenceType{T}"/> to get the <see cref="Value"/> from.</param>
        /// <returns><see cref="Value"/>.</returns>
        public static implicit operator T(ReferenceType<T> _ReferenceType) => _ReferenceType.Value;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="ReferenceType{T}"/> <see cref="object"/>.
        /// </summary>
        public ReferenceType()
        {
            this.Value = default;
        }

        /// <summary>
        /// Creates a new <see cref="ReferenceType{T}"/> <see cref="object"/>.
        /// </summary>
        /// <param name="_Value"><see cref="Value"/>.</param>
        public ReferenceType(T _Value)
        {
            this.Value = _Value;
        }
        #endregion
    }
}