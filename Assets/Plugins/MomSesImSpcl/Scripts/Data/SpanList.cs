using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// TODO: Test all methods if they work.
    /// </summary>
    [Serializable]
    public sealed class SpanList<T> : IList<T>
    {
        #region Inspector Fields
        /// <summary>
        /// Contains all elements in this <see cref="SpanList{T}"/>.
        /// </summary>
        [SerializeField] private T[] elements;
        #endregion
        
        #region Fields
        /// <summary>
        /// The index of the last item in <see cref="elements"/>.
        /// </summary>
        private int lastUsedIndex = -1; // TODO: Check if there is a better name.
        #endregion
        
        #region Properties
        /// <summary>
        /// The number of items currently in <see cref="elements"/>. <br/>
        /// <i>Can be less than <see cref="Size"/>.</i>
        /// </summary>
        public int Count => this.lastUsedIndex + 1;
        /// <summary>
        /// The minimum desired size of <see cref="elements"/>.
        /// </summary>
        public int Size { get; private set; }
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
        /// <see cref="SpanList{T}"/>.
        /// </summary>
        /// <param name="_Size"><see cref="Size"/>.</param>
        public SpanList(int _Size = 0)
        {
            if (_Size == 0)
            {
                this.elements = Array.Empty<T>();
            }
            else
            {
                this.lastUsedIndex = _Size - 1;
                this.Size = _Size;
                this.elements = new T[_Size];
            }
        }

        /// <summary>
        /// <see cref="SpanList{T}"/>.
        /// </summary>
        /// <param name="_Collection">A collection to add to <see cref="elements"/>.</param>
        /// <param name="_Size"><see cref="Size"/>.</param>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public SpanList(IEnumerable<T> _Collection, int _Size = 0)
        {
            var _collection = _Collection as ICollection<T>;
            var _elements = _collection?.Count ?? _Collection.Count();
            var _size = Mathf.Max(_elements, _Size);
            this.Size = _Size;
            
            this.elements = new T[_size];

            if (_size > 0)
            {
                this.lastUsedIndex = _size - 1;
            }
            
            if (_collection != null)
            {
                _collection.CopyTo(this.elements, 0);
            }
            else
            {
                var _index = 0;
                foreach (var _element in _Collection)
                {
                    this.elements[_index++] = _element;
                }
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Adds the given <c>_Item</c> to <see cref="elements"/>.
        /// </summary>
        /// <param name="_Item">The item  to add.</param>
        public void Add(T _Item)
        {
            if (this.Count + 1 > this.Size)
            {
                Array.Resize(ref this.elements, this.Size + 1);    
            }
            
            this.elements[++this.lastUsedIndex] = _Item;
        }

        /// <summary>
        /// Adds the given collection to <see cref="elements"/>.
        /// </summary>
        /// <param name="_Collection">The elements to add.</param>
        public void AddRange(IEnumerable<T> _Collection)
        {
            // TODO: Check if "Possible multiple enumeration" can be avoided.
            var _collection = _Collection as ICollection<T>;
            var _elements = _collection?.Count ?? _Collection.Count();

            if (this.Count + _elements > this.Size)
            {
                Array.Resize(ref this.elements, this.Size + _elements);    
            }

            if (_collection != null)
            {
                _collection.CopyTo(this.elements, this.lastUsedIndex + 1);
                this.lastUsedIndex += _elements;
            }
            else
            {
                foreach (var _item in _Collection)
                {
                    this.elements[++this.lastUsedIndex] = _item;
                }   
            }
        }
        
        /// <summary>
        /// Clears all <see cref="elements"/>.
        /// </summary>
        public void Clear()
        {
            if (this.Size == 0)
            {
                this.elements = Array.Empty<T>();
            }
            else
            {
                if (this.elements.Length > this.Size)
                {
                    Array.Resize(ref this.elements, this.Size);
                }

                // ReSharper disable once InconsistentNaming
                for (var i = 0; i < this.Size; i++)
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
            return this.elements.Any(_Element => EqualityComparer<T>.Default.Equals(_Element, _Item));
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
            if (this.Count + 1 > this.Size)
            {
                Array.Resize(ref this.elements, this.Size + 1);    
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = this.lastUsedIndex; i > _Index; i--)
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
            // ReSharper disable once VariableHidesOuterVariable
            return this.elements.FindIndex(_Item, (_Element, _Item) => EqualityComparer<T>.Default.Equals(_Element, _Item));
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
                this.elements[_Index] = this.elements[_Index + 1];    
            }
                
            this.lastUsedIndex--;
                
            this.ResizeOrDefault();
        }
        
        /// <summary>
        /// Removes the element at the given <c>_Index</c>, without maintaining the order.
        /// </summary>
        /// <param name="_Index">The index in <see cref="elements"/> to remove.</param>
        public void RemoveAtUnordered(int _Index)
        {
            this.elements[_Index] = this.elements[this.lastUsedIndex--];
            
            this.ResizeOrDefault();
        }

        /// <summary>
        /// Resizes the <see cref="elements"/> <see cref="Array"/> if it is greater than <see cref="Size"/>, <br/>
        /// or set the element at <see cref="lastUsedIndex"/> to <c>default</c>.
        /// </summary>
        private void ResizeOrDefault()
        {
            if (this.elements.Length > this.Size)
            {
                Array.Resize(ref this.elements, Mathf.Max(this.Size, this.Count));
            }
            else
            {
                this.elements[this.lastUsedIndex + 1] = default;
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