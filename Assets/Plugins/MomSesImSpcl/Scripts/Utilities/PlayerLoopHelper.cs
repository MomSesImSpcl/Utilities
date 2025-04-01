using System;
using MomSesImSpcl.Extensions;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Helper class to add/remove static loop method to/from the <see cref="PlayerLoop"/>.
    /// </summary>
    public static class PlayerLoopHelper
    {
        #region Methods
        /// <summary>
        /// Adds a static method to the <see cref="PlayerLoop"/>.
        /// </summary>
        /// <param name="_PlayerLoopSystemType">
        /// The <see cref="Type"/> of the <see cref="PlayerLoopSystem"/> to add the static method to. <br/>
        /// <i>For example: <see cref="Update"/>, <see cref="FixedUpdate"/>, etc.</i>
        /// </param>
        /// <param name="_StaticLoopType">
        /// The <see cref="Type"/> the static method is declared in. <br/>
        /// <b>Should be a unique <see cref="Type"/> under the given <c>_PlayerLoopSystemType</c>.</b>
        /// </param>
        /// <param name="_UpdateDelegate">The method to call as the static loop.</param>
        public static void AddStaticLoop(Type _PlayerLoopSystemType, Type _StaticLoopType, PlayerLoopSystem.UpdateFunction _UpdateDelegate)
        {
            var _playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop(); // ReSharper disable once VariableHidesOuterVariable
            var _updateLoopSystemIndex = _playerLoopSystem.subSystemList.FindIndex(_PlayerLoopSystemType, (_PlayerLoopSystem, _PlayerLoopSystemType) => _PlayerLoopSystem.type == _PlayerLoopSystemType);

            if (_updateLoopSystemIndex == -1)
            {
                Debug.LogError($"Could not find any {nameof(PlayerLoopSystem)} of type: {_PlayerLoopSystemType.Bold()}");
                return;
            }
            
            var _updateLoopSystem = _playerLoopSystem.subSystemList[_updateLoopSystemIndex];
            var _updateLoopSubSystemList = _updateLoopSystem.subSystemList;
            
            Array.Resize(ref _updateLoopSubSystemList, _updateLoopSubSystemList.Length + 1);

            _updateLoopSubSystemList[^1] = new PlayerLoopSystem
            {
                type = _StaticLoopType,
                updateDelegate = _UpdateDelegate
            };

            _updateLoopSystem.subSystemList = _updateLoopSubSystemList;
            _playerLoopSystem.subSystemList[_updateLoopSystemIndex] = _updateLoopSystem;
            
            PlayerLoop.SetPlayerLoop(_playerLoopSystem);
        }

        /// <summary>
        /// Removes a static loop from the <see cref="PlayerLoop"/>.
        /// </summary>
        /// <param name="_PlayerLoopSystemType">
        /// The <see cref="Type"/> of the <see cref="PlayerLoopSystem"/> under which the static loop runs. <br/>
        /// <i>For example: <see cref="Update"/>, <see cref="FixedUpdate"/>, etc.</i>
        /// </param>
        /// <param name="_StaticLoopType">The <see cref="Type"/> where the static loop method is declared in.</param>
        public static void RemoveStaticLoop(Type _PlayerLoopSystemType, Type _StaticLoopType)
        {
            var _playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop(); // ReSharper disable once VariableHidesOuterVariable
            var _updateLoopSystemIndex = _playerLoopSystem.subSystemList.FindIndex(_PlayerLoopSystemType, (_PlayerLoopSystem, _PlayerLoopSystemType) => _PlayerLoopSystem.type == _PlayerLoopSystemType);

            if (_updateLoopSystemIndex == -1)
            {
                Debug.LogError($"Could not find any {nameof(PlayerLoopSystem)} of type: {_PlayerLoopSystemType.Bold()}");
                return;
            }
            
            var _updateLoopSystem = _playerLoopSystem.subSystemList[_updateLoopSystemIndex];
            var _updateLoopSubSystemList = _updateLoopSystem.subSystemList; // ReSharper disable once VariableHidesOuterVariable
            var _updateLoopSubSystemListIndex = _updateLoopSubSystemList.FindIndex(_StaticLoopType, (_PlayerLoopSystem, _StaticLoopType) => _PlayerLoopSystem.type == _StaticLoopType);

            if (_updateLoopSubSystemListIndex == -1)
            {
                Debug.LogError($"Could not find the {nameof(_StaticLoopType)}: {_StaticLoopType.Bold()}");
                return;
            }
            
            // ReSharper disable once InconsistentNaming
            for (var i = _updateLoopSubSystemListIndex + 1; i < _updateLoopSubSystemList.Length; i++)
            {
                _updateLoopSubSystemList[i - 1] = _updateLoopSubSystemList[i];
            }
            
            Array.Resize(ref _updateLoopSubSystemList, _updateLoopSubSystemList.Length - 1);
            
            _updateLoopSystem.subSystemList = _updateLoopSubSystemList;
            _playerLoopSystem.subSystemList[_updateLoopSystemIndex] = _updateLoopSystem;
            
            PlayerLoop.SetPlayerLoop(_playerLoopSystem);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Prints every <see cref="PlayerLoop"/> to the console. <br/>
        /// <b>Only works in editor.</b>
        /// </summary>
        /// <param name="_OnlyUnityLoops">Set to <c>false</c> to also print non unity player loops.</param>
        public static void PrintCurrentPlayerLoops(bool _OnlyUnityLoops = true)
        {
            var _playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            var _output = string.Empty;
            
            TraversePlayerLoops(_playerLoopSystem, ref _output, _OnlyUnityLoops);

            Debug.Log(_output);
        }

        /// <summary>
        /// Recursively traverses every <see cref="PlayerLoopSystem"/>. <br/>
        /// <b>Only works in editor.</b>
        /// </summary>
        /// <param name="_PlayerLoopSystem">The <see cref="PlayerLoopSystem"/> to get the <see cref="Type"/> of.</param>
        /// <param name="_Output">The <see cref="Type"/> of the <see cref="PlayerLoopSystem"/> will be concatenated to this <see cref="string"/>.</param>
        /// <param name="_OnlyUnityLoops">Set to <c>false</c> to also traverse every <see cref="PlayerLoopSystem.subSystemList"/> of this <see cref="PlayerLoopSystem"/>.</param>
        private static void TraversePlayerLoops(PlayerLoopSystem _PlayerLoopSystem, ref string _Output, bool _OnlyUnityLoops)
        {
            if (_PlayerLoopSystem.subSystemList is null)
            {
                return;
            }
            
            foreach (var _playerLoopSubSystem in _PlayerLoopSystem.subSystemList)
            {
                _Output += _playerLoopSubSystem.type + Environment.NewLine;

                if (!_OnlyUnityLoops)
                {
                    TraversePlayerLoops(_playerLoopSubSystem, ref _Output, false);
                }
            }
        }
#endif
        #endregion
    }
}
