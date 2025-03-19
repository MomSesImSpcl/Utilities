using System;
using System.Collections.Generic;
using MomSesImSpcl.Utilities.Comparers;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        #region Methods
        /// <summary>
        /// Sorts a <see cref="List{T}"/> in ascending order. <br/>
        /// <b>This will sort the original <see cref="List{T}"/>.</b>
        /// </summary>
        /// <param name="_List">The <see cref="List{T}"/> to sort.</param>
        /// <param name="_SortBy">The value to sort by.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="List{T}"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the value to sort by.</typeparam>
        /// <returns>The sorted <see cref="List{T}"/>.</returns>
        public static List<T> SortAscending<T,V>(this List<T> _List, Func<T,V> _SortBy) where V : IComparable<V>
        {
            _List.Sort(new AscendingComparer<T,V>(_SortBy));
            return _List;
        }
        
        /// <summary>
        /// Sorts a <see cref="List{T}"/> in ascending order. <br/>
        /// <b>This will sort the original <see cref="List{T}"/>.</b>
        /// </summary>
        /// <param name="_List">The <see cref="List{T}"/> to sort.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="List{T}"/>.</typeparam>
        /// <returns>The sorted <see cref="List{T}"/>.</returns>
        public static List<T> SortAscending<T>(this List<T> _List) where T : IComparable<T>
        {
            _List.Sort((_First, _Second) => _First.CompareTo(_Second));
            return _List;
        }
        
        /// <summary>
        /// Sorts a <see cref="List{T}"/> in descending order. <br/>
        /// <b>This will sort the original <see cref="List{T}"/>.</b>
        /// </summary>
        /// <param name="_List">The <see cref="List{T}"/> to sort.</param>
        /// <param name="_SortBy">The value to sort by.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="List{T}"/>.</typeparam>
        /// <typeparam name="V">The <see cref="Type"/> of the value to sort by.</typeparam>
        /// <returns>The sorted <see cref="List{T}"/>.</returns>
        public static List<T> SortDescending<T,V>(this List<T> _List, Func<T,V> _SortBy) where V : IComparable<V>
        {
            _List.Sort(new DescendingComparer<T,V>(_SortBy));
            return _List;
        }
        
        /// <summary>
        /// Sorts a <see cref="List{T}"/> in descending order. <br/>
        /// <b>This will sort the original <see cref="List{T}"/>.</b>
        /// </summary>
        /// <param name="_List">The <see cref="List{T}"/> to sort.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="List{T}"/>.</typeparam>
        /// <returns>The sorted <see cref="List{T}"/>.</returns>
        public static List<T> SortDescending<T>(this List<T> _List) where T : IComparable<T>
        {
            _List.Sort((_First, _Second) => _Second.CompareTo(_First));
            return _List;
        }
        #endregion
    }
}