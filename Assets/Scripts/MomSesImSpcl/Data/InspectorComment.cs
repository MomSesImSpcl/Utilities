using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Used to add a description to a <see cref="GameObject"/> in the inspector.
    /// </summary>
    public class InspectorComment : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Inspector Fields
        [Tooltip("Use this to add a description to a GameObject in the inspector.")]
        [SerializeField][TextArea] private string comment;
        #endregion
#endif
        #region Methods
        private void Awake()
        {
            if (!Application.isEditor)
            {
                Destroy(this);
            }
        }
        #endregion
    }
}