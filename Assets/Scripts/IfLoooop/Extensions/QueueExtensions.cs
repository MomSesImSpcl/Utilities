#nullable enable
using System.Collections.Generic;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Queue{T}"/>.
    /// </summary>
    public static class QueueExtensions
    {
        #region Methods
        /// <summary>
        /// Tries to dequeue the object at the front of the specified queue.
        /// </summary>
        /// <param name="_Queue">The <see cref="Queue{T}"/> from which to dequeue an object.</param>
        /// <param name="_Object">The output parameter that will contain the object dequeued from the front of the queue if the operation is successful; otherwise, it will be set to <c>default</c>.</param>
        /// <typeparam name="T">The type of objects stored in the queue.</typeparam>
        /// <returns><c>true</c> if an object was successfully dequeued; otherwise, <c>false</c>.</returns>
        public static bool TryDequeue<T>(this Queue<T> _Queue, out T? _Object)
        {
            if (_Queue.Count > 0)
            {
                _Object = _Queue.Dequeue();
                return true;
            }

            _Object = default;
            
            return false;
        }

        /// <summary>
        /// Tries to peek at the object at the front of the specified queue without removing it.
        /// </summary>
        /// <param name="_Queue">The <see cref="Queue{T}"/> from which to peek at an object.</param>
        /// <param name="_Object">The output parameter that will contain the object at the front of the queue if the operation is successful; otherwise, it will be set to <c>default</c>.</param>
        /// <typeparam name="T">The type of objects stored in the queue.</typeparam>
        /// <returns><c>true</c> if an object was successfully peeked; otherwise, <c>false</c>.</returns>
        public static bool TryPeek<T>(this Queue<T> _Queue, out T? _Object)
        {
            if (_Queue.Count > 0)
            {
                _Object = _Queue.Peek();
                return true;
            }

            _Object = default;
            
            return false;
        }
        #endregion
    }
}