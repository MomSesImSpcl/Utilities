#if FMOD
using System;
using FMOD.Studio;
using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Represents a serializable <see cref="PARAMETER_ID"/>.
    /// </summary>
    [Serializable]
    public struct SerializedParameterId
    {
        #region Inspector Fields
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The lower 32 bits of the PARAMETER_ID.")]
        [SerializeField] private uint data1;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The upper 32 bits of the PARAMETER_ID.")]
        [SerializeField] private uint data2;
        #endregion

        #region Properties
        /// <summary>
        /// This <see cref="SerializedParameterId"/> as a <see cref="PARAMETER_ID"/>.
        /// </summary>
        public PARAMETER_ID ParameterId => new()
        {
            data1 = this.data1, 
            data2 = this.data2
        };
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly converts the given <see cref="SerializedParameterId"/> into a <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_SerializedParameterId">The <see cref="SerializedParameterId"/> to convert to a <see cref="PARAMETER_ID"/>.</param>
        /// <returns>The <see cref="SerializedParameterId"/> as a <see cref="PARAMETER_ID"/>.</returns>
        public static implicit operator PARAMETER_ID(SerializedParameterId _SerializedParameterId) => _SerializedParameterId.ParameterId;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="SerializedParameterId"/>.
        /// </summary>
        /// <param name="_ParameterID">The <see cref="PARAMETER_ID"/> to convert to a <see cref="SerializedParameterId"/>.</param>
        public SerializedParameterId(PARAMETER_ID _ParameterID)
        {
            this.data1 = _ParameterID.data1;
            this.data2 = _ParameterID.data2;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the values of <see cref="data1"/> and <see cref="data2"/>.
        /// </summary>
        /// <returns>The values of <see cref="data1"/> and <see cref="data2"/>.</returns>
        public override string ToString()
        {
            return $"{nameof(this.data1)}: {this.data1.Bold()} | {nameof(this.data2)}: {this.data2.Bold()}";
        }
        #endregion
    }
}
#endif
