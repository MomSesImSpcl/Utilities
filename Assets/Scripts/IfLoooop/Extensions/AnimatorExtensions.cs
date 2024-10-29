using System;
using System.Linq;
using UnityEngine;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Animator"/>.
    /// </summary>
    public static class AnimatorExtensions
    {
        #region Methods
        /// <summary>
        /// Converts a parameter name to a hash/id for the specified <see cref="Animator"/>.
        /// <i>Calls <see cref="ValidateParameter"/> when called from the editor.</i>
        /// <b>Use only for <see cref="Animator"/> parameters!</b>
        /// </summary>
        /// <param name="_Animator">The <see cref="Animator"/> instance to use.</param>
        /// <param name="_ParameterName">The name of the parameter to convert to a hash/id.</param>
        /// <returns>The hash/id for the specified parameter name.</returns>
        public static int ParameterNameToHash(this Animator _Animator, string _ParameterName)
        {
#if UNITY_EDITOR
            ValidateParameter(_Animator, _ParameterName);
#endif
            return Animator.StringToHash(_ParameterName);
        }

        /// <summary>
        /// Converts a state name to a hash/id for the specified <see cref="Animator"/>.
        /// Validates the layer and state when called from the editor.
        /// </summary>
        /// <param name="_Animator">The <see cref="Animator"/> instance to use.</param>
        /// <param name="_LayerName">The name of the layer containing the state.</param>
        /// <param name="_StateName">The name of the state to convert to a hash/id.</param>
        /// <param name="_LayerIndex">Outputs the index of the specified layer.</param>
        /// <returns>The hash/id for the specified state name.</returns>
        public static int StateNameToHash(this Animator _Animator, string _LayerName, string _StateName, out int _LayerIndex)
        {
            var _stateID = Animator.StringToHash(_StateName);
            _LayerIndex = _Animator.GetLayerIndex(_LayerName);

#if UNITY_EDITOR
            ValidateState(_Animator, _LayerName, _stateID, _StateName);
#endif
            return _stateID;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Validates if the specified parameter name exists in the provided <see cref="Animator"/> instance.
        /// Throws an <see cref="ArgumentException"/> if the parameter is not found.
        /// </summary>
        /// <param name="_Animator">The <see cref="Animator"/> instance to validate against.</param>
        /// <param name="_ParameterName">The name of the parameter to validate.</param>
        private static void ValidateParameter(Animator _Animator, string _ParameterName)
        {
            if (_Animator.parameters.All(_Parameter => _Parameter.name != _ParameterName))
            {
                throw new ArgumentException($"The Animator: [{_Animator.name}], does not contain any parameter with the name: [{_ParameterName}].");
            }
        }

        /// <summary>
        /// Validates the specified state in the given <see cref="Animator"/>.
        /// Checks if the layer and state exist within the animator.
        /// </summary>
        /// <param name="_Animator">The <see cref="Animator"/> instance to validate against.</param>
        /// <param name="_LayerName">The name of the layer containing the state.</param>
        /// <param name="_StateID">The hash/id of the state to validate.</param>
        /// <param name="_StateName">The name of the state to validate.</param>
        /// <exception cref="ArgumentException">Thrown if the layer or state does not exist in the animator.</exception>
        private static void ValidateState(Animator _Animator, string _LayerName, int _StateID, string _StateName)
        {
            var _layerIndex = _Animator.GetLayerIndex(_LayerName);
            var _haseState = _Animator.HasState(_layerIndex, _StateID);
            
            if (_layerIndex == -1)
            {
                throw new ArgumentException($"The LayerName: [{_LayerName}], does not exist in the Animator: [{_Animator.name}].");
            }
            
            if (!_haseState)
            {
                throw new ArgumentException($"The Animator: [{_Animator.name}], does not contain the state: [{_StateName}]");
            }
        }
#endif
        #endregion
    }
}