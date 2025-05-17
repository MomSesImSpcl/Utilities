using System;
using System.Collections.Generic;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Represents a serialized tuple with two elements of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the tuple.</typeparam>
    [Serializable]
    public struct SerializedTuple<T> : IEquatable<SerializedTuple<T>>
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
#if ODIN_INSPECTOR
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
        
        #region Operators
        /// <summary>
        /// Implicitly converts this <see cref="SerializedTuple{T}"/> to a <see cref="Tuple{T1, T2}"/>.
        /// </summary>
        /// <param name="_SerializedTuple">The <see cref="SerializedTuple{T}"/> to convert.</param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> with the values of this <see cref="SerializedTuple{T}"/>.</returns>
        public static implicit operator (T, T) (SerializedTuple<T> _SerializedTuple) => (_SerializedTuple.item1, _SerializedTuple.item2);

        /// <summary>
        /// Compares <see cref="Item1"/> and <see cref="Item2"/> for the given <see cref="SerializedTuple{T}"/> for equality.
        /// </summary>
        /// <param name="_SerializedTuple1"><see cref="SerializedTuple{T}"/>.</param>
        /// <param name="_SerializedTuple2"><see cref="SerializedTuple{T}"/>.</param>
        /// <returns><c>true</c> if both <see cref="SerializedTuple{T}"/> are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(SerializedTuple<T> _SerializedTuple1, SerializedTuple<T> _SerializedTuple2)
        {
            return EqualityComparer<T>.Default.Equals(_SerializedTuple1.item1, _SerializedTuple2.item1) && EqualityComparer<T>.Default.Equals(_SerializedTuple1.item2, _SerializedTuple2.item2);
        }
        
        /// <summary>
        /// Compares <see cref="Item1"/> and <see cref="Item2"/> for the given <see cref="SerializedTuple{T}"/> for inequality.
        /// </summary>
        /// <param name="_SerializedTuple1"><see cref="SerializedTuple{T}"/>.</param>
        /// <param name="_SerializedTuple2"><see cref="SerializedTuple{T}"/>.</param>
        /// <returns><c>true</c> if both <see cref="SerializedTuple{T}"/> are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(SerializedTuple<T> _SerializedTuple1, SerializedTuple<T> _SerializedTuple2)
        {
            return !(_SerializedTuple1 == _SerializedTuple2);
        }
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
        public bool Equals(SerializedTuple<T> _SerializedTuple)
        {
            return this == _SerializedTuple;
        }
        
        public override bool Equals(object _Object)
        {
            if (_Object is SerializedTuple<T> _serializedTuple)
            {
                return this == _serializedTuple;
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            var _hash1 = this.item1 == null ? 0 : item1.GetHashCode();
            var _hash2 = this.item2 == null ? 0 : item2.GetHashCode();
            
            // Use a hashing strategy that combines both
            return _hash1 * 397 ^ _hash2;
        }
        
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

        /// <summary>
        /// Returns the value of <see cref="item1"/> and <see cref="item2"/>.
        /// </summary>
        /// <returns>The value of <see cref="item1"/> and <see cref="item2"/>.</returns>
        public override string ToString()
        {
            return $"{nameof(this.Item1)}: {this.item1.ToString().Bold()} | {nameof(this.Item2)}: {this.item2.ToString().Bold()}";
        }
        #endregion
    } 
}
