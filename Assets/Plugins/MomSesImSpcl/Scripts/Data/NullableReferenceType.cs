using System;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Wrapper class that contains a nullable primitive <see cref="Type"/> to use it as a reference <see cref="Type"/>.
    /// </summary>
    public sealed class NullableReferenceType<T> where T : struct
    {
        #region Properties
        /// <summary>
        /// Contains the nullable value of the primitive <see cref="Type"/>.
        /// </summary>
        public T? Value { get; set; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly converts this <see cref="ReferenceType{T}"/> to the underlying <see cref="Value"/>.
        /// </summary>
        /// <param name="_ReferenceType">The <see cref="ReferenceType{T}"/> to get the <see cref="Value"/> from.</param>
        /// <returns><see cref="Value"/>.</returns>
        public static implicit operator T?(NullableReferenceType<T> _ReferenceType) => _ReferenceType.Value;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="ReferenceType{T}"/> <see cref="object"/>.
        /// </summary>
        public NullableReferenceType()
        {
            this.Value = null;
        }

        /// <summary>
        /// Creates a new <see cref="ReferenceType{T}"/> <see cref="object"/>.
        /// </summary>
        /// <param name="_Value"><see cref="Value"/>.</param>
        public NullableReferenceType(T? _Value)
        {
            this.Value = _Value;
        }
        #endregion
    }
}