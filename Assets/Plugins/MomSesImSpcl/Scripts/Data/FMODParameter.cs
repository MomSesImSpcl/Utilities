#if FMOD
using System;
using FMOD;
using FMOD.Studio;
using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains info for a <see cref="PARAMETER_DESCRIPTION"/> and the <see cref="EventDescriptionId"/> it belongs to.
    /// </summary>
    [Serializable]
    public struct FMODParameter
    {
#if UNITY_EDITOR
        #region Constants
        /// <summary>
        /// Name for an unused <see cref="FMODParameter"/>. <br/>
        /// <b>Is only accessible in editor.</b>
        /// </summary>
        public const string NULL_PARAMETER = "NULL";
        #endregion
#endif
            
        #region Fields
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The name of the parameter in FMOD.")] 
        [SerializeField] private string parameterName;
#endif
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        [Tooltip("The GUID of the EventDescription this FMODParameter belongs to.")] 
        [SerializeField] private SerializedGUID eventDescriptionId;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.InlineProperty]
#endif
        [Tooltip("The PARAMETER_ID of the PARAMETER_DESCRIPTION.")] 
        [SerializeField] private SerializedParameterId parameterId;
        #endregion
        
        #region Properties
#if UNITY_EDITOR
        /// <summary>
        /// <see cref="parameterName"/>.
        /// </summary>
        public string ParameterName => this.parameterName;
#endif
        /// <summary>
        /// <see cref="eventDescriptionId"/>.
        /// </summary>
        public GUID EventDescriptionId => this.eventDescriptionId.GUID;
        /// <summary>
        /// <see cref="parameterId"/>.
        /// </summary>
        public PARAMETER_ID ParameterId => this.parameterId.ParameterId;
#if UNITY_EDITOR
        /// <summary>
        /// Gets an <see cref="FMODParameter"/> with all values set to <c>default</c> and <see cref="ParameterName"/> set to <see cref="NULL_PARAMETER"/>.
        /// </summary>
        public static FMODParameter NullParameter { get; } = CreateNullParameter();
#endif
        #endregion
        
#if UNITY_EDITOR
        #region Operators
        /// <summary>
        /// Implicitly returns the <see cref="parameterName"/>.
        /// </summary>
        /// <param name="_FMODParameter">The <see cref="FMODParameter"/> to get the <see cref="parameterName"/> of.</param>
        /// <returns><see cref="parameterName"/>.</returns>
        public static implicit operator string(FMODParameter _FMODParameter) => _FMODParameter.parameterName;
        #endregion
#endif
        #region Constructors
        /// <summary>
        /// <see cref="FMODParameter"/>.
        /// </summary>
        /// <param name="_EventDescription"><see cref="eventDescriptionId"/>.</param>
        /// <param name="_ParameterDescription"><see cref="parameterId"/>.</param>
        internal FMODParameter(EventDescription _EventDescription, PARAMETER_DESCRIPTION _ParameterDescription)
        {
            _EventDescription.getID(out var _eventDescriptionId);
            this.eventDescriptionId = new SerializedGUID(_eventDescriptionId);
            this.parameterId = new SerializedParameterId(_ParameterDescription.id);
#if UNITY_EDITOR
            this.parameterName = _ParameterDescription.GetName();
#endif
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns <c>true</c> if this <see cref="FMODParameter"/> has been correctly initialized, otherwise <c>false</c>.
        /// </summary>
        /// <returns><c>true</c> if this <see cref="FMODParameter"/> has been correctly initialized, otherwise <c>false</c>.</returns>
        public bool IsValid()
        {
            return !this.eventDescriptionId.GUID.IsNull;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Creates an <see cref="FMODParameter"/> with all values set to <c>default</c> and <see cref="ParameterName"/> set to <see cref="NULL_PARAMETER"/>.
        /// </summary>
        /// <returns>An <see cref="FMODParameter"/> with all values set to <c>default</c> and <see cref="ParameterName"/> set to <see cref="NULL_PARAMETER"/>.</returns>
        private static FMODParameter CreateNullParameter()
        {
            var _parameterDescription = new PARAMETER_DESCRIPTION
            { 
                name = new StringWrapper(System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(NULL_PARAMETER))
            };
            
            return new FMODParameter(default, _parameterDescription);
        }
        
        /// <summary>
        /// Returns the <see cref="parameterName"/>.
        /// </summary>
        /// <returns><see cref="parameterName"/>.</returns>
        public override string ToString()
        {
            return this.parameterName;
        }
#endif
        #endregion
    }
}
#endif
