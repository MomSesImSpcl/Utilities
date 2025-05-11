using MomSesImSpcl.Data;
using MomSesImSpcl.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// <see cref="EventTrigger"/> with an optional <see cref="Flag"/> field.
    /// </summary>
    public sealed class CustomEventTrigger : EventTrigger
    {
        #region Inspector Fields
#if UNITY_EDITOR
        [Tooltip("Use this to add optional information for this GameObject.")]
        [SerializeField][TextArea] private string comment;        
#endif
        [Tooltip("Optional flag to differentiate this EventTrigger from others on the same GameObject.\nMust be a class that implements the IFlag interface.")]
        [SerializeField] private InterfaceReference<IFlag> flag;
        #endregion
         
#if UNITY_EDITOR
        #region Constants
        /// <summary>
        /// Refactor resistant field name for <see cref="comment"/>.
        /// </summary>
        public const string COMMENT = nameof(comment);
        /// <summary>
        /// Refactor resistant field name for <see cref="Flag"/>.
        /// </summary>
        public const string FLAG = nameof(flag);
        #endregion
#endif
        #region Properties
        /// <summary>
        /// <see cref="flag"/>.
        /// </summary>
        public string Flag => this.flag?.Interface?.Flag;
        #endregion
    }
}