using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Used to add a description to a <see cref="GameObject"/> in the inspector.
    /// </summary>
    public sealed class InspectorComment : EditorComponent
    {
#if UNITY_EDITOR
        #region Inspector Fields
        [Tooltip("Use this to add a description to a GameObject in the inspector.")]
        [SerializeField][TextArea] private string comment;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="comment"/>.
        /// </summary>
        public string Comment { get => comment; set => comment = value; }
        #endregion
#endif
    }
}