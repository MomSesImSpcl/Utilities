using System;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Data
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;

    /// <summary>
    /// Represents a read-only selectable entry containing a serialized key-value pair of type <typeparamref name="V"/>.
    /// </summary>
    /// <typeparam name="V">The type of the value in the serialized key-value pair.</typeparam>
    [Serializable]
    public struct ReadonlySelectableEntry<V>
    {
        #region Inspector Fields
        [Tooltip("Serialized key-value pair of type V with a boolean key in a read-only selectable entry.")]
        [HideLabel]
        [SerializeField] private SerializedKeyValuePair<bool, V> entry;
        #endregion
        
        #region Constants
        /// <summary>
        /// Refactor resistant name for <see cref="Entry"/>. <br/>
        /// <i>For custom <see cref="PropertyDrawer"/>.</i>
        /// </summary>
        public const string ENTRY = nameof(entry);
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the serialized key-value pair of the read-only selectable entry.
        /// </summary>
        public SerializedKeyValuePair<bool, V> Entry { get => this.entry; set => this.entry = value; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ReadonlySelectableEntry{V}"/>.
        /// </summary>
        /// <param name="_Key"><see cref="SerializedKeyValuePair{K,V}.key"/>.</param>
        /// <param name="_Value"><see cref="SerializedKeyValuePair{K,V}.value"/>.</param>
        public ReadonlySelectableEntry(bool _Key, V _Value)
        {
            this.entry = new SerializedKeyValuePair<bool, V>(_Key, _Value);
        }
        #endregion
    } 
#endif
}
