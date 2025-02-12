using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// <see cref="EventTrigger"/> with an optional <see cref="flag"/> field.
    /// </summary>
    [Serializable]
    public class CustomEventTrigger : EventTrigger
    {
        #region Inspector Fields
        [Tooltip("Optional flag to differentiate this EventTrigger from others on the same GameObject.")]
        [SerializeField] private string flag;
        #endregion
        
#if UNITY_EDITOR
        #region Constants
        /// <summary>
        /// Refactor resistant field name for <see cref="flag"/>.
        /// </summary>
        public const string FLAG = nameof(flag);
        #endregion
#endif
        #region Properties
        /// <summary>
        /// <see cref="flag"/>.
        /// </summary>
        public string Flag => this.flag;
        #endregion
    }
}