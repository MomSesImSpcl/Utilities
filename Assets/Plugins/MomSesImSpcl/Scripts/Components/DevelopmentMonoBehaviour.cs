using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Destroys this <see cref="GameObject"/> in non development builds.
    /// </summary>
    public class DevelopmentMonoBehaviour : MonoBehaviour
    {
        #region Methods
        protected virtual void Awake()
        {
            if (!Application.isEditor && !Debug.isDebugBuild)
            {
                Destroy(base.gameObject);
            }
        }
        #endregion
    }
}
