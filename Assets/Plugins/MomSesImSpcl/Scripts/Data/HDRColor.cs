using System;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains a <see cref="UnityEngine.Color"/> field that is displayed as HDR in the inspector.
    /// </summary>
    [Serializable]
    public struct HDRColor
    {
        #region Inspector Fields
        [Tooltip("A HDR Color.")]
        [ColorUsage(true, true)]
        [SerializeField] private Color hdrColor;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="hdrColor"/>.
        /// </summary>
        public Color Color { get => this.hdrColor; set => this.hdrColor = value; }
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly returns the <see cref="Color"/> value from the given <see cref="HDRColor"/>.
        /// </summary>
        /// <param name="_Color"><see cref="HDRColor"/>.</param>
        /// <returns><see cref="hdrColor"/>.</returns>
        public static implicit operator Color(HDRColor _Color) => _Color.Color;
        /// <summary>
        /// Implicitly creates a new <see cref="HDRColor"/> <see cref="object"/> from the given <see cref="UnityEngine.Color"/>.
        /// </summary>
        /// <param name="_Color"><see cref="UnityEngine.Color"/>.</param>
        /// <returns><see cref="HDRColor"/>.</returns>
        public static implicit operator HDRColor(Color _Color) => new(_Color);
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="HDRColor"/>.
        /// </summary>
        /// <param name="_Color"><see cref="hdrColor"/>.</param>
        public HDRColor(Color _Color)
        {
            this.hdrColor = _Color;
        }
        #endregion
    }
}