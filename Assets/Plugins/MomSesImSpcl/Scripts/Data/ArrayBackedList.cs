using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// A resizable <see cref="List{T}"/> with an accessible minimum backing-array size.
    /// </summary>
    [Serializable]
    public sealed class ArrayBackedList<T> : IList<T>
    {
        #region Inspector Fields
        /// <summary>
        /// Contains all elements in this <see cref="ArrayBackedList{T}"/>.
        /// </summary>
        [SerializeField] private T[] elements;
        #endregion
        
        #region Fields
        /// <summary>
        /// The index of the last item in <see cref="elements"/>.
        /// </summary>
        private int lastUsedIndex = -1;
        #endregion
        
        #region Properties
        /// <summary>
        /// The number of used items currently in <see cref="elements"/>. <br/>
        /// <i>Can be less than <see cref="MinSize"/>.</i>
        /// </summary>
        public int Count => this.lastUsedIndex + 1;
        /// <summary>
        /// The minimum desired size of <see cref="elements"/>.
        /// </summary>
        public int MinSize { get; private set; }
        /// <summary>
        /// The current size of <see cref="elements"/>.
        /// </summary>
        public int Capacity => this.elements.Length;
        /// <summary>
        /// Will always be <c>false</c>.
        /// </summary>
        public bool IsReadOnly => false;
        #endregion
        
        #region Indexer
        /// <summary>
        /// Returns the element in <see cref="elements"/> at the given <c>_Index</c>.
        /// </summary>
        /// <param name="_Index">The index to get the element of.</param>
        public T this[int _Index]
        {
            get => this.elements[_Index];
            set => this.elements[_Index] = value;
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ArrayBackedList{T}"/>.
        /// </summary>
        /// <param name="_MinSize"><see cref="MinSize"/>.</param>
        public ArrayBackedList(int _MinSize = 0)
        {
            if (_MinSize <= 0)
            {
                this.elements = Array.Empty<T>();
            }
            else
            {
                this.elements = new T[_MinSize];
                this.MinSize = _MinSize;
            }
        }

        /// <summary>
        /// <see cref="ArrayBackedList{T}"/>.
        /// </summary>
        /// <param name="_Collection">A collection to add to <see cref="elements"/>.</param>
        /// <param name="_MinSize"><see cref="MinSize"/>.</param>
        public ArrayBackedList(IEnumerable<T> _Collection, int _MinSize = 0)
        {
            this.MinSize = _MinSize;

            if (_Collection is ICollection<T> _collection)
            {
                var _elements = _collection.Count;
                var _size = Mathf.Max(_elements, _MinSize);

                this.elements = new T[_size];

                if (_elements > 0)
                {
                    this.lastUsedIndex = _elements - 1;
                }

                _collection.CopyTo(this.elements, 0);

                return;
            }

            var _items = _Collection.ToArray();
            var _itemCount = _items.Length;
            var _arraySize = Mathf.Max(_itemCount, _MinSize);

            this.elements = new T[_arraySize];

            if (_itemCount > 0)
            {
                this.lastUsedIndex = _itemCount - 1;
            }

            Array.Copy(_items, 0, this.elements, 0, _itemCount);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Adds the given <c>_Item</c> to <see cref="elements"/>.
        /// </summary>
        /// <param name="_Item">The item  to add.</param>
        public void Add(T _Item)
        {
            this.EnsureCapacity(this.Count + 1);
            this.elements[++this.lastUsedIndex] = _Item;
        }

        /// <summary>
        /// Adds the given collection to <see cref="elements"/>.
        /// </summary>
        /// <param name="_Collection">The elements to add.</param>
        public void AddRange(IEnumerable<T> _Collection)
        {
            if (_Collection is ICollection<T> _collection)
            {
                this.EnsureCapacity(this.Count + _collection.Count);
                _collection.CopyTo(this.elements, this.lastUsedIndex + 1);
                this.lastUsedIndex += _collection.Count;
            }
            else
            {
                var _items = _Collection.ToArray();

                this.EnsureCapacity(this.Count + _items.Length);

                foreach (var _item in _items)
                {
                    this.elements[++this.lastUsedIndex] = _item;
                }   
            }
        }

        /// <summary>
        /// Returns a <see cref="Span{T}"/> of <see cref="elements"/>.
        /// </summary>
        /// <returns><see cref="elements"/> as a <see cref="Span{T}"/>.</returns>
        public Span<T> AsSpan()
        {
            return this.elements.AsSpan(0, this.Count);
        }
        
        /// <summary>
        /// Clears all <see cref="elements"/>.
        /// </summary>
        public void Clear()
        {
            if (this.MinSize == 0)
            {
                this.elements = Array.Empty<T>();
            }
            else
            {
                if (this.Capacity > this.MinSize)
                {
                    Array.Resize(ref this.elements, this.MinSize);
                }

                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < this.MinSize; i++)
                {
                    this.elements[i] = default;
                }
            }
            
            this.lastUsedIndex = -1;
        }
        
        /// <summary>
        /// Checks if the given <c>_Item</c> is in <see cref="elements"/>.
        /// </summary>
        /// <param name="_Item">The item to look for.</param>
        /// <returns><c>true</c> if the item is in <see cref="elements"/>, otherwise <c>false</c>.</returns>
        public bool Contains(T _Item) 
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(this.elements[i], _Item))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Copies all <see cref="elements"/> to the given <see cref="Array"/>, starting at the given index.
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to copy the <see cref="elements"/> into.</param>
        /// <param name="_ArrayIndex">The index in the given <see cref="Array"/> to start inserting the <see cref="elements"/>.</param>
        public void CopyTo(T[] _Array, int _ArrayIndex)
        {
            Array.Copy(this.elements, 0, _Array, _ArrayIndex, this.Count);
        }
        
        /// <summary>
        /// Ensures that <see cref="elements"/> can contain at least the given <c>_Capacity</c>.
        /// </summary>
        /// <param name="_Capacity">The minimum required capacity.</param>
        private void EnsureCapacity(int _Capacity)
        {
            if (_Capacity <= this.Capacity)
            {
                return;
            }

            var _newCapacity = this.Capacity == 0 ? Mathf.Max(this.MinSize, 4) : this.elements.Length * 2;

            if (_newCapacity < _Capacity)
            {
                _newCapacity = _Capacity;
            }

            Array.Resize(ref this.elements, _newCapacity);
        }
        
        /// <summary>
        /// Searches for the index of the given <c>_Item</c> in <see cref="elements"/>.
        /// </summary>
        /// <param name="_Item">The item to find the index of.</param>
        /// <returns>The index of the given <c>_Item</c> in <see cref="elements"/>, or <c>-1</c> if it couldn't be found.</returns>
        public int IndexOf(T _Item)
        {
            return this.FindIndex(_Item);
        }
        
        /// <summary>
        /// Inserts the given <c>_Item</c> into <see cref="elements"/> at the given <c>_Index</c>.
        /// </summary>
        /// <param name="_Index">The index in <see cref="elements"/> to insert the item at.</param>
        /// <param name="_Item">The item to insert.</param>
        public void Insert(int _Index, T _Item)
        {
            this.EnsureCapacity(this.Count + 1);
            
            // ReSharper disable once InconsistentNaming
            for (var i = this.lastUsedIndex + 1; i > _Index; i--)
            {
                this.elements[i] = this.elements[i - 1];
            }

            this.elements[_Index] = _Item;
            this.lastUsedIndex++;
        }
        
        /// <summary>
        /// Removes the given <c>_Item</c> from <see cref="elements"/>. <br/>
        /// <i>If the order of the <see cref="elements"/> is not important, use <see cref="RemoveUnordered"/>.</i>
        /// </summary>
        /// <param name="_Item">The item to remove.</param>
        /// <returns><c>true</c> if the item was removed, otherwise <c>false</c>.</returns>
        public bool Remove(T _Item)
        {
            if (this.FindIndex(_Item) is var _index and >= 0)
            {
                this.RemoveAt(_index);
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Removes the given <c>_Item</c> from <see cref="elements"/> without maintaining the order.
        /// </summary>
        /// <param name="_Item">The item to remove.</param>
        /// <returns><c>true</c> if the item was removed, otherwise <c>false</c>.</returns>
        public bool RemoveUnordered(T _Item)
        {
            if (this.FindIndex(_Item) is var _index and >= 0)
            {
                this.RemoveAtUnordered(_index);
                
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Searches for the index of the given <c>_Item</c> in <see cref="elements"/>.
        /// </summary>
        /// <param name="_Item">The item to find the index of.</param>
        /// <returns>The index of the given <c>_Item</c> in <see cref="elements"/>, or <c>-1</c> if it couldn't be found.</returns>
        private int FindIndex(T _Item)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(this.elements[i], _Item))
                {
                    return i;
                }
            }

            return -1;
        }
        
        /// <summary>
        /// Removes the element at the given <c>_Index</c>. <br/>
        /// <i>If the order of the <see cref="elements"/> is not important, use <see cref="RemoveAtUnordered"/>.</i>
        /// </summary>
        /// <param name="_Index">The index in <see cref="elements"/> to remove.</param>
        public void RemoveAt(int _Index)
        {
            // ReSharper disable once InconsistentNaming
            for (var i = _Index; i < this.lastUsedIndex; i++)
            {
                this.elements[i] = this.elements[i + 1];    
            }
            
            this.elements[this.lastUsedIndex--] = default;
        }
        
        /// <summary>
        /// Removes the element at the given <c>_Index</c>, without maintaining the order. <br/>
        /// <i>Faster than <see cref="RemoveAt"/>.</i>
        /// </summary>
        /// <param name="_Index">The index in <see cref="elements"/> to remove.</param>
        public void RemoveAtUnordered(int _Index)
        {
            this.elements[_Index] = this.elements[this.lastUsedIndex];
            this.elements[this.lastUsedIndex--] = default;
        }

        /// <summary>
        /// Resizes the <see cref="elements"/> <see cref="Array"/> if it is greater than <see cref="MinSize"/> or <see cref="Count"/>.
        /// </summary>
        public void TrimExcess()
        {
            if (this.Capacity > Mathf.Max(this.MinSize, this.Count))
            {
                Array.Resize(ref this.elements, Mathf.Max(this.MinSize, this.Count));
            }
        }
        
        /// <summary>
        /// Iterates over <see cref="elements"/>.
        /// </summary>
        /// <returns><see cref="elements"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// Iterates over <see cref="elements"/>.
        /// </summary>
        /// <returns><see cref="elements"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < this.Count; i++)
            {
                yield return this.elements[i];
            }
        }
        #endregion
    }
}