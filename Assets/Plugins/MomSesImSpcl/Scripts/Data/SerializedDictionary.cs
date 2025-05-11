#if ODIN_INSPECTOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// A <see cref="Dictionary{K,V}"/> of which the elements can be displayed in the inspector.
    /// </summary>
    /// <typeparam name="K">The <see cref="Type"/> of the <see cref="Dictionary{K,V}.Keys"/>.</typeparam>
    /// <typeparam name="V">The <see cref="Type"/> of the <see cref="Dictionary{K,V}.Values"/>.</typeparam>
    [Serializable]
    public sealed class SerializedDictionary<K,V> : IDictionary<K,V>
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
#if UNITY_EDITOR
        #region Inspector Fields
        /// <summary>
        /// Only used in editor to display all <see cref="dictionary"/> entries in the inspector.
        /// </summary>
        [Tooltip("Contains all elements of the underlying Dictionary.")]
        [SerializeField] private List<SerializedKeyValuePair<K,V>> list;
        #endregion
#endif
        #region Fields
        /// <summary>
        /// The underlying <see cref="Dictionary{K,V}"/> of this <see cref="SerializedDictionary{K,V}"/>.
        /// </summary>
        private readonly Dictionary<K,V> dictionary;
        #endregion
        
        #region Properties
        /// <summary>
        /// All <see cref="Dictionary{K,V}.Keys"/> in this <see cref="dictionary"/>.
        /// </summary>
        public ICollection<K> Keys => this.dictionary.Keys;
        /// <summary>
        /// All <see cref="Dictionary{K,V}.Values"/> in this <see cref="dictionary"/>.
        /// </summary>
        public ICollection<V> Values => this.dictionary.Values;
        /// <summary>
        /// The total number of <see cref="KeyValuePair{TKey,TValue}"/> in this <see cref="dictionary"/>.
        /// </summary>
        public int Count => this.dictionary.Count;
        /// <summary>
        /// Indicates whether this <see cref="SerializedDictionary{K,V}"/> is readonly or not.
        /// </summary>
        public bool IsReadOnly => false;
        #endregion
        
        #region Indexer
        /// <summary>
        /// Gets or set the value associated with the given <see cref="KeyValuePair{TKey,TValue}.Key"/> in this <see cref="dictionary"/>. <br/>
        /// <i>If the <see cref="KeyValuePair{TKey,TValue}.Key"/> doesn't exist, it will be added.</i> <br/>
        /// <i>Otherwise the <see cref="KeyValuePair{TKey,TValue}.Value"/> for the given <see cref="KeyValuePair{TKey,TValue}.Key"/> will be updated.</i>
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to get/set the <see cref="KeyValuePair{TKey,TValue}.Value"/> for/of.</param>
        public V this[K _Key]
        {
            get => this.dictionary[_Key];
            set
            {
                if (!this.dictionary.TryAdd(_Key, value))
                {
                    this.dictionary[_Key] = value;
                }
#if UNITY_EDITOR
                if (this.list.FindIndex(_Kvp => Equals(_Kvp.Key, _Key)) is var _index && _index != -1)
                {
                    this.list[_index] = new KeyValuePair<K,V>(_Key, value);
                }
                else
                {
                    this.list.Add(new KeyValuePair<K,V>(_Key, value));
                }
#endif
            }
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/>.
        /// </summary>
        public SerializedDictionary()
        {
            this.dictionary = new Dictionary<K,V>();
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>();
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> with the elements of the given <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_Dictionary"><see cref="IDictionary{TKey,TValue}"/>.</param>
        public SerializedDictionary(IDictionary<K,V> _Dictionary)
        {
            this.dictionary = new Dictionary<K,V>(_Dictionary);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_Dictionary.Select(_Kvp => new SerializedKeyValuePair<K,V>(_Kvp)));
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> with the elements of the given <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_Dictionary"><see cref="IDictionary{TKey,TValue}"/>.</param>
        /// <param name="_Comparer"><see cref="IEqualityComparer{T}"/>.</param>
        public SerializedDictionary(IDictionary<K,V> _Dictionary, IEqualityComparer<K> _Comparer)
        {
            this.dictionary = new Dictionary<K,V>(_Dictionary, _Comparer);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_Dictionary.Select(_Kvp => new SerializedKeyValuePair<K,V>(_Kvp)));
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> from the given <c>_Collection</c>.
        /// </summary>
        /// <param name="_Collection"><see cref="IEnumerable{T}"/>.</param>
        public SerializedDictionary(IEnumerable<KeyValuePair<K,V>> _Collection)
        {
            var _array = _Collection.ToArray();
            this.dictionary = new Dictionary<K,V>(_array);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_array.Select(_Kvp => new SerializedKeyValuePair<K,V>(_Kvp)));
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> from the given <c>_Collection</c>.
        /// </summary>
        /// <param name="_Collection"><see cref="IEnumerable{T}"/>.</param>
        /// <param name="_Comparer"><see cref="IEqualityComparer{T}"/>.</param>
        public SerializedDictionary(IEnumerable<KeyValuePair<K,V>> _Collection, IEqualityComparer<K> _Comparer)
        {
            var _array = _Collection.ToArray();
            this.dictionary = new Dictionary<K,V>(_array, _Comparer);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_array.Select(_Kvp => new SerializedKeyValuePair<K,V>(_Kvp)));
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> with the given <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="_Comparer"><see cref="IEqualityComparer{T}"/>.</param>
        public SerializedDictionary(IEqualityComparer<K> _Comparer)
        {
            this.dictionary = new Dictionary<K,V>(_Comparer);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>();
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> with the given <c>_Capacity</c>.
        /// </summary>
        /// <param name="_Capacity">The initial size of the <see cref="dictionary"/>.</param>
        public SerializedDictionary(int _Capacity)
        {
            this.dictionary = new Dictionary<K,V>(_Capacity);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_Capacity);
#endif
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedDictionary{K,V}"/> with the given <c>_Capacity</c> and <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="_Capacity">The initial size of the <see cref="dictionary"/>.</param>
        /// <param name="_Comparer"><see cref="IEqualityComparer{T}"/>.</param>
        public SerializedDictionary(int _Capacity, IEqualityComparer<K> _Comparer)
        {
            this.dictionary = new Dictionary<K,V>(_Capacity, _Comparer);
#if UNITY_EDITOR
            this.list = new List<SerializedKeyValuePair<K,V>>(_Capacity);
#endif
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Creates a new <see cref="KeyValuePair{TKey,TValue}"/> with the given <see cref="KeyValuePair{TKey,TValue}.Key"/> and <see cref="KeyValuePair{TKey,TValue}.Value"/> and adds it to the <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_Key"><see cref="KeyValuePair{TKey,TValue}.Key"/>.</param>
        /// <param name="_Value"><see cref="KeyValuePair{TKey,TValue}.Value"/>.</param>
        public void Add(K _Key, V _Value)
        {
            this.dictionary.Add(_Key, _Value);
#if UNITY_EDITOR
            this.list.Add(new KeyValuePair<K, V>(_Key, _Value));
#endif
        }
        
        /// <summary>
        /// Adds the given <see cref="KeyValuePair{TKey,TValue}"/> to the <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_KeyValuePair"><see cref="KeyValuePair"/>.</param>
        public void Add(KeyValuePair<K, V> _KeyValuePair)
        {
            this.dictionary.Add(_KeyValuePair.Key, _KeyValuePair.Value);
#if UNITY_EDITOR
            this.list.Add(_KeyValuePair);
#endif
        }

        /// <summary>
        /// Tries to remove the <see cref="KeyValuePair{TKey,TValue}"/> with the given <see cref="KeyValuePair{TKey,TValue}.Key"/> from the <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> that should be removed.</param>
        /// <returns><c>true</c> if the <see cref="KeyValuePair{TKey,TValue}.Key"/> has been found and removed, otherwise <c>false</c>.</returns>
        public bool Remove(K _Key)
        {
            var _hasBeenRemoved = this.dictionary.Remove(_Key);
#if UNITY_EDITOR
            var _index = this.list.FindIndex(_Kvp => Equals(_Kvp.Key, _Key));
            this.list.RemoveAt(_index);
#endif
            return _hasBeenRemoved;
        }
        
        /// <summary>
        /// Tries to remove the given <see cref="KeyValuePair{TKey,TValue}"/> from the <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> that should be removed.</param>
        /// <returns><c>true</c> if the <see cref="KeyValuePair{TKey,TValue}"/> has been found and removed, otherwise <c>false</c>.</returns>
        public bool Remove(KeyValuePair<K, V> _KeyValuePair)
        {
            var _hasBeenRemoved = this.dictionary.Remove(_KeyValuePair.Key);
#if UNITY_EDITOR
            var _index = this.list.FindIndex(_Kvp => Equals(_Kvp.Key, _KeyValuePair.Key));
            this.list.RemoveAt(_index);
#endif
            return _hasBeenRemoved;
        }
        
        /// <summary>
        /// Tries to get the <see cref="KeyValuePair{TKey,TValue}.Value"/> for the given <see cref="KeyValuePair{TKey,TValue}.Key"/> from the <see cref="dictionary"/>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to get the <see cref="KeyValuePair{TKey,TValue}.Value"/> for.</param>
        /// <param name="_Value">The <see cref="KeyValuePair{TKey,TValue}.Value"/> to get.</param>
        /// <returns><c>true</c> if the <see cref="KeyValuePair{TKey,TValue}.Value"/> for the given <see cref="KeyValuePair{TKey,TValue}.Key"/> has been found, otherwise <c>false</c>.</returns>
        public bool TryGetValue(K _Key, out V _Value)
        {
            return this.dictionary.TryGetValue(_Key, out _Value);
        }
        
        /// <summary>
        /// Checks if the <see cref="dictionary"/> contains the given <see cref="KeyValuePair{TKey,TValue}.Key"/>.
        /// </summary>
        /// <param name="_Key">The <see cref="KeyValuePair{TKey,TValue}.Key"/> to search for.</param>
        /// <returns><c>true</c> if the <see cref="dictionary"/> contains the given <see cref="KeyValuePair{TKey,TValue}.Key"/>, otherwise <c>false</c>.</returns>
        public bool ContainsKey(K _Key)
        {
            return this.dictionary.ContainsKey(_Key);
        }
        
        /// <summary>
        /// Checks if the <see cref="dictionary"/> contains the given <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to search for.</param>
        /// <returns><c>true</c> if the <see cref="dictionary"/> contains the given <see cref="KeyValuePair{TKey,TValue}"/>, otherwise <c>false</c>.</returns>
        public bool Contains(KeyValuePair<K, V> _KeyValuePair)
        {
            return this.dictionary.Contains(_KeyValuePair);
        }

        /// <summary>
        /// Removes every <see cref="KeyValuePair{TKey,TValue}"/> from this <see cref="dictionary"/>.
        /// </summary>
        public void Clear()
        {
            this.dictionary.Clear();
#if UNITY_EDITOR
            this.list.Clear();
#endif
        }
        
        /// <summary>
        /// Copies the elements of the given <c>_Array</c> to into this <see cref="dictionary"/>, starting at the given <c>_ArrayIndex</c>.
        /// </summary>
        /// <param name="_Array">The elements to copy into <see cref="dictionary"/>.</param>
        /// <param name="_ArrayIndex">The index in <c>_Array</c> at which to start the copying.</param>
        public void CopyTo(KeyValuePair<K,V>[] _Array, int _ArrayIndex)
        {
            ((ICollection<KeyValuePair<K,V>>)this.dictionary).CopyTo(_Array, _ArrayIndex);
#if UNITY_EDITOR
            var _serializedKeyValuePairs = new SerializedKeyValuePair<K, V>[_Array.Length];
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _Array.Length; i++)
            {
                _serializedKeyValuePairs[i] = new SerializedKeyValuePair<K, V>(_Array[i].Key, _Array[i].Value);
            }
            
            this.list.CopyTo(_serializedKeyValuePairs, _ArrayIndex);
#endif
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
        
#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            this.list.Clear();
            
            foreach (var _kvp in this.dictionary)
            {
                this.list.Add(new SerializedKeyValuePair<K,V>(_kvp.Key, _kvp.Value));
            }
        }
        
        public void OnAfterDeserialize()
        {
            this.dictionary.Clear();
            
            foreach (var _kvp in this.list)
            {
                this.dictionary[_kvp.Key] = _kvp.Value;
            }
        }
#endif
        #endregion
    }
}
#endif
