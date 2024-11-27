#nullable enable
using System;
using UnityEngine;

namespace IfLoooop.Data
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    
    /// <summary>
    /// Represents a serialized tuple with two elements of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the tuple.</typeparam>
    [Serializable]
    public sealed class SerializedTuple<T>
    {
        #region Inspector Fields
        [HorizontalGroup]
        [LabelText("$" + nameof(Item1Label))][Tooltip("The first item of the serialized tuple.")]
        [SerializeField] private T? item1;
        [HorizontalGroup]
        [LabelText("$" + nameof(Item2Label))][Tooltip("The second item of the serialized tuple.")]
        [SerializeField] private T? item2;
        #endregion
        
        #region Properties
        /// <summary>
        /// Inspector name for <see cref="item1"/>.
        /// </summary>
        public string Item1Label { get; set; } = nameof(item1);
        /// <summary>
        /// Inspector name for <see cref="item2"/>.
        /// </summary>
        public string Item2Label { get; set; } = nameof(item2);
        
        /// <summary>
        /// <see cref="item1"/>.
        /// </summary>
        public T? Item1 => this.item1;
        /// <summary>
        /// <see cref="item2"/>.
        /// </summary>
        public T? Item2 => this.item2;
        #endregion
    } 
#endif
}