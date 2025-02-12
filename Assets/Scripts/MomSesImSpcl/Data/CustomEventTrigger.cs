using MomSesImSpcl.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// <see cref="EventTrigger"/> with an optional <see cref="Flag"/> field.
    /// </summary>
    public class CustomEventTrigger : EventTrigger
    {
         #region Inspector Fields
         [Tooltip("Optional flag to differentiate this EventTrigger from others on the same GameObject.\nMust be a class that implements the IFlag interface.")]
         [SerializeField] private InterfaceReference<IFlag> flag;
         #endregion
         
#if UNITY_EDITOR
        #region Constants
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
        public string Flag => flag?.Interface?.Flag;
        #endregion
    }
}
