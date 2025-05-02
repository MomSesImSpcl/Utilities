#if FMOD
using FMOD.Studio;
using FMODUnity;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension method for <see cref="EventReference"/>.
    /// </summary>
    public static class EventReferenceExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the <see cref="PARAMETER_ID"/> for the given name.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the parameter from.</param>
        /// <param name="_ParameterName">The name of the parameter to get the <see cref="PARAMETER_ID"/> of.</param>
        /// <returns>The <see cref="PARAMETER_ID"/> for the given name.</returns>
        public static PARAMETER_ID GetParameterId(this EventReference _EventReference, string _ParameterName)
        {
            var _eventDescription = RuntimeManager.GetEventDescription(_EventReference);
            _eventDescription.getParameterDescriptionByName(_ParameterName, out var _parameterDescription);

            return _parameterDescription.id;
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        public static void PlayOneShot(this EventReference _EventReference, STOP_MODE _StopMode)
        {
            var _eventInstance = RuntimeManager.CreateInstance(_EventReference);
            _eventInstance.stop(_StopMode);
            _eventInstance.start();
            _eventInstance.release();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_ParameterId">The id of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        public static void PlayOneShot(this EventReference _EventReference, PARAMETER_ID _ParameterId, float _ParameterValue)
        {
            var _eventInstance = RuntimeManager.CreateInstance(_EventReference);
            _eventInstance.setParameterByID(_ParameterId, _ParameterValue);
            _eventInstance.start();
            _eventInstance.release();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <param name="_ParameterId">The id of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        public static void PlayOneShot(this EventReference _EventReference, STOP_MODE _StopMode, PARAMETER_ID _ParameterId, float _ParameterValue)
        {
            var _eventInstance = RuntimeManager.CreateInstance(_EventReference);
            _eventInstance.stop(_StopMode);
            _eventInstance.setParameterByID(_ParameterId, _ParameterValue);
            _eventInstance.start();
            _eventInstance.release();
        }
        #endregion
    }
}
#endif
