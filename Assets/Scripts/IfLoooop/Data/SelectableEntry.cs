using System;
using UnityEngine;

namespace IfLoooop.Data
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    
    /// <summary>
    /// Represents a selectable entry with a generic value.
    /// </summary>
    /// <typeparam name="V">The type of the value in the selectable entry.</typeparam>
    [Serializable]
    public struct SelectableEntry<V>
    {
        #region Inspector Fields
        [Tooltip("A KeyValuePair with a generic value.")]
        [HideLabel]
        [SerializeField] private SerializedKeyValuePair<bool, V> entry;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the serialized key-value pair entry with a boolean key and a generic value.
        /// </summary>
        public SerializedKeyValuePair<bool, V> Entry { get => this.entry; set => this.entry = value; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ReadonlySelectableEntry{V}"/>.
        /// </summary>
        /// <param name="_Key"><see cref="SerializedKeyValuePair{K,V}.key"/>.</param>
        /// <param name="_Value"><see cref="SerializedKeyValuePair{K,V}.value"/>.</param>
        public SelectableEntry(bool _Key, V _Value)
        {
            this.entry = new SerializedKeyValuePair<bool, V>(_Key, _Value);
        }
        #endregion
    } 
#endif
}