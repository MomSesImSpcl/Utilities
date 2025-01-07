#nullable enable
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Data
{
#if ODIN_INSPECTOR
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
        public T? Item1 { get => this.item1; set => this.item1 = value; }
        /// <summary>
        /// <see cref="item2"/>.
        /// </summary>
        public T? Item2 { get => this.item2; set => this.item2 = value; }
        #endregion
    } 
#endif
}
