using System;
using System.Collections.Generic;

namespace MomSesImSpcl.Extensions
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
        /// Removes the first element in this <see cref="IList{T}"/> that matches the given <c>_Condition</c>. <br/>
        /// <i>If no element inside the <see cref="IList{T}"/> matches the given <c>_Condition</c>, nothing will be removed.</i>
        /// </summary>
        /// <param name="_List">The <see cref="IList{T}"/> to remove the element from.</param>
        /// <param name="_Condition">The condition that must be met for the element to be removed.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the element.</typeparam>
        public static void Remove<T>(this IList<T> _List, Predicate<T> _Condition)
        {
            var _index = _List.FindIndex(_Condition);

            if (_index != -1)
            {
                _List.RemoveAt(_index);
            }
        }
        
        /// <summary>
        /// Randomizes the order of elements in the list. <br/>
        /// <i>Uses the Fisher-Yates shuffle.</i>
        /// </summary>
        /// <param name="_List">The list to be shuffled.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        public static void Shuffle<T>(this IList<T> _List)
        {
            var _random = new Random();
            
            // ReSharper disable once InconsistentNaming
            for (var i = _List.Count - 1; i > 0; i--)
            {
                // ReSharper disable once InconsistentNaming
                var j = _random.Next(i + 1);
                
                (_List[i], _List[j]) = (_List[j], _List[i]);
            }
        }

        /// <summary>
        /// Removes an element from the <see cref="IList{T}"/> by swapping it with the last element and then removing the last element. <br/>
        /// <b>Don't use this when the order of the elements matters.</b>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="_List">The <see cref="IList{T}"/> from which to remove an element.</param>
        /// <param name="_Index">The index of the element to remove.</param>
        public static void SwapAndPop<T>(this IList<T> _List, int _Index)
        {
            var _lastIndex = _List.Count - 1;
            _List[_Index] = _List[_lastIndex];
            _List.RemoveAt(_lastIndex);
        }
        #endregion
    }
}
