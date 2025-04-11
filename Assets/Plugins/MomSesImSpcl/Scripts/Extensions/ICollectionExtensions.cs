using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ICollection{T}"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ICollectionExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the index of the last element in this <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="_ICollection">The <see cref="ICollection{T}"/> to get the index in.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="ICollection{T}"/>.</typeparam>
        /// <returns>The index of the last element in this <see cref="ICollection{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndex<T>(this ICollection<T> _ICollection)
        {
            return _ICollection.Count - 1;
        }
        
        /// <summary>
        /// Populates this <see cref="ICollection{T}"/> with elements from the given <c>_Factory</c>-method.
        /// </summary>
        /// <param name="_ICollection">The <see cref="ICollection{T}"/> to populate.</param>
        /// <param name="_Amount">The number of elements to <see cref="ICollection{T}.Add"/> to the <see cref="ICollection{T}"/>.</param>
        /// <param name="_Factory">Defines how the elements should be created.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="ICollection{T}"/>.</typeparam>
        /// <typeparam name="N">Must be a numeric <see cref="Type"/> that fits inside a <see cref="uint"/>.</typeparam>
        /// <returns>The populated <see cref="ICollection{T}"/>.</returns>
        /// <exception cref="OverflowException">When the given <c>_Amount</c> cannot be converted into a <see cref="uint"/>.</exception>
        public static ICollection<T> Populate<T,N>(this ICollection<T> _ICollection, N _Amount, Func<T> _Factory) where N : unmanaged, IFormattable
        {
            uint _amount;
            
            try
            {
                _amount = Convert.ToUInt32(_Amount);
            }
            catch (OverflowException)
            {
                throw new OverflowException($"{nameof(_Amount).Bold()} must be convertible into a {"uint".Bold()}.");
            }
            
            foreach (var _ in _amount)
            {
                _ICollection.Add(_Factory());
            }
            
            return _ICollection;
        }
        #endregion
    }
}
