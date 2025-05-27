using MomSesImSpcl.Data;
using UnityEngine;

namespace MomSesImSpcl.Components.Development
{
    /// <summary>
    /// Helper class to instantiate prefabs in development builds.
    /// </summary>
    public sealed class DevelopmentInstantiator : DevelopmentMonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
        [Tooltip("GameObjects to instantiate during development builds.\nValue: true = Also instantiate in editor.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DictionaryDrawerSettings(KeyLabel = "Prefab", ValueLabel = "Instantiate in Editor")]
#endif
        [SerializeField] private SerializedDictionary<GameObject, bool> prefabs;
        #endregion
        
        #region Methods
        protected override void Awake()
        {
            base.Awake();
            
            foreach (var (_prefab, _instantiateInEditor) in this.prefabs)
            {
                if (Application.isEditor && !_instantiateInEditor)
                {
                    continue;
                }
                
                InstantiateAsync(_prefab, base.transform);
            }
        }
        #endregion
    }
}