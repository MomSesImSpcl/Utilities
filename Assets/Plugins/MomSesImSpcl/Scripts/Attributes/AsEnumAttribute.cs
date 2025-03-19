using System;
using System.Diagnostics;
using UnityEngine;

namespace MomSesImSpcl.Attributes
{
    /// <summary>
    /// Add this to a number <see cref="Type"/> field, to display the corresponding <see cref="Enum"/> value in the inspector.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public sealed class AsEnumAttribute : PropertyAttribute
    {
        #region Properties
        /// <summary>
        /// The <see cref="Type"/> of the <see cref="Enum"/> to display the value as.
        /// </summary>
        public Type EnumType { get; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="AsEnumAttribute"/>.
        /// </summary>
        /// <param name="_EnumType"><see cref="EnumType"/>.</param>
        public AsEnumAttribute(Type _EnumType)
        {
            this.EnumType = _EnumType;
        }
        #endregion
    }
}