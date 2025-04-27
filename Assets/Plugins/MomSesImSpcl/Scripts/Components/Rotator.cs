#if ODIN_INSPECTOR
using MomSesImSpcl.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Helper class to rotate child <see cref="GameObject"/>s to a specific angle.
    /// </summary>
    public sealed class Rotator : MonoBehaviour
    {
#if UNITY_EDITOR
        #region Inspector Fields
        [Title("References")]
        [Tooltip("Additional Transforms to rotate.")]
        [SerializeField] private Transform[] transforms;
        
        [Tooltip("The z-rotation to set the GameObjects to.")]
        [Range(-360, 360)]
        [SerializeField] private float angle;
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
        
#if UNITY_EDITOR
        [Button, Tooltip("Sets the z-rotation of all direct children + the GameObject in the \"Transforms\" array, the the value set in \"Angle\".")]
        private void Rotate()
        {
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < base.transform.childCount; i++)
            {
                var _child = base.transform.GetChild(i);
                _child.localEulerAngles = _child.localEulerAngles.WithZ(this.angle);
            }

            foreach (var _transform in this.transforms)
            {
                _transform.localEulerAngles = _transform.localEulerAngles.WithZ(this.angle);
            }
        }
#endif
        #endregion
    }
}
#endif