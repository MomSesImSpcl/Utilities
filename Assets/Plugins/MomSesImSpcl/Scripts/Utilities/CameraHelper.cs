using System;
using System.Globalization;
using JetBrains.Annotations;
using MomSesImSpcl.Extensions;
using MomSesImSpcl.Utilities.Singleton;
using Unity.Mathematics;
using UnityEngine;

#if DOTWEEN
using DG.Tweening;
#endif
#if UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Contains helper methods and an instance of the <see cref="UnityEngine.Camera"/>. <br/>
    /// <b>Must not be on the same <see cref="GameObject"/> as the <see cref="UnityEngine.Camera"/> <see cref="Component"/>, this <see cref="Component"/> must be a child of the <see cref="Camera.main"/> <see cref="UnityEngine.Camera"/> <see cref="GameObject"/> in the scene.</b>
    /// </summary>
    [ExecuteInEditMode]
    public sealed class CameraHelper : SingletonMonoBehaviour<CameraHelper>
    {
        #region Fields
        /// <summary>
        /// <see cref="Screen"/>.<see cref="Screen.width"/>.
        /// </summary>
        private int width = Screen.width;
        /// <summary>
        /// <see cref="Screen"/>.<see cref="Screen.height"/>.
        /// </summary>
        private int height = Screen.height;
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
            
            if (base.GetComponent<Camera>() == null) // Must not be "is", otherwise this will return "false" for some reason.
            {
                var _canvas = base.GetComponent<Canvas>();
                
                if (_canvas == null) // Must not be "is", must be "==".
                {
                    _canvas = base.gameObject.AddComponent<Canvas>();
                }
                
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
                _canvas.worldCamera = this.camera;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"{nameof(CameraHelper)} must not be on the same GameObject as the Camera, it must be a child of the Camera GameObject.");
                base.Invoke(nameof(DestroyThis), float.Epsilon);
            }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Destroy this component.
        /// </summary>
        private void DestroyThis() => DestroyImmediate(this);
#endif
        private void OnRectTransformDimensionsChange()
        {
            var _width = Screen.width;
            var _height = Screen.height;

            if (_width != this.width || _height != this.height)
            {
                this.width = _width;
                this.height = _height;
                
                OnAspectRatioChanged?.Invoke();
            }
        }
        #endregion
        
#if DOTWEEN && ODIN_INSPECTOR
        #region Inspector Fields
        [Sirenix.OdinInspector.Title("Debug")]
        [Tooltip("Increases the strength by intensityScale.")]
        [SerializeField][Sirenix.OdinInspector.ReadOnly] private int intensity = -1;
        [Tooltip("The duration + the added intensity.")]
        [SerializeField][Sirenix.OdinInspector.ReadOnly] private float duration;
        [Tooltip("The strength + the added intensity.")]
        [SerializeField][Sirenix.OdinInspector.ReadOnly] private float3 strength;
#if UNITY_EDITOR
        [Tooltip("Prints the calculated shake values that are being used for the current shake.")]
        [SerializeField] private bool printShakeValues;
#endif
        #endregion

        #region Fields
        /// <summary>
        /// The <see cref="Transform.position"/> of the <see cref="camera"/> before the <see cref="Shake"/> was started.
        /// </summary>
        private static Vector3 originPosition;
        /// <summary>
        /// The <see cref="Tween"/> that shakes the <see cref="camera"/>.
        /// </summary>
        [CanBeNull] private static Tween shakeTween;
        #endregion
        
        #region Methods
#if UNITY_EDITOR
        [Tooltip("Shakes the camera.")]
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonStyle.FoldoutButton)]
        private static void Shake(float _MinDuration = .175f, float _MaxDuration = .625f, float _MinStrength = .25f, float _MaxStrength = .5f, int _Vibrato = 50, float _StartingIntensity = 0f, ushort _FreezeFrame = 1, uint _MaxIntensitySteps = 10)
        {
            var _durationIntensityStep = (_MaxDuration - _MinDuration) / _MaxIntensitySteps;
            var _strengthIntensityStep = (_MaxStrength - _MinStrength) / _MaxIntensitySteps;
            
#pragma warning disable CS4014
            Shake((_MinDuration, _MaxDuration), (_MinStrength, _MaxStrength), _Vibrato, _StartingIntensity, (_durationIntensityStep, _strengthIntensityStep), _FreezeFrame);
#pragma warning restore CS4014
        }
#endif
        /// <summary>
        /// Shakes the <see cref="camera"/>.
        /// </summary>
        /// <param name="_Duration">
        /// The duration of the shake. <br/>
        /// <b>Min:</b> The minimum duration in seconds. <br/>
        /// <b>Max:</b> The maximum duration in seconds.
        /// </param>
        /// <param name="_Strength">
        /// How far the <see cref="camera"/> <see cref="Transform.position"/> will move from the origin <see cref="Transform.position"/>. <br/>
        /// <b>Min:</b> The minimum strength value. <br/>
        /// <b>Max:</b> The maximum strength value.
        /// </param>
        /// <param name="_Vibrato">Higher values increase the shake speed.</param>
        /// <param name="_StartIntensity">
        /// A percentage value from <c>0</c>-<c>1</c> to set the initial intensity to. <br/>
        /// <i>Relative to the maximum possible duration/strength values.</i>
        /// </param>
        /// <param name="_Add">
        /// The value by which the duration/strength will be increased, each time this method is called while it is still running. <br/>
        /// <i>Will be reset once the <see cref="Tween"/> completes.</i>
        /// </param>
        /// <param name="_FreezeTime">
        /// Will freeze the <see cref="Time.timeScale"/> for the specified amount of frames. <br/>
        /// <i>Set to <c>null</c> to disable.</i> <br/>
        /// <b>This will be frames only when UniTask is in the project, otherwise this will be milliseconds.</b>
        /// </param>
        public static async
#if UNITASK
            UniTask
#else
            Task 
#endif
            Shake(
            (float Min, float Max) _Duration,
            (float Min, float Max) _Strength,
            int _Vibrato,
            float _StartIntensity = 0f,
            (float Duration, float Strength) _Add = default,
            int? _FreezeTime = 1)
        {
            if (_FreezeTime.HasValue)
            {
                Time.timeScale = 0f;
#if UNITASK
                await UniTask.DelayFrame(_FreezeTime.Value);
#else
                await Task.Delay(_FreezeTime.Value);
#endif
                Time.timeScale = 1f;
            }

            if (shakeTween is not null)
            {
                Camera.transform.DOKill();
                Camera.transform.position = originPosition;
            }
            else
            {
                originPosition = Camera.transform.position;
            }
            
            Instance!.intensity++;
            
            var _clampedIntensity = _StartIntensity.Clamp(0, 1);
            
            var _baseDuration = Mathf.Lerp(_Duration.Min, _Duration.Max, _clampedIntensity);
            var _additiveDuration = _Add.Duration * Instance.intensity;
            var _duration = Mathf.Clamp(_baseDuration + _additiveDuration, _Duration.Min, _Duration.Max);
            
            var _baseStrength = math.lerp(float3.zero.WithXYZ(_Strength.Min), float3.zero.WithXYZ(_Strength.Max), _clampedIntensity);
            var _additiveStrength = float3.zero.WithXYZ(_Add.Strength * Instance.intensity);
            var _strength = math.min(_baseStrength + _additiveStrength, float3.zero.WithXYZ(_Strength.Max));

            if (_duration < Instance.duration)
            {
                _duration = Instance.duration;
            }

            if (_strength.Average() < Instance.strength.Average())
            {
                _strength = Instance.strength;
            }
            
#if UNITY_EDITOR
            if (Instance.printShakeValues)
            {
                Debug.Log($"Duration: {_duration.ToString(CultureInfo.InvariantCulture)} | Strength: {_strength.Average().ToString(CultureInfo.InvariantCulture)} | Intensity: {Instance.intensity.ToString()}");
            }
#endif
            Instance.duration = _duration;
            Instance.strength = _strength;
            
            shakeTween = Camera.transform.DOShakePosition(_duration, _strength, _Vibrato).OnComplete(() =>
            {
                Instance.intensity = -1;
                Instance.duration = 0;
                Instance.strength = default;
                
            }).OnFinish(() =>
            {
                shakeTween = null;
            });
        }
        #endregion
#endif
    }
}
