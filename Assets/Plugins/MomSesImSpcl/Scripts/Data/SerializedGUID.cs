#if FMOD
using System;
using System.Diagnostics.CodeAnalysis;
using FMOD;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// A serialized version of a FMOD <see cref="FMOD.GUID"/>.
    /// </summary>
    [Serializable, SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct SerializedGUID
    {
        #region Inspector Fields
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("First part of the GUID.")]
        [SerializeField] private int data1;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Second part of the GUID.")]
        [SerializeField] private int data2;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Third part of the GUID.")]
        [SerializeField] private int data3;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Fourth part of the GUID.")]
        [SerializeField] private int data4;
        #endregion
        
        #region Properties
        /// <summary>
        /// A FMOD <see cref="FMOD.GUID"/>.
        /// </summary>
        public GUID GUID => new GUID
        {
            Data1 = this.data1,
            Data2 = this.data2,
            Data3 = this.data3,
            Data4 = this.data4
        };
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly converts this <see cref="SerializedGUID"/> into a FMOD <see cref="FMOD.GUID"/>.
        /// </summary>
        /// <param name="_SerializedGUID">The <see cref="SerializedGUID"/> to convert.</param>
        /// <returns>A FMOD <see cref="FMOD.GUID"/>.</returns>
        public static implicit operator GUID(SerializedGUID _SerializedGUID) => _SerializedGUID.GUID;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="SerializedGUID"/>.
        /// </summary>
        /// <param name="_GUID"><see cref="GUID"/>.</param>
        public SerializedGUID(GUID _GUID)
        {
            this.data1 = _GUID.Data1;
            this.data2 = _GUID.Data2;
            this.data3 = _GUID.Data3;
            this.data4 = _GUID.Data4;
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns <see cref="GUID"/>.
        /// </summary>
        /// <returns><see cref="GUID"/>.</returns>
        public override string ToString()
        {
            return this.GUID.ToString();
        }
        #endregion
    }
}
#endif