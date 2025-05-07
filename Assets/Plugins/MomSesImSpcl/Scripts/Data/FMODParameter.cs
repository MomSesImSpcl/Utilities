#if FMOD
using System;
using FMOD;
using FMOD.Studio;
using MomSesImSpcl.Extensions;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Contains info for a <see cref="PARAMETER_DESCRIPTION"/> and the <see cref="EventDescriptionId"/> it belongs to.
    /// </summary>
    [Serializable]
    public struct FMODParameter
    {
        #region Fields
#if UNITY_EDITOR
        /// <summary>
        /// The name of the parameter in FMOD.
        /// </summary>
        private string parameterName;
#endif
        /// <summary>
        /// The <see cref="GUID"/> of the <see cref="EventDescription"/> the <see cref="PARAMETER_DESCRIPTION"/> belongs to.
        /// </summary>
        private GUID eventDescriptionId;
        /// <summary>
        /// The <see cref="PARAMETER_ID"/> of the <see cref="PARAMETER_DESCRIPTION"/>.
        /// </summary>
        private SerializedParameterId parameterId;
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
        public GUID EventDescriptionId => this.eventDescriptionId;
        /// <summary>
        /// <see cref="parameterId"/>.
        /// </summary>
        public PARAMETER_ID ParameterId => this.parameterId.ParameterId;
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
            _EventDescription.getID(out this.eventDescriptionId);
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
            return !this.eventDescriptionId.IsNull;
        }
        
#if UNITY_EDITOR
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
