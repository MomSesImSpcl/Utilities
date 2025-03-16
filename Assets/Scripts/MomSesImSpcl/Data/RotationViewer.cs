using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Helper class to display every rotation value of a <see cref="Transform"/> in the inspector.
    /// </summary>
    [ExecuteAlways]
    public sealed class RotationViewer : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Inspector Fields
        [Header("Rotation")]
        [Tooltip("Unsigned Euler Angles.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Vector3 eulerAngles;
        [Tooltip("Signed Euler Angles.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Vector3 signedEulerAngles;
        [Tooltip("Quaternion Rotation.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Quaternion rotation;
        
        [Header("Local Rotation")]
        [Tooltip("Unsigned Local Euler Angles.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Local Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Vector3 localEulerAngles;
        [Tooltip("Signed Local Euler Angles.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Local Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Vector3 signedLocalEulerAngles;
        [Tooltip("Local Quaternion Rotation.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Local Rotation")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private Quaternion localRotation;
        #endregion
        
        #region Properties
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Vector3 EulerAngles { get => this.eulerAngles; set => this.eulerAngles = value; }
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Vector3 SignedEulerAngles { get => this.signedEulerAngles; set => this.signedEulerAngles = value; }
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Quaternion Rotation { get => this.rotation; set => this.rotation = value; }
        
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Vector3 LocalEulerAngles { get => this.localEulerAngles; set => this.localEulerAngles = value; }
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Vector3 SignedLocalEulerAngles { get => this.signedLocalEulerAngles; set => this.signedLocalEulerAngles = value; }
        /// <summary>
        /// <see cref="eulerAngles"/>.
        /// </summary>
        internal Quaternion LocalRotation { get => this.localRotation; set => this.localRotation = value; }
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