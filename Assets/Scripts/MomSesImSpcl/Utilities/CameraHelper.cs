using System;
using MomSesImSpcl.Utilities.Singleton;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains helper methods and an instance of the <see cref="UnityEngine.Camera"/>. <br/>
    /// <b>Must not be on the same <see cref="GameObject"/> as the <see cref="UnityEngine.Camera"/> <see cref="Component"/>, this <see cref="Component"/> must be a child of the <see cref="Camera.main"/> <see cref="UnityEngine.Camera"/> <see cref="GameObject"/> in the scene.</b>
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Canvas))]
    public sealed class CameraHelper : SingletonMonoBehaviour<CameraHelper>
    {
        #region Fields
        /// <summary>
        /// <see cref="Screen"/>.<see cref="Screen.width"/>.
        /// </summary>
        private float width = Screen.width;
        /// <summary>
        /// <see cref="Screen"/>.<see cref="Screen.height"/>.
        /// </summary>
        private float height = Screen.height;
        /// <summary>
        /// Reference to the <see cref="UnityEngine.Camera"/> in the scene.
        /// </summary>
        private new Camera camera;
        #endregion
        
        #region Properties
        protected override InitializationMethod InitializationMethod => InitializationMethod.Awake;
        protected override bool EditorInitialization => true;
        /// <summary>
        /// <see cref="camera"/>.
        /// </summary>
        public static Camera Camera => Instance!.camera;
        #endregion

        #region Events
        /// <summary>
        /// Is fired when the aspect ratio of the <see cref="camera"/> changes.
        /// </summary>
        public static event Action OnAspectRatioChanged;
        #endregion
        
        #region Methods
        protected override void Awake()
        {
            base.Awake();
            this.camera = base.GetComponentInParent<Camera>();
            var _canvas = base.GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = this.camera;
#if UNITY_EDITOR
            if (base.GetComponent<Camera>() is not null)
            {
                Debug.LogWarning($"{nameof(CameraHelper)} must not be on the same GameObject as the Camera, it must be a child of the Camera GameObject.");
            }
#endif
        }

        private void OnRectTransformDimensionsChange()
        {
            var _width = Screen.width;
            var _height = Screen.height;

            if (!Mathf.Approximately(_width, this.width) || !Mathf.Approximately(_height, this.height))
            {
                this.width = _width;
                this.height = _height;
                
                OnAspectRatioChanged?.Invoke();
            }
        }
        #endregion
    }
}