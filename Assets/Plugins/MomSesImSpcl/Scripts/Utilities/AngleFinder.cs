using MomSesImSpcl.Extensions;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Helper class to find the angle of the mouse to the position of this <see cref="GameObject"/>.
    /// </summary>
    [ExecuteAlways]
    public sealed class AngleFinder : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("The unsigned angle of the mouse position to the position of thi GameObject.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif  // ReSharper disable once NotAccessedField.Local
        [SerializeField] private float unsignedAngle;
        [Tooltip("The signed angle of the mouse position to the position of thi GameObject.")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [SerializeField] private float signedAngle;
        #endregion
        
        #region Fields
        /// <summary>
        /// <see cref="Camera"/>.<see cref="Camera.main"/>.
        /// </summary>
        private new Camera camera;
        /// <summary>
        /// Will be <c>true</c> when any mouse button is pressed.
        /// </summary>
        private bool mouseDown;
        #endregion
        
        #region Methods
        private void OnEnable()
        {
            this.camera = Camera.main;
        }

        private void OnGUI()
        {
            var _event = Event.current;
            this.mouseDown = _event.type switch
            {
                EventType.MouseDown => true,
                EventType.MouseUp => false,
                _ => this.mouseDown
            };

            if (this.mouseDown)
            {
                var _transform = base.transform;
                var _worldOrigin = _transform.position;
                var _screenOrigin = this.camera.WorldToScreenPoint(_worldOrigin);
                var _mousePosition = _event.mousePosition.ToVector3().WithZ(_screenOrigin.z);
                var _target = this.camera.ScreenToWorldPoint(_mousePosition);
                
                var _direction = _transform.forward;
                var _radius = _worldOrigin.Distance(_target);
                this.signedAngle = _worldOrigin.Angle(_target);
                this.unsignedAngle = Math.SignedToUnsignedAngle(this.signedAngle);
                
                Draw.Angle(_worldOrigin, _direction, _radius, this.signedAngle, .05f);
            }
        }
        #endregion
    }
}