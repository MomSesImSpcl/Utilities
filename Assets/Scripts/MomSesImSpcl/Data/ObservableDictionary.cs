#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// A dictionary that provides notifications when items are added, removed, or updated.
    /// </summary>
    /// <typeparam name="K">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="V">The type of values in the dictionary.</typeparam>
    public sealed class ObservableDictionary<K, V> : IDictionary<K, V>
    {
        #region Fields

        /// <summary>
        /// Underlying dictionary that stores the key-value pairs.
        /// </summary>
        private readonly Dictionary<K, V> dictionary = new();
        #endregion
        
        #region Events
        /// <summary>
        /// Event triggered when an item is added to the dictionary.
        /// </summary>
        /// <remarks>
        /// This event provides the key and value of the item that was added.
        /// </remarks>
        public event Action<K,V>? OnItemAdded;
        /// <summary>
        /// Event triggered when an item is removed from the dictionary.
        /// </summary>
        /// <remarks>
        /// This event provides the key of the item that was removed.
        /// </remarks>
        public event Action<K>? OnItemRemoved;
        /// <summary>
        /// Event that is triggered when an item in the dictionary is updated.
        /// </summary>
        /// <remarks>
        /// This event provides the key and value of the item that was added.
        /// </remarks>
        public event Action<K,V>? OnItemUpdated;
        #endregion
        
        #region Indexer

        /// <summary>
        /// Gets or sets the value associated with the specified key in the dictionary.
        /// If the key does not exist in the dictionary, it adds the key and value and triggers the OnItemAdded event.
        /// If the key exists, it updates the value and triggers the OnItemUpdated event.
        /// </summary>
        /// <param name="_Key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        public V this[K _Key]
        {
            get => this.dictionary[_Key];
            set
            {
                if (!this.dictionary.TryAdd(_Key, value))
                {
                    this.dictionary[_Key] = value;
                    this.OnItemUpdated?.Invoke(_Key, value);
                }
                else
                {
                    this.OnItemAdded?.Invoke(_Key, value);
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Collection of keys contained in the dictionary.
        /// </summary>
        public ICollection<K> Keys => this.dictionary.Keys;
        /// <summary>
        /// Gets a collection containing the values in the dictionary.
        /// </summary>
        public ICollection<V> Values => this.dictionary.Values;
        /// <summary>
        /// Gets the number of key-value pairs contained in the dictionary.
        /// </summary>
        public int Count => this.dictionary.Count;
        /// <summary>
        /// Gets a value indicating whether the dictionary is read-only.
        /// </summary>
        /// <returns>true if the dictionary is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;
        #endregion
        
        #region Methods
        /// <summary>
        /// Adds the given <see cref="KeyValuePair{TKey,TValue}.Key"/> and <see cref="KeyValuePair{TKey,TValue}.Value"/> to <see cref="dictionary"/> and invokes <see cref="OnItemAdded"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to add to <see cref="dictionary"/>.</param>
        public void Add(KeyValuePair<K, V> _KeyValuePair)
        {
            this.Add(_KeyValuePair.Key, _KeyValuePair.Value);
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary and invokes the OnItemAdded event.
        /// </summary>
        /// <param name="_Key">The key to add to the dictionary.</param>
        /// <param name="_Value">The value to add to the dictionary.</param>
        public void Add(K _Key, V _Value)
        {
            this.dictionary.Add(_Key, _Value);
            this.OnItemAdded?.Invoke(_Key, _Value);
        }

        /// <summary>
        /// Removes the given <see cref="KeyValuePair{TKey,TValue}"/> from <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> containing the key to remove from <see cref="dictionary"/>.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if the key was not found in the original <see cref="dictionary"/>.</returns>
        public bool Remove(KeyValuePair<K, V> _KeyValuePair)
        {
            return this.Remove(_KeyValuePair.Key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="ObservableDictionary{K,V}"/> and invokes <see cref="OnItemRemoved"/> if successful.
        /// </summary>
        /// <param name="_Key">The key of the element to remove from the dictionary.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>
        public bool Remove(K _Key)
        {
            if (this.dictionary.Remove(_Key))
            {
                this.OnItemRemoved?.Invoke(_Key);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key from the dictionary.
        /// </summary>
        /// <param name="_Key">The key whose value to get.</param>
        /// <param name="_Value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(K _Key, out V _Value)
        {
            return this.dictionary.TryGetValue(_Key, out _Value);
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="_Key">The key to locate in the dictionary.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(K _Key)
        {
            return this.dictionary.ContainsKey(_Key);
        }

        /// <summary>
        /// Determines whether the dictionary contains a specific key-value pair.
        /// </summary>
        /// <param name="_KeyValuePair">The key-value pair to locate in the dictionary.</param>
        /// <returns>true if the key-value pair is found in the dictionary; otherwise, false.</returns>
        public bool Contains(KeyValuePair<K, V> _KeyValuePair)
        {
            return this.dictionary.Contains(_KeyValuePair);
        }

        /// <summary>
        /// Removes all keys and values from the <see cref="ObservableDictionary{K, V}"/> and invokes <see cref="OnItemRemoved"/> for each removed item.
        /// </summary>
        public void Clear()
        {
            // ReSharper disable once InconsistentNaming
            for (var i = this.dictionary.Count; i >= 0; i--)
            {
                var _key = this.dictionary.Keys.ElementAt(i);
                this.dictionary.Remove(_key);
                this.OnItemRemoved?.Invoke(_key);
            }
        }

        /// <summary>
        /// Copies the elements of the dictionary to an array, starting at a particular array index.
        /// </summary>
        /// <param name="_Array">The one-dimensional array that is the destination of the elements copied from the dictionary. The array must have zero-based indexing.</param>
        /// <param name="_ArrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(KeyValuePair<K, V>[] _Array, int _ArrayIndex)
        {
            ((ICollection<KeyValuePair<K, V>>)this.dictionary).CopyTo(_Array, _ArrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through <see cref="dictionary"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through <see cref="dictionary"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns>
        /// An enumerator for the dictionary.
        /// </returns>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
        #endregion
    }
}