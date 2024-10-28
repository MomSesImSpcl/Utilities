#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IfLoooop.Utilities
{
    /// <summary>
    /// An <see cref="IDictionary{TKey,TValue}"/> with events for <see cref="OnItemAdded"/>, <see cref="OnItemRemoved"/> and <see cref="OnItemUpdated"/>.
    /// </summary>
    public sealed class ObservableDictionary<K, V> : IDictionary<K, V>
    {
        #region Fields
        /// <summary>
        /// The <see cref="Dictionary{TKey,TValue}"/> that hold the data.
        /// </summary>
        private readonly Dictionary<K, V> dictionary = new();
        #endregion
        
        #region Events
        /// <summary>
        /// Will be triggered when an item is added.
        /// </summary>
        public event Action<K,V>? OnItemAdded;
        /// <summary>
        /// Will be triggered when an item is removed.
        /// </summary>
        public event Action<K>? OnItemRemoved;
        /// <summary>
        /// Will be triggered when an item is updated.
        /// </summary>
        public event Action<K,V>? OnItemUpdated;
        #endregion
        
        #region Indexer
        /// <summary>
        /// Invokes the <see cref="OnItemUpdated"/> and <see cref="OnItemAdded"/> events when an item is added or updated.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> in <see cref="dictionary"/>.</param>
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
        /// All <see cref="Dictionary{TKey,TValue}.Keys"/> in <see cref="dictionary"/>.
        /// </summary>
        public ICollection<K> Keys => this.dictionary.Keys;
        /// <summary>
        /// All <see cref="Dictionary{TKey,TValue}.Values"/> in <see cref="dictionary"/>.
        /// </summary>
        public ICollection<V> Values => this.dictionary.Values;
        /// <summary>
        /// The number of <see cref="KeyValuePair{TKey,TValue}"/> in <see cref="dictionary"/>.
        /// </summary>
        public int Count => this.dictionary.Count;
        /// <summary>
        /// Will always be <c>false</c>.
        /// </summary>
        public bool IsReadOnly => false;
        #endregion
        
        #region Methods
        /// <summary>
        /// Adds the given <see cref="KeyValuePair{TKey,TValue}"/> to <see cref="dictionary"/> and invokes <see cref="OnItemAdded"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to add to <see cref="dictionary"/>.</param>
        public void Add(KeyValuePair<K, V> _KeyValuePair)
        {
            this.Add(_KeyValuePair.Key, _KeyValuePair.Value);
        }
        
        /// <summary>
        /// Adds the given <see cref="KeyValuePair{TKey,TValue}.Key"/> and <see cref="KeyValuePair{TKey,TValue}.Value"/> to <see cref="dictionary"/> and invokes <see cref="OnItemAdded"/>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to add to <see cref="dictionary"/>.</param>
        /// <param name="_Value">The <see cref="KeyValuePair{TKey,TValue}.Value"/> to add to <see cref="dictionary"/>.</param>
        public void Add(K _Key, V _Value)
        {
            this.dictionary.Add(_Key, _Value);
            this.OnItemAdded?.Invoke(_Key, _Value);
        }
        
        /// <summary>
        /// Removes the given <see cref="KeyValuePair{TKey,TValue}"/> from <see cref="dictionary"/> and invokes <see cref="OnItemRemoved"/> if the <see cref="KeyValuePair{TKey,TValue}"/> was in <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to remove from <see cref="dictionary"/>.</param>
        /// <returns><c>true</c> if the <see cref="KeyValuePair{TKey,TValue}"/> was in <see cref="dictionary"/>, otherwise <c>false</c>.</returns>
        public bool Remove(KeyValuePair<K, V> _KeyValuePair)
        {
            return this.Remove(_KeyValuePair.Key);
        }
        
        /// <summary>
        /// Removes the given <c>_Key</c> from <see cref="dictionary"/> and invokes <see cref="OnItemRemoved"/> if the <c>_Key</c> was in <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_Key">The <c>_Key</c> to remove from <see cref="dictionary"/>.</param>
        /// <returns><c>true</c> if the <c>_Key</c> was in <see cref="dictionary"/>, otherwise <c>false</c>.</returns>
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
        /// Attempts to get the <see cref="KeyValuePair{TKey,TValue}.Value"/> for the given <c>_Key</c>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to get the <see cref="KeyValuePair{TKey,TValue}.Value"/> for.</param>
        /// <param name="_Value">The <see cref="KeyValuePair{TKey,TValue}.Value"/> of the given <see cref="KeyValuePair{TKey,TValue}.Key"/>.</param>
        /// <returns><c>true</c> if the <c>_Key</c> is in <see cref="dictionary"/>, otherwise <c>false</c>.</returns>
        public bool TryGetValue(K _Key, out V _Value)
        {
            return this.dictionary.TryGetValue(_Key, out _Value);
        }

        /// <summary>
        /// Checks if the given <c>_Key</c> is in <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to look for in <see cref="dictionary"/>.</param>
        /// <returns><c>true</c> if the <c>_Key</c> is in <see cref="dictionary"/>, otherwise <c>false</c>.</returns>
        public bool ContainsKey(K _Key)
        {
            return this.dictionary.ContainsKey(_Key);
        }

        /// <summary>
        /// Checks if the given <see cref="KeyValuePair{TKey,TValue}"/> is in <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to look for in <see cref="dictionary"/>.</param>
        /// <returns><c>true</c> if the <see cref="KeyValuePair{TKey,TValue}"/> is in <see cref="dictionary"/>, otherwise <c>false</c>.</returns>
        public bool Contains(KeyValuePair<K, V> _KeyValuePair)
        {
            return this.dictionary.Contains(_KeyValuePair);
        }

        /// <summary>
        /// Removes all elements from <see cref="dictionary"/> and invokes <see cref="OnItemRemoved"/> on every <see cref="KeyValuePair{TKey,TValue}.Key"/>.
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
        /// Copies the elements from <see cref="dictionary"/> to the given <c>_Array</c>, starting at the given <c>_ArrayIndex</c>.
        /// </summary>
        /// <param name="_Array">The array tro copy the <see cref="dictionary"/> elements into.</param>
        /// <param name="_ArrayIndex">The index in <c>_Array</c> to start copying the <see cref="dictionary"/> elements into.</param>
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
        /// Returns an enumerator that iterates through <see cref="dictionary"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through <see cref="dictionary"/>.</returns>
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }
        #endregion
    }
}