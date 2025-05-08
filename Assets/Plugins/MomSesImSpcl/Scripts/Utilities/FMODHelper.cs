#if FMOD && UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using MomSesImSpcl.Data;
using MomSesImSpcl.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Debug = UnityEngine.Debug;
using INITFLAGS = FMOD.INITFLAGS;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains helper methods for FMOD. <br/>
    /// <b>Only works in  editor.</b>
    /// </summary>
    public static class FMODHelper
    {
        #region Constatns
        /// <summary>
        /// Name of the <c>Master.strings</c> bank. <br/>
        /// <i>No file extension.</i>
        /// </summary>
        private const string MASTER_STRINGS_BANK = "Master.strings";
        #endregion
        
        #region Fields
        /// <summary>
        /// Singleton of the editor FMOD <see cref="FMOD.Studio.System"/>.
        /// </summary>
        private static FMOD.Studio.System fmodSystem;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="fmodSystem"/>.
        /// </summary>
        /// <exception cref="Exception">When <see cref="FMOD.Studio.System"/>.<see cref="FMOD.Studio.System.create"/> or <see cref="FMOD.Studio.System"/>.<see cref="FMOD.Studio.System.initialize"/> don't return <see cref="RESULT.OK"/>.</exception>
        public static FMOD.Studio.System FMODSystem
        {
            get
            {
                if (fmodSystem.isValid())
                {
                    return fmodSystem;
                }
            
                if (FMOD.Studio.System.create(out fmodSystem) is var _createResult && _createResult != RESULT.OK)
                {
                    throw new Exception($"Create: {_createResult}");
                }

                if (fmodSystem.initialize(0, FMOD.Studio.INITFLAGS.NORMAL, INITFLAGS.NORMAL, IntPtr.Zero) is var _initializeResult && _initializeResult != RESULT.OK)
                {
                    throw new Exception($"Initialize: {_initializeResult}");
                }

                LoadBank(MASTER_STRINGS_BANK, out _);
                
                return fmodSystem;
            }
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Releases the <see cref="fmodSystem"/> before every domain reload and on application quit.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Init()
        {
            AssemblyReloadEvents.beforeAssemblyReload += ReleaseSystem;
            EditorApplication.quitting += ReleaseSystem;
            
            return;

            void ReleaseSystem()
            {
                if (fmodSystem.isValid())
                {
                    fmodSystem.unloadAll();
                    fmodSystem.release();  
                    fmodSystem.clearHandle();
                }   
            }
        }
        
        /// <summary>
        /// Searches and loads the <see cref="Bank"/> that contains the given <see cref="EventReference"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to search the <see cref="Bank"/> of.</param>
        /// <returns>The <see cref="Bank"/> containing the given <see cref="EventReference"/>, or <c>null</c> if none could be found.</returns>
        /// <exception cref="Exception">When <see cref="Bank"/>.<see cref="Bank.getEventList"/> doesn't return <see cref="RESULT.OK"/>.</exception>
        public static Bank? GetBank(EventReference _EventReference)
        {
            Bank _bank = default;
            var _banks = Settings.Instance.Banks;
            var _bankFound = false;
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _banks.Count && !_bankFound; i++)
            {
                try
                {
                    if (!IsBankLoaded(_banks[i], out _bank))
                    {
                        LoadBank(_banks[i], out _bank);   
                    }

                    if (_bank.getEventList(out var _eventList) is var _eventListResult && _eventListResult != RESULT.OK)
                    {
                        throw new Exception($"Get Event list: {_eventListResult}");
                    }

                    var _eventDescription = GetEventDescription(_EventReference, out _);
                    
                    foreach (var _bankEvent in _eventList)
                    {
                        _bankEvent.getID(out var _bankEventId);
                        _eventDescription.getID(out var _targetEventId);

                        if (_bankEventId == _targetEventId)
                        {
                            _bankFound = true;
                            break;
                        }
                    }
                }
                finally
                {
                    if (!_bankFound && _bank.isValid())
                    {
                        _bank.unload();
                    }
                }   
            }
            
            return _bankFound ? _bank : null;
        }
        
        /// <summary>
        /// Loads the <see cref="Bank"/> with the given name.
        /// </summary>
        /// <param name="_BankName">
        /// The name of the bank to load. <br/>
        /// <b>Must not include the <c>.bank</c> file extension.</b>
        /// </param>
        /// <param name="_Bank">Will contain the loaded <see cref="Bank"/>.</param>
        /// <returns>The loaded <see cref="Bank"/>.</returns>
        /// <exception cref="Exception">When <see cref="FMOD.Studio.System"/>.<see cref="FMOD.Studio.System.loadBankFile"/> doesn't return <see cref="RESULT.OK"/>.</exception>
        public static Bank LoadBank(string _BankName, out Bank _Bank)
        {
            var _absoluteBankPath = GetBankFilePathInUnity(_BankName);

            if (FMODSystem.loadBankFile(_absoluteBankPath, LOAD_BANK_FLAGS.NORMAL, out _Bank) is var _bankResult && _bankResult != RESULT.OK)
            {
                throw new Exception($"Load Bank: {_bankResult}{Environment.NewLine}{_absoluteBankPath}");
            }

            return _Bank;
        }

        /// <summary>
        /// Returns the absolute filepath of the <see cref="Bank"/> with the given name inside Unity.
        /// </summary>
        /// <param name="_BankName">The bank name to get the filepath for.</param>
        /// <returns>The absolute filepath of the <see cref="Bank"/> with the given name inside Unity.</returns>
        public static string GetBankFilePathInUnity(string _BankName)
        {
            var _fmodInstance = Settings.Instance;
            return Path.Combine(Application.dataPath.RemoveString("Assets"), _fmodInstance.SourceBankPath, _fmodInstance.PlayInEditorPlatform.BuildDirectory, $"{_BankName}.bank");
        }
        
        /// <summary>
        /// Checks if the given bank is laoded in <see cref="fmodSystem"/>.
        /// </summary>
        /// <param name="_BankName">The name of the bank to check.</param>
        /// <param name="_Bank">Will contain the loaded <see cref="Bank"/> or <c>default</c>.</param>
        /// <returns><c>true</c> if the bank is loaded, otherwise <c>false</c>.</returns>
        public static bool IsBankLoaded(string _BankName, out Bank _Bank)
        {
            FMODSystem.getBankList(out var _banks);
            
            foreach (var _bank in _banks)
            {
                _bank.getPath(out var _bankPath);
                
                if (string.Equals($"bank:/{_BankName}", _bankPath))
                {
                    _Bank = _bank;
                    return true;
                }
            }

            _Bank = default;
            return false;
        }
        
        /// <summary>
        /// Gets the <see cref="EventDescription"/> for the given <see cref="EventReference"/>. <br/>
        /// <i>The <see cref="Bank"/> that contains the given <see cref="EventReference"/> must have been loaded before this is called. -> <see cref="LoadBank"/>.</i>
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the <see cref="EventDescription"/> for.</param>
        /// <param name="_EventDescription">Will contain the <see cref="EventDescription"/> for the given <see cref="EventReference"/>.</param>
        /// <returns>The <see cref="EventDescription"/> for the given <see cref="EventReference"/>.</returns>
        /// <exception cref="Exception">When <see cref="FMOD.Studio.System"/>.<see cref="FMOD.Studio.System.getEventByID"/> doesn't return <see cref="RESULT.OK"/>.</exception>
        public static EventDescription GetEventDescription(EventReference _EventReference, out EventDescription _EventDescription)
        {
            if (FMODSystem.getEventByID(_EventReference.Guid, out _EventDescription) is var _eventDescriptionResult && _eventDescriptionResult != RESULT.OK)
            {
                throw new Exception($"Get Event: {_eventDescriptionResult}{Environment.NewLine}{_EventReference.Path}");
            }

            return _EventDescription;
        }

        /// <summary>
        /// Gets every <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.
        /// </summary>
        /// <param name="_EventReference">The <see cref="EventReference"/> to get the <see cref="PARAMETER_DESCRIPTION"/>s of.</param>
        /// <param name="_Exclude">
        /// Excludes every <see cref="PARAMETER_DESCRIPTION"/> that match this conditions. <br/>
        /// <i>Set to <c>() =&gt; false</c> to include every parameter.</i>
        /// </param>
        /// <param name="_FMODParameters">Will contain all <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.</param>
        /// <returns>All <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.</returns>
        public static List<FMODParameter> GetParameters(EventReference _EventReference, Func<PARAMETER_DESCRIPTION, bool> _Exclude, out List<FMODParameter> _FMODParameters)
        {
            GetEventDescription(_EventReference, out var _eventDescription);
            return GetParameters(_eventDescription, _Exclude, out _FMODParameters);
        }
        
        /// <summary>
        /// Gets every <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.
        /// </summary>
        /// <param name="_EventDescription">The <see cref="EventDescription"/> to get the <see cref="PARAMETER_DESCRIPTION"/>s of.</param>
        /// <param name="_Exclude">
        /// Excludes every <see cref="PARAMETER_DESCRIPTION"/> that match this conditions. <br/>
        /// <i>Set to <c>() =&gt; false</c> to include every parameter.</i>
        /// </param>
        /// <param name="_FMODParameters">Will contain all <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.</param>
        /// <returns>All <see cref="PARAMETER_DESCRIPTION"/> for the given <see cref="EventDescription"/>.</returns>
        /// <exception cref="Exception">When <see cref="EventDescription"/>.<see cref="EventDescription.getParameterDescriptionCount"/> doesn't return <see cref="RESULT.OK"/>.</exception>
        public static List<FMODParameter> GetParameters(EventDescription _EventDescription, Func<PARAMETER_DESCRIPTION, bool> _Exclude, out List<FMODParameter> _FMODParameters)
        {
            if (_EventDescription.getParameterDescriptionCount(out var _parameterCount) is var _parameterResult && _parameterResult != RESULT.OK)
            {
                throw new Exception($"Get Parameter Count: {_parameterResult}");
            }

            _FMODParameters = ListPool<FMODParameter>.Get();
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _parameterCount; i++)
            {
                _EventDescription.getParameterDescriptionByIndex(i, out var _parameterDescription);

                if (_Exclude(_parameterDescription))
                {
                    continue;
                }
                
                _FMODParameters.Add(new FMODParameter(_EventDescription, _parameterDescription));
            }

            return _FMODParameters;
        }
        
        /// <summary>
        /// Returns a <see cref="Dictionary{TKey,TValue}"/> with every labeled parameter for the given <see cref="FMODParameter"/>.
        /// </summary>
        /// <param name="_FMODParameter">The <see cref="FMODParameter"/> to get the labels for.</param>
        /// <param name="_ParameterName">The actual name of the <see cref="FMODParameter"/>.</param>
        /// <returns>An <see cref="Array"/> with all <see cref="ParameterLabel"/> of the given <see cref="FMODParameter"/>.</returns>
        /// <exception cref="Exception">When <see cref="FMOD.Studio.System"/>.<see cref="FMOD.Studio.System.getEventByID"/> or <see cref="EventDescription"/>.<see cref="EventDescription.getParameterDescriptionByID"/> don't return <see cref="RESULT.OK"/>.</exception>
        public static ParameterLabel[] GetParameterLabels(FMODParameter _FMODParameter, out string _ParameterName)
        {
            _ParameterName = _FMODParameter.ParameterName;

            if (FMODSystem.getEventByID(_FMODParameter.EventDescriptionId, out var _eventDescription) is var _eventDescriptionResult && _eventDescriptionResult != RESULT.OK)
            {
                throw new Exception($"Get Event: {_eventDescriptionResult}");
            }
            
            if (_eventDescription.getParameterDescriptionByID(_FMODParameter.ParameterId, out var _parameterDescription) is var _parameterDescriptionResult && _parameterDescriptionResult != RESULT.OK)
            {
                throw new Exception($"Get Parameter: {_parameterDescriptionResult}");
            }
            
            var _parameterLabels = new ParameterLabel[_parameterDescription.maximum.AsInt() + 1];
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i <= _parameterDescription.maximum; i++)
            {
                if (_eventDescription.getParameterLabelByID(_FMODParameter.ParameterId, i, out var _label) is var _labelResult && _labelResult != RESULT.OK)
                {
                    Debug.LogError($"Get Parameter Label: {_labelResult}{Environment.NewLine}Parameter: {_ParameterName.Bold()}");
                    continue;
                }

                _parameterLabels[i] = new ParameterLabel(_label, i);
            }

            return _parameterLabels;
        }

        /// <summary>
        /// Gets the event path for the given <see cref="Guid"/>.
        /// </summary>
        /// <param name="_Guid">The <see cref="Guid"/> to get the event path for.</param>
        /// <returns>The event path for the given <see cref="Guid"/>.</returns>
        public static string GetEventPathFromGuid(string _Guid)
        {
            if (Guid.TryParse(_Guid, out var _guid))
            {
                if (RuntimeManager.StudioSystem.getEventByID(new FMOD.GUID(_guid), out var _eventDescription) is var _eventResult && _eventResult != RESULT.OK)
                {
                    Debug.LogError($"Get Event: {_eventResult}");
                }
                else
                {
                    _eventDescription.getPath(out var _eventPath);
                    return _eventPath;
                }
            }
            else
            {
                Debug.LogError("Invalid GUID format.");
            }

            return string.Empty;
        }
        #endregion
    }
}
#endif
