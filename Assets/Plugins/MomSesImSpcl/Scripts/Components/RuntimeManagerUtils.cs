#if FMOD
using FMOD.Studio;
using FMODUnity;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Contains utility methods for <see cref="RuntimeManager"/>.
    /// </summary>
    public sealed class RuntimeManagerUtils : RuntimeManager
    {
        #region Methods
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        public static void PlayOneShot(EventReference _EventReference)
        {
            var _eventInstance = CreateInstance(_EventReference);
            _eventInstance.start();
            _eventInstance.release();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_ParameterId">The id of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        public static void PlayOneShot(EventReference _EventReference, PARAMETER_ID _ParameterId, float _ParameterValue)
        {
            var _eventInstance = CreateInstance(_EventReference);
            _eventInstance.setParameterByID(_ParameterId, _ParameterValue);
            _eventInstance.start();
            _eventInstance.release();
        }
        #endregion
    }
}
#endif