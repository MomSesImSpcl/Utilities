using System;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Represents a serialized tuple with two elements of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the tuple.</typeparam>
    [Serializable]
    public struct SerializedTuple<T>
    {
        #region Inspector Fields
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
        [Sirenix.OdinInspector.LabelText("$" + nameof(Item1Label))]
#endif
        [Tooltip("The first item of the serialized tuple.")]
        [SerializeField] private T item1;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
        [Sirenix.OdinInspector.LabelText("$" + nameof(Item2Label))]
#endif
        [Tooltip("The second item of the serialized tuple.")]
        [SerializeField] private T item2;
        #endregion
        
        #region Properties
#if UNITY_EDITOR && ODIN_INSPECTOR
        /// <summary>
        /// Refactor resistant name for <see cref="item1"/>. <br/>
        /// <i>For custom <see cref="PropertyDrawer"/>.</i>
        /// </summary>
        public string Item1Label { get; set; }
        /// <summary>
        /// Refactor resistant name for <see cref="item2"/>. <br/>
        /// <i>For custom <see cref="PropertyDrawer"/>.</i>
        /// </summary>
        public string Item2Label { get; set; }
#endif
        /// <summary>
        /// <see cref="item1"/>.
        /// </summary>
        public T Item1 { get => this.item1; set => this.item1 = value; }
        /// <summary>
        /// <see cref="item2"/>.
        /// </summary>
        public T Item2 { get => this.item2; set => this.item2 = value; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="SerializedTuple{T}"/> <see cref="object"/>.
        /// </summary>
        /// <param name="_Item1"><see cref="item1"/>.</param>
        /// <param name="_Item2"><see cref="item2"/>.</param>
        public SerializedTuple(T _Item1, T _Item2)
        {
            this.item1 = _Item1;
            this.item2 = _Item2;
#if UNITY_EDITOR && ODIN_INSPECTOR
            this.Item1Label = nameof(this.item1);
            this.Item2Label = nameof(this.item2);
#endif
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Deconstructor for <see cref="SerializedTuple{T}"/>.
        /// </summary>
        /// <param name="_Item1"><see cref="item1"/>.</param>
        /// <param name="_Item2"><see cref="item2"/>.</param>
        public void Deconstruct(out T _Item1, out T _Item2)
        {
                _Item1 = this.item1;
                _Item2 = this.item2;
        }
        #endregion
    } 
}