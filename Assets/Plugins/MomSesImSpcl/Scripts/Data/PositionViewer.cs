using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Data
{
    /// <summary>
    /// Helper class to display every position value of a <see cref="Transform"/>/<see cref="RectTransform"/> in the inspector.
    /// </summary>
    public sealed class PositionViewer : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Inspector Fields
#if !ODIN_INSPECTOR
        [Header("World")]
#endif
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("World")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Position in world space.")]
        [SerializeField] private Vector3 position;
        
        [Header("Rect Transform")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(this.IsRectTransform))]
        [Sirenix.OdinInspector.FoldoutGroup("World")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Minimum corner position in world space.")]
        [SerializeField] private Vector2 min;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(this.IsRectTransform))]
        [Sirenix.OdinInspector.FoldoutGroup("World")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Maximum corner position in world space.")]
        [SerializeField] private Vector2 max;
        
#if !ODIN_INSPECTOR
        [Header("Local")]
#endif
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.FoldoutGroup("Local")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Position in local space.")]
        [SerializeField] private Vector3 localPosition;
        
        [Header("Rect Transform")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(this.IsRectTransform))]
        [Sirenix.OdinInspector.FoldoutGroup("Local")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Minimum corner position in local space.")]
        [SerializeField] private Vector2 localMin;
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ShowIf(nameof(this.IsRectTransform))]
        [Sirenix.OdinInspector.FoldoutGroup("Local")]
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Maximum corner position in local space.")]
        [SerializeField] private Vector2 localMax;
        #endregion
        #region Properties
        /// <summary>
        /// Will be <c>true</c> if this <see cref="Transform"/> is a <see cref="RectTransform"/>.
        /// </summary>
        internal bool IsRectTransform { get; set; }
        
        /// <summary>
        /// <see cref="position"/>.
        /// </summary>
        internal Vector3 Position { get => this.position; set => this.position = value; }
        /// <summary>
        /// <see cref="min"/>.
        /// </summary>
        internal Vector2 Min { get => this.min; set => this.min = value; }
        /// <summary>
        /// <see cref="max"/>.
        /// </summary>
        internal Vector2 Max { get => this.max; set => this.max = value; }
        
        /// <summary>
        /// <see cref="localPosition"/>.
        /// </summary>
        internal Vector3 LocalPosition { get => this.localPosition; set => this.localPosition = value; }
        /// <summary>
        /// <see cref="localMin"/>.
        /// </summary>
        internal Vector2 LocalMin { get => this.localMin; set => this.localMin = value; }
        /// <summary>
        /// <see cref="localMax"/>.
        /// </summary>
        internal Vector2 LocalMax { get => this.localMax; set => this.localMax = value; }
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

#if UNITY_EDITOR && ODIN_INSPECTOR
        /// <param name="_Transform">The <see cref="Transform"/> to center.</param>
        /// <param name="_X">Set this to <c>false</c> to not center the <see cref="Vector3.x"/> <see cref="Transform.position"/>.</param>
        /// <param name="_Y">Set this to <c>false</c> to not center the <see cref="Vector3.y"/> <see cref="Transform.position"/>.</param>
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonStyle.FoldoutButton), Tooltip("Centers the given Transform between the Min and Max positions of this RecTransform.")]
        private void Center(Transform _Transform, bool _X = true, bool _Y = true)
        {
            if (!this.IsRectTransform)
            {
                Debug.LogError($"The transform of this {nameof(PositionViewer)} is not a RectTransform.");
                return;
            }
                
            var _center = (this.min + this.max) / 2;

            if (_X)
            {
                 _Transform.position = _Transform.position.WithX(_center.x);
            }

            if (_Y)
            {
                _Transform.position = _Transform.position.WithY(_center.y);
            }
        }
#endif
        #endregion
    }
}