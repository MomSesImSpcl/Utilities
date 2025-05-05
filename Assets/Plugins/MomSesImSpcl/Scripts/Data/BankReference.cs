#if FMOD
using System;
using FMODUnity;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Identifier for a bank in FMOD.
    /// </summary>
    [Serializable]
    public struct BankReference
    {
        #region Fields
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
#endif
        [Tooltip("The bank in FMOD."), BankRef]
        [SerializeField] private string bank;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.HorizontalGroup]
#endif
        [Tooltip("Preloads the sample data alongside the bank if true.")]
        [SerializeField] private bool preloadSamples;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="bank"/>.
        /// </summary>
        public string Bank => this.bank;
        /// <summary>
        /// <see cref="preloadSamples"/>.
        /// </summary>
        public bool PreloadSamples => this.preloadSamples;
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly returns <see cref="bank"/> from this <see cref="BankReference"/>.
        /// </summary>
        /// <param name="_BankReference">The <see cref="BankReference"/> to get the <see cref="bank"/> <see cref="string"/> from.</param>
        /// <returns><see cref="bank"/>.</returns>
        public static implicit operator string(BankReference _BankReference) => _BankReference.bank;
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns <see cref="bank"/>.
        /// </summary>
        /// <returns><see cref="bank"/></returns>
        public override string ToString()
        {
            return this.bank;
        }
        #endregion
    }
}
#endif