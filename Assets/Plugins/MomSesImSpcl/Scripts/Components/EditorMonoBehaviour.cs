using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Destroys this <see cref="GameObject"/> in build.
    /// </summary>
    public class EditorMonoBehaviour : MonoBehaviour
    {
        #region Methods
        protected virtual void Awake()
        {
            if (!Application.isEditor)
            {
               Destroy(base.gameObject); 
            }
        }
        #endregion
    }
}
