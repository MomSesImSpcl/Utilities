#if FMOD
using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using MomSesImSpcl.Data;
using MomSesImSpcl.Utilities;
using MomSesImSpcl.Utilities.Pooling;

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
        /// <summary>
        /// Maps every <see cref="PARAMETER_ID"/> of a <see cref="EventReference"/> to their name.
        /// </summary>
        private static readonly Dictionary<EventReferenceGuid, Dictionary<string, PARAMETER_ID>> parameterIds = new();
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns the <see cref="EventDescription"/> for this <see cref="EventReference"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the <see cref="EventDescription"/> of.</param>
        /// <returns>The <see cref="EventDescription"/> for this <see cref="EventReference"/>.</returns>
        public static EventDescription GetEventDescription(this EventReference _EventReference)
        {
            return RuntimeManager.GetEventDescription(_EventReference);
        }
        
        /// <summary>
        /// Returns the <see cref="EventDescription"/> for this <see cref="EventReference"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the <see cref="EventDescription"/> of.</param>
        /// <param name="_IsValid"><c>true</c> if the <see cref="EventDescription"/> is valid, otherwise <c>false</c>.</param>
        /// <returns>The <see cref="EventDescription"/> for this <see cref="EventReference"/>.</returns>
        public static EventDescription GetEventDescription(this EventReference _EventReference, out bool _IsValid)
        {
            var _eventDescription = RuntimeManager.GetEventDescription(_EventReference);

            _IsValid = _eventDescription.isValid();
            
            return _eventDescription;
        }
        
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
        /// <param name="_EventReferences">Every <see cref="EventReference"/> in <see cref="eventInstances"/> for the given <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the previous <see cref="EventInstance"/> is valid, otherwise <c>false</c>.</returns>
        private static bool GetOrAddEventInstances(Type _Type, EventReference _EventReference, out EventInstance _PreviousEventInstance, out EventInstance _NewEventInstance, out Dictionary<EventReference, EventInstance> _EventReferences)
        {
            _PreviousEventInstance = default;
            _NewEventInstance = RuntimeManager.CreateInstance(_EventReference);
            
            if (eventInstances.TryGetValue(_Type, out _EventReferences))
            {
                if (_EventReferences.TryGetValue(_EventReference, out _PreviousEventInstance))
                {
                    _EventReferences[_EventReference] = _NewEventInstance;
                }
                else
                {
                    _EventReferences.Add(_EventReference, _NewEventInstance);
                }
            }
            else
            {
                eventInstances.Add(_Type, new Dictionary<EventReference, EventInstance>(EventReferenceComparer.Instance)
                {
                    { _EventReference, _NewEventInstance }
                });
            }

            return _PreviousEventInstance.isValid();
        }
        
        /// <summary>
        /// Retrieves or adds a cached <see cref="PARAMETER_ID"/> from/to <see cref="parameterIds"/> for the given <see cref="EventReference"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the <see cref="PARAMETER_ID"/> for.</param>
        /// <param name="_ParameterName">The FMOD parameter to get the <see cref="PARAMETER_ID"/> of.</param>
        /// <returns>The <see cref="PARAMETER_ID"/> for the given parameter name.</returns>
        private static PARAMETER_ID GetOrAddCachedParameterId(EventReference _EventReference, string _ParameterName)
        {
            var _eventReferencePath = new EventReferenceGuid(_EventReference);
            PARAMETER_ID _parameterId;

            if (parameterIds.TryGetValue(_eventReferencePath, out var _parameterIds))
            {
                if (!_parameterIds.TryGetValue(_ParameterName, out _parameterId))
                {
                    _parameterId = _EventReference.GetParameterId(_ParameterName);
                    _parameterIds.Add(_ParameterName, _parameterId);   
                }
            }
            else
            {
                _parameterId = _EventReference.GetParameterId(_ParameterName);
                parameterIds.Add(_eventReferencePath, new Dictionary<string, PARAMETER_ID>()
                {
                    { _ParameterName, _parameterId }
                });
            }

            return _parameterId;
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_ParameterName">The name of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        public static void PlayOneShot(this EventReference _EventReference, string _ParameterName, float _ParameterValue)
        {
            var _parameterId = GetOrAddCachedParameterId(_EventReference, _ParameterName);
            var _eventInstance = RuntimeManager.CreateInstance(_EventReference);
            _eventInstance.setParameterByID(_parameterId, _ParameterValue);
            _eventInstance.start();
            _eventInstance.release();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_ParameterNames">The names of the parameters in FMOD.</param>
        /// <param name="_ParameterValues">The values to set the parameters to.</param>
        public static void PlayOneShot(this EventReference _EventReference, string[] _ParameterNames, float[] _ParameterValues)
        {
            var _length = _ParameterNames.Length;
            var _parameterIds = ArrayPool<PARAMETER_ID>.Get(_length);
            var _eventInstance = RuntimeManager.CreateInstance(_EventReference);
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _ParameterNames.Length; i++)
            {
                _parameterIds[i] = GetOrAddCachedParameterId(_EventReference, _ParameterNames[i]);
            }
            
            _eventInstance.setParametersByIDs(_parameterIds, _ParameterValues, _length);
            _eventInstance.start();
            _eventInstance.release();
            _parameterIds.ReturnToArrayPool();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_Type">Identifier from where this is called from.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <returns>The <see cref="EventInstance"/> that was created to play the sound.</returns>
        public static void PlayOneShot(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode)
        {
            if (GetOrAddEventInstances(_Type, _EventReference, out var _previousEventInstance, out var _newEventInstance, out var _eventReferences))
            {
                StopAndRelease(_previousEventInstance, _eventReferences, _EventReference, _StopMode);
            }
            
            _newEventInstance.start();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_Type">Identifier from where this is called from.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <param name="_ParameterName">The name of the parameter in FMOD.</param>
        /// <param name="_ParameterValue">The value to set the parameter to.</param>
        public static void PlayOneShot(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode, string _ParameterName, float _ParameterValue)
        {
            if (GetOrAddEventInstances(_Type, _EventReference, out var _previousEventInstance, out var _newEventInstance, out var _eventReferences))
            {
                StopAndRelease(_previousEventInstance, _eventReferences, _EventReference, _StopMode);
            }

            var _parameterId = GetOrAddCachedParameterId(_EventReference, _ParameterName);
            
            _newEventInstance.setParameterByID(_parameterId, _ParameterValue);
            _newEventInstance.start();
        }
        
        /// <summary>
        /// Plays the audio for the given <see cref="EventReference"/> and set the value for the parameter with the given <see cref="PARAMETER_ID"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the audio event to play.</param>
        /// <param name="_Type">Identifier from where this is called from.</param>
        /// <param name="_StopMode">Determines how the previous event should be stopped.</param>
        /// <param name="_ParameterNames">The names of the parameters in FMOD.</param>
        /// <param name="_ParameterValues">The values to set the parameters to.</param>
        public static void PlayOneShot(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode, string[] _ParameterNames, float[] _ParameterValues)
        {
            if (GetOrAddEventInstances(_Type, _EventReference, out var _previousEventInstance, out var _newEventInstance, out var _eventReferences))
            {
                StopAndRelease(_previousEventInstance, _eventReferences, _EventReference, _StopMode);
            }

            var _length = _ParameterNames.Length;
            var _parameterIds = ArrayPool<PARAMETER_ID>.Get(_length);

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _ParameterNames.Length; i++)
            {
                _parameterIds[i] = GetOrAddCachedParameterId(_EventReference, _ParameterNames[i]);
            }
            
            _newEventInstance.setParametersByIDs(_parameterIds, _ParameterValues, _length);
            _newEventInstance.start();
            _parameterIds.ReturnToArrayPool();
        }

        /// <summary>
        /// Stops the <see cref="EventInstance"/> for the given <see cref="EventReference"/> and <see cref="Type"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> of the <see cref="EventInstance"/> to stop.</param>
        /// <param name="_Type">Identifier in <see cref="eventInstances"/>.</param>
        /// <param name="_StopMode"><see cref="STOP_MODE"/>.</param>
        public static void Stop(this EventReference _EventReference, Type _Type, STOP_MODE _StopMode)
        {
            if (!eventInstances.TryGetValue(_Type, out var _eventReferences))
            {
                return;
            }

            if (!_eventReferences.TryGetValue(_EventReference, out var _eventInstance))
            {
                return;
            }

            if (!_eventInstance.isValid())
            {
                return;
            }
            
            StopAndRelease(_eventInstance, _eventReferences, _EventReference, _StopMode);
        }

        /// <summary>
        /// Stops the given <see cref="EventInstance"/> and releases it if it is valid.
        /// </summary>
        /// <param name="_EventInstance">The <see cref="EventInstance"/> to stop.</param>
        /// <param name="_EventReferences">Should be a <see cref="KeyValuePair{TKey,TValue}.Value"/> from <see cref="eventInstances"/>.</param>
        /// <param name="_EventReference">The <see cref="EventReference"/> for which the <see cref="EventInstance"/> was created.</param>
        /// <param name="_StopMode"><see cref="STOP_MODE"/>.</param>
        private static void StopAndRelease(EventInstance _EventInstance, Dictionary<EventReference, EventInstance> _EventReferences, EventReference _EventReference, STOP_MODE _StopMode)
        {
            if (_EventInstance.stop(_StopMode) is not RESULT.OK)
            {
                return;
            }

            _EventInstance.ReleaseIfValid();
            _EventReferences[_EventReference] = default;
        }
        #endregion
    }
}
#endif
