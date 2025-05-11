using System;
using MomSesImSpcl.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Holds 4 <see cref="float"/> values.
    /// </summary>
    [Serializable, InlineProperty]
    public struct Float4Parameter
    {
        #region Inspector Fields
        [Tooltip("Holds the 4 float values."), HorizontalGroup]
        [SerializeField, HideLabel] private Vector4 vector4;
        [Tooltip("Will be false when this Vector4 should be treated as not set."), HorizontalGroup]
        [SerializeField] private bool hasValue;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="vector4"/>.
        /// </summary>
        public Vector4 Vector4 => this.vector4;
        /// <summary>
        /// <see cref="hasValue"/>.
        /// </summary>
        public bool HasValue => this.hasValue;
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly returns the <see cref="vector4"/> from the given <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Float4Parameter">The <see cref="Float4Parameter"/> to get the <see cref="vector4"/> from.</param>
        /// <returns>The <see cref="vector4"/> from the given <see cref="Float4Parameter"/>.</returns>
        public static implicit operator Vector4(Float4Parameter _Float4Parameter) => _Float4Parameter.vector4;
        /// <summary>
        /// Implicitly returns the <see cref="Vector3"/> from the given <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Float4Parameter">The <see cref="Float4Parameter"/> to get the <see cref="Vector3"/> from.</param>
        /// <returns>The <see cref="Vector3"/> from the given <see cref="Float4Parameter"/>.</returns>
        public static implicit operator Vector3(Float4Parameter _Float4Parameter) => _Float4Parameter.vector4;
        /// <summary>
        /// Implicitly returns the <see cref="Vector2"/> from the given <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Float4Parameter">The <see cref="Float4Parameter"/> to get the <see cref="Vector2"/> from.</param>
        /// <returns>The <see cref="Vector2"/> from the given <see cref="Float4Parameter"/>.</returns>
        public static implicit operator Vector2(Float4Parameter _Float4Parameter) => _Float4Parameter.vector4;
        /// <summary>
        /// Implicitly returns the <see cref="Quaternion"/> from the given <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Float4Parameter">The <see cref="Float4Parameter"/> to get the <see cref="Quaternion"/> from.</param>
        /// <returns>The <see cref="Quaternion"/> from the given <see cref="Float4Parameter"/>.</returns>
        public static implicit operator Quaternion(Float4Parameter _Float4Parameter) => _Float4Parameter.vector4.ToQuaternion();
        /// <summary>
        /// Implicitly returns <see cref="hasValue"/> from the given <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Float4Parameter">The <see cref="Float4Parameter"/> to get <see cref="hasValue"/> from.</param>
        /// <returns><see cref="hasValue"/>.</returns>
        public static implicit operator bool(Float4Parameter _Float4Parameter) => _Float4Parameter.hasValue;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_X"><see cref="Vector4.x"/>.</param>
        /// <param name="_Y"><see cref="Vector4.y"/>.</param>
        /// <param name="_Z"><see cref="Vector4.z"/>.</param>
        /// <param name="_W"><see cref="Vector4.w"/>.</param>
        /// <param name="_HasValue"><see cref="hasValue"/>.</param>
        public Float4Parameter(float _X = 0, float _Y = 0, float _Z = 0, float _W = 0, bool _HasValue = true)
        {
            this.vector4 = new Vector4(_X, _Y, _Z, _W);
            this.hasValue = _HasValue;
        }
        
        /// <summary>
        /// <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Vector4"><see cref="vector4"/>.</param>
        /// <param name="_HasValue"><see cref="hasValue"/>.</param>
        public Float4Parameter(Vector4 _Vector4, bool _HasValue)
        {
            this.vector4 = _Vector4;
            this.hasValue = _HasValue;
        }
        
        /// <summary>
        /// <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Vector3"><see cref="vector4"/>.</param>
        /// <param name="_HasValue"><see cref="hasValue"/>.</param>
        public Float4Parameter(Vector3 _Vector3, bool _HasValue)
        {
            this.vector4 = _Vector3;
            this.hasValue = _HasValue;
        }
        
        /// <summary>
        /// <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Vector2"><see cref="vector4"/>.</param>
        /// <param name="_HasValue"><see cref="hasValue"/>.</param>
        public Float4Parameter(Vector2 _Vector2, bool _HasValue)
        {
            this.vector4 = _Vector2;
            this.hasValue = _HasValue;
        }
        
        /// <summary>
        /// <see cref="Float4Parameter"/>.
        /// </summary>
        /// <param name="_Quaternion"><see cref="vector4"/>.</param>
        /// <param name="_HasValue"><see cref="hasValue"/>.</param>
        public Float4Parameter(Quaternion _Quaternion, bool _HasValue)
        {
            this.vector4 = _Quaternion.ToVector4();
            this.hasValue = _HasValue;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns <see cref="vector4"/>.
        /// </summary>
        /// <returns><see cref="vector4"/>.</returns>
        public override string ToString()
        {
            return this.vector4.ToString();
        }
        #endregion
    }
}