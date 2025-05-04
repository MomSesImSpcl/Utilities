#if FMOD
using FMOD.Studio;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="EventInstance"/>.
    /// </summary>
    public static class EventInstanceExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the <see cref="PARAMETER_ID"/> for the given name.
        /// </summary>
        /// <param name="_EventInstance">The <see cref="EventInstance"/> to get the parameter from.</param>
        /// <param name="_ParameterName">The name of the parameter to get the <see cref="PARAMETER_ID"/> of.</param>
        /// <returns>The <see cref="PARAMETER_ID"/> for the given name.</returns>
        public static PARAMETER_ID GetParameterId(this EventInstance _EventInstance, string _ParameterName)
        {
            _EventInstance.getDescription(out var _eventDescription);
            _eventDescription.getParameterDescriptionByName(_ParameterName, out var _parameterDescription);

            return _parameterDescription.id;
        }

        /// <summary>
        /// Releases this <see cref="EventInstance"/> if it is valid.
        /// </summary>
        /// <param name="_EventInstance">The <see cref="EventInstance"/> to release.</param>
        public static void ReleaseIfValid(this EventInstance _EventInstance)
        {
            if (_EventInstance.isValid())
            {
                _EventInstance.release();
            }
        }
        #endregion
    }
}
#endif
