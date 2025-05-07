#if FMOD
using System;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains the name and value of a labeled parameter in FMOD.
    /// </summary>
    [Serializable]
    public struct ParameterLabel
    {
        #region Inspector Fields
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The name of this parameter label.")]
        [SerializeField] private string label;
#endif
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The value/index of this label.")]
        [SerializeField] private int value;
        #endregion
        
        #region Properties
#if UNITY_EDITOR
        /// <summary>
        /// <see cref="label"/>.
        /// </summary>
        public string Label => this.label;
#endif
        /// <summary>
        /// <see cref="value"/>.
        /// </summary>
        public int Value => this.value;
        #endregion
        
        #region Operators
#if UNITY_EDITOR
        /// <summary>
        /// Implicitly returns the <see cref="label"/> of the given <see cref="ParameterLabel"/>.
        /// </summary>
        /// <param name="_ParameterLabel">The <see cref="ParameterLabel"/> to get the <see cref="label"/> of.</param>
        /// <returns>The <see cref="label"/> of the given <see cref="ParameterLabel"/>.</returns>
        public static implicit operator string(ParameterLabel _ParameterLabel) => _ParameterLabel.label;
#endif
        /// <summary>
        /// Implicitly returns the <see cref="value"/> of the given <see cref="ParameterLabel"/>.
        /// </summary>
        /// <param name="_ParameterLabel">The <see cref="ParameterLabel"/> to get the <see cref="value"/> of.</param>
        /// <returns>The <see cref="value"/> of the given <see cref="ParameterLabel"/>.</returns>
        public static implicit operator int(ParameterLabel _ParameterLabel) => _ParameterLabel.value;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ParameterLabel"/>.
        /// </summary>
        /// <param name="_Label"><see cref="label"/>.</param>
        /// <param name="_Value"><see cref="value"/>.</param>
        public ParameterLabel(string _Label, int _Value)
        {
#if UNITY_EDITOR
            this.label = _Label;
#endif
            this.value = _Value;
        }
        #endregion
        
#if UNITY_EDITOR
        #region Methods
        /// <summary>
        /// Returns the <see cref="label"/>.
        /// </summary>
        /// <returns><see cref="label"/>.</returns>
        public override string ToString()
        {
            return this.label;
        }
        #endregion
#endif
    }
}
#endif
