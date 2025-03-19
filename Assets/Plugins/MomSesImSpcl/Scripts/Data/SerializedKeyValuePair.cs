using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// SerializedKeyValuePair is a serializable struct that represents a key/value pair, specifically designed to be used with Unity's serialization system.
    /// It uses Odin Inspector attributes for better editor integration and customization.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="V">The type of the value.</typeparam>
    [Serializable]
    public struct SerializedKeyValuePair<K,V>
    {
        #region Inspector Fields
        [Tooltip("Represents the key in a serialized key/value pair.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup, Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField] private K key;
        [Tooltip("Represents the value in a serialized key/value pair.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup, Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField] private V value;
        #endregion
        
#if UNITY_EDITOR
        #region Constants
        /// <summary>
        /// Refactor resistant name for <see cref="key"/>. <br/>
        /// <i>For custom <see cref="PropertyDrawer"/>.</i>
        /// </summary>
        public const string KEY = nameof(key);
        /// <summary>
        /// Refactor resistant name for <see cref="value"/>. <br/>
        /// <i>For custom <see cref="PropertyDrawer"/>.</i>
        /// </summary>
        public const string VALUE = nameof(value);
        #endregion 
#endif
        #region Properties
        /// <summary>
        /// <see cref="key"/>.
        /// </summary>
        public K Key { get => this.key; set => this.key = value; }
        /// <summary>
        /// <see cref="value"/>.
        /// </summary>
        public V Value { get => this.value; set => this.value = value; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly creates a new <see cref="SerializedKeyValuePair{K,V}"/> from the given <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_KeyValuePair">The <see cref="KeyValuePair{TKey,TValue}"/> to create a <see cref="SerializedKeyValuePair{K,V}"/> from.</param>
        /// <returns>A new <see cref="SerializedKeyValuePair{K,V}"/> with the value of the given <see cref="KeyValuePair{TKey,TValue}"/>.</returns>
        public static implicit operator SerializedKeyValuePair<K,V>(KeyValuePair<K,V> _KeyValuePair)
        {
            return new SerializedKeyValuePair<K,V>(_KeyValuePair.Key, _KeyValuePair.Value);
        }
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="SerializedKeyValuePair{K,V}"/> from the given <c>_Key</c> and <c>_Value</c>.
        /// </summary>
        /// <param name="_Key"><see cref="key"/>.</param>
        /// <param name="_Value"><see cref="value"/>.</param>
        public SerializedKeyValuePair(K _Key, V _Value)
        {
            this.key = _Key;
            this.value = _Value;
        }

        /// <summary>
        /// Creates a new <see cref="SerializedKeyValuePair{K,V}"/> from the given <see cref="KeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_KeyValuePair"><see cref="KeyValuePair{TKey,TValue}"/>.</param>
        public SerializedKeyValuePair(KeyValuePair<K,V> _KeyValuePair)
        {
            this.key = _KeyValuePair.Key;
            this.value = _KeyValuePair.Value;
        }
        
        /// <summary>
        /// Creates a new <see cref="SerializedKeyValuePair{K,V}"/> from the given <see cref="SerializedKeyValuePair{TKey,TValue}"/>.
        /// </summary>
        /// <param name="_SerializedKeyValuePair"><see cref="SerializedKeyValuePair{K,V}"/>.</param>
        public SerializedKeyValuePair(SerializedKeyValuePair<K,V> _SerializedKeyValuePair)
        {
            this.key = _SerializedKeyValuePair.Key;
            this.value = _SerializedKeyValuePair.Value;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Deconstructor for <see cref="SerializedKeyValuePair{K,V}"/>.
        /// </summary>
        /// <param name="_Key"><see cref="Key"/>.</param>
        /// <param name="_Value"><see cref="Value"/>.</param>
        public void Deconstruct(out K _Key, out V _Value)
        {
            _Key = this.key;
            _Value = this.value;
        }
        #endregion
    }
}