using System;
using System.Collections.Generic;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="List{T}"/>
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IListExtensions
    {
        #region Methods
        /// <summary>
        /// Removes and returns the first item in the list.
        /// </summary>
        /// <param name="_List">The list from which the first item will be removed and returned.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <returns>The first item that was removed from the list.</returns>
        public static T Dequeue<T>(this IList<T> _List)
        {
            var _firstEntry = _List[0];
            _List.RemoveAt(0);

            return _firstEntry;
        }

        /// <summary>
        /// Moves an item from one index to another within the list.
        /// </summary>
        /// <param name="_List">The list containing the item to be moved.</param>
        /// <param name="_OldIndex">The current index of the item.</param>
        /// <param name="_NewIndex">The new index to which the item should be moved.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        public static void Move<T>(this IList<T> _List, int _OldIndex, int _NewIndex)
        {
            var _item = _List[_OldIndex];
            
            _List.RemoveAt(_OldIndex);
            _List.Insert(_NewIndex, _item);
        }

        /// <summary>
        /// Moves the given item to the new index in the list.
        /// </summary>
        /// <param name="_List">The list containing the item to be moved.</param>
        /// <param name="_Item">The item to move.</param>
        /// <param name="_NewIndex">The new index to which the item should be moved.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        public static void Move<T>(this IList<T> _List, T _Item, int _NewIndex)
        {
            _List.Remove(_Item);
            _List.Insert(_NewIndex, _Item);
        }

        /// <summary>
        /// Randomizes the order of elements in the list.
        /// </summary>
        /// <param name="_List">The list to be shuffled.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <returns>The shuffled list.</returns>
        public static IList<T> Shuffle<T>(this IList<T> _List)
        {
            var _random = new Random();
            var _count = _List.Count;

            while (_count > 1)
            {
                _count--;
                var _index = _random.Next(_count + 1);
                (_List[_index], _List[_count]) = (_List[_count], _List[_index]);
            }

            return _List;
        }
        #endregion
    }
}