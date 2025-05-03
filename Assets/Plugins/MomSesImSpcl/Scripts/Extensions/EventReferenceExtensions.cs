#if FMOD
using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using MomSesImSpcl.Utilities;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension method for <see cref="EventReference"/>.
    /// </summary>
    public static class EventReferenceExtensions
    {
        #region Fields
        /// <summary>
        /// Maps every created <see cref="EventInstance"/> to the <see cref="EventReference"/> they were created from and the <see cref="Type"/> they were created under.
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<EventReference, EventInstance>> eventInstances = new();
        #endregion
        
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
        /// Creates a new <see cref="EventInstance"/> for the given <see cref="Type"/> and <see cref="EventReference"/> and checks if <see cref="eventInstances"/> already contains a <see cref="EventInstance"/> for the given parameters.
        /// </summary>
        /// <param name="_Type">The <see cref="Type"/> identifier for <see cref="eventInstances"/>.</param>
        /// <param name="_EventReference">The <see cref="EventReference"/> identifier for <see cref="eventInstances"/>.</param>
        /// <param name="_PreviousEventInstance">Will contain the previous <see cref="EventInstance"/> from <see cref="eventInstances"/> or <c>default</c>.</param>
        /// <param name="_NewEventInstance">The newly created <see cref="EventInstance"/>.</param>
        /// <returns><c>true</c> if the previous <see cref="EventInstance"/> is valid, otherwise <c>false</c>.</returns>
        private static bool GetEventInstances(Type _Type, EventReference _EventReference, out EventInstance _PreviousEventInstance, out EventInstance _NewEventInstance)
        {
            _PreviousEventInstance = default;
            _NewEventInstance = RuntimeManager.CreateInstance(_EventReference);
            
            if (eventInstances.TryGetValue(_Type, out var _eventReferences))
            {
                if (_eventReferences.TryGetValue(_EventReference, out _PreviousEventInstance))
                {
                    _eventReferences[_EventReference] = _NewEventInstance;
                }
                else
                {
                    _eventReferences.Add(_EventReference, _NewEventInstance);
                }
            }
            else
            {
                eventInstances.Add(_Type, new Dictionary<EventReference, EventInstance>(EventReferenceComparer.Instance)
                {
                    {  _EventReference, _NewEventInstance }
                });
            }

            return _PreviousEventInstance.isValid();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_Type">Identifier from where this is called from.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <param name="_Release">Set to <c>true</c> to immediately release the <see cref="EventInstance"/> after starting it.</param>
        /// <returns>The <see cref="EventInstance"/> that was created to play the sound.</returns>
        public static void PlayOneShot(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode, bool _Release)
        {
            if (GetEventInstances(_Type, _EventReference, out var _previousEventInstance, out var _newEventInstance))
            {
                _previousEventInstance.stop(_StopMode);
                _previousEventInstance.release();
            }
            
            _newEventInstance.start();
            
            if (_Release)
            {
                _newEventInstance.release();
            }
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
        /// <param name="_Type">Identifier from where this is called from.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <param name="_ParameterId">The id of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        /// <param name="_Release">Set to <c>true</c> to immediately release the <see cref="EventInstance"/> after starting it.</param>
        public static void PlayOneShot(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode, PARAMETER_ID _ParameterId, float _ParameterValue, bool _Release)
        {
            if (GetEventInstances(_Type, _EventReference, out var _previousEventInstance, out var _newEventInstance))
            {
                _previousEventInstance.stop(_StopMode);
                _previousEventInstance.release();
            }
            
            _newEventInstance.setParameterByID(_ParameterId, _ParameterValue);
            _newEventInstance.start();

            if (_Release)
            {
                _newEventInstance.release();
            }
        }
        #endregion
    }
}
#endif
