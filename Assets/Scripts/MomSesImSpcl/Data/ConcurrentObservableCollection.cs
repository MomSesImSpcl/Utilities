using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Represents a thread-safe, observable collection of objects. This collection manages concurrency internally
    /// and provides notifications when items are added, removed, or when the collection is otherwise modified.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public sealed class ConcurrentObservableCollection<T> : ObservableCollection<T>, IProducerConsumerCollection<T>
    {
        #region Fields
        /// <summary>
        /// A lock mechanism used to manage access to the collection,
        /// ensuring that read and write operations are thread-safe.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock = new();
        #endregion

        #region Properties
        /// <summary>
        /// Gets the element at the specified index using a thread-safe read operation.
        /// </summary>
        /// <param name="_Index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public new T this[int _Index]
        {
            get
            {
                if (readerWriterLock.IsWriteLockHeld)
                {
                    return base[_Index];
                }
                
                readerWriterLock.EnterReadLock();
                try
                {
                    return base[_Index];
                }
                finally
                {
                    readerWriterLock.ExitReadLock();
                }
            }
        }
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// This operation is thread-safe.
        /// </summary>
        public new int Count
        {
            get
            {
                readerWriterLock.EnterReadLock();
                try
                {
                    return base.Count;
                }
                finally
                {
                    readerWriterLock.ExitReadLock();
                }
            }
            
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ConcurrentObservableCollection{T}"/>.
        /// </summary>
        public ConcurrentObservableCollection() { }

        /// <summary>
        /// <see cref="ConcurrentObservableCollection{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to add.</param>
        public ConcurrentObservableCollection(IEnumerable<T> _IEnumerable) : base(_IEnumerable) { }
        
        /// <summary>
        /// <see cref="ConcurrentObservableCollection{T}"/>.
        /// </summary>
        /// <param name="_List">The <see cref="List{T}"/> to add.</param>
        public ConcurrentObservableCollection(List<T> _List) : base(_List) { }
        #endregion
        
        #region Methods
        /// <summary>
        /// Adds an item to the <see cref="ConcurrentObservableCollection{T}"/> in a thread-safe manner.
        /// </summary>
        /// <param name="_Item">The item to add to the collection.</param>
        public new void Add(T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.Add(_Item);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds a range of items to the <see cref="ConcurrentObservableCollection{T}"/>.
        /// This operation is thread-safe.
        /// </summary>
        /// <param name="_Items">The collection of items to be added.</param>
        public void AddRange(IEnumerable<T> _Items)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                foreach (var _item in _Items)
                {
                    base.Add(_item);
                }
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Attempts to add an item to the <see cref="ConcurrentObservableCollection{T}"/>.
        /// This operation is thread-safe.
        /// </summary>
        /// <param name="_Item">The item to be added to the collection.</param>
        /// <returns>Returns <c>true</c> if the item was added successfully; otherwise, <c>false</c>.</returns>
        public bool TryAdd(T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.Add(_Item);
                return true;
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index in a thread-safe manner.
        /// </summary>
        /// <param name="_Index">The zero-based index at which the item should be inserted.</param>
        /// <param name="_Item">The item to be inserted into the collection.</param>
        public new void Insert(int _Index, T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.InsertItem(_Index, _Item);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Replaces the element at the specified index with a new value in a thread-safe manner.
        /// </summary>
        /// <param name="_Index">The zero-based index of the element to replace.</param>
        /// <param name="_Item">The new value for the element at the specified index.</param>
        public void Set(int _Index, T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.SetItem(_Index, _Item);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Moves an item from the specified old index to the specified new index within the collection.
        /// </summary>
        /// <param name="_OldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="_NewIndex">The zero-based index to which the item should be moved.</param>
        public new void Move(int _OldIndex, int _NewIndex)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.MoveItem(_OldIndex, _NewIndex);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ConcurrentObservableCollection{T}"/>.
        /// </summary>
        /// <param name="_Item">The object to remove from the collection.</param>
        /// <returns>true if item was successfully removed; otherwise, false. This method also returns false if item is not found.</returns>
        public new bool Remove(T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                return base.Remove(_Item);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Attempts to take an element from the collection.
        /// </summary>
        /// <param name="_Item">The element removed from the collection, or the default value if the collection is empty.</param>
        /// <returns>true if an element was removed and retrieved successfully; otherwise, false.</returns>
        public bool TryTake(out T _Item)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                if (base.Count > 0)
                {
                    _Item = base[0];
                    base.RemoveAt(0);
                    return true;
                }
                else
                {
                    _Item = default!;
                    return false;
                }
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the item at the specified index from the collection in a thread-safe manner.
        /// </summary>
        /// <param name="_Index">The zero-based index of the item to remove.</param>
        public new void RemoveAt(int _Index)
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.RemoveItem(_Index);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Clears all items from the collection in a thread-safe manner.
        /// </summary>
        public new void Clear()
        {
            readerWriterLock.EnterWriteLock();
            try
            {
                base.Clear();
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="_Item">The object to locate in the collection.</param>
        /// <returns>true if the item is found in the collection; otherwise, false.</returns>
        public new bool Contains(T _Item)
        {
            readerWriterLock.EnterReadLock();
            try
            {
                return base.Contains(_Item);
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Converts the elements of the ConcurrentObservableCollection to an array.
        /// </summary>
        /// <returns>An array containing the elements of the ConcurrentObservableCollection.</returns>
        public T[] ToArray()
        {
            readerWriterLock.EnterReadLock();
            try
            {
                return this.ToArray();
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="ConcurrentObservableCollection{T}"/> to an <see cref="Array"/>, starting at a particular array index.
        /// </summary>
        /// <param name="_Array">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
        /// <param name="_Index">The zero-based index in the array at which copying begins.</param>
        public new void CopyTo(T[] _Array, int _Index)
        {
            readerWriterLock.EnterReadLock();
            try
            {
                base.CopyTo(_Array, _Index);
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="ConcurrentObservableCollection{T}"/>.
        /// </summary>
        /// <param name="_Item">The object to locate in the collection.</param>
        /// <returns>The index of the item if found in the collection; otherwise, -1.</returns>
        public new int IndexOf(T _Item)
        {
            readerWriterLock.EnterReadLock();
            try
            {
                return base.IndexOf(_Item);
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            readerWriterLock.EnterReadLock();
            try
            {
                return base.GetEnumerator();
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Raises the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        /// If the current synchronization context is not null, the event is raised
        /// on the synchronization context; otherwise, the event is raised directly.
        /// </summary>
        /// <param name="_NotifyCollectionChangedEventArgs">
        /// The event data, providing information about the change to the collection.
        /// </param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs _NotifyCollectionChangedEventArgs)
        {
            if (SynchronizationContext.Current == null)
            {
                base.OnCollectionChanged(_NotifyCollectionChangedEventArgs);
            }
            else
            {
                SynchronizationContext.Current.Post(_ => base.OnCollectionChanged(_NotifyCollectionChangedEventArgs), null);
            }
        }
        #endregion
    }
}