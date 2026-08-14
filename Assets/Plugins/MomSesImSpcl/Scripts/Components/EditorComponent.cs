using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Destroys this <see cref="Component"/> in build.
    /// </summary>
    public class EditorComponent : MonoBehaviour
    {
        #region Methods
        protected virtual void Awake()
        {
            if (!Application.isEditor)
            {
                Destroy(this);
            }
        }
        #endregion
    }
}