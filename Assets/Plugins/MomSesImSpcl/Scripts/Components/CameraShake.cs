#if DOTWEEN
using System.Globalization;
using DG.Tweening;
using JetBrains.Annotations;
using MomSesImSpcl.Components.Singleton;
using MomSesImSpcl.Extensions;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Helper class for <see cref="Camera"/> shake.
    /// </summary>
    public sealed class CameraShake : SingletonMonoBehaviour<CameraShake>
    {
        #region Inspector Fields
        [Header("Debug")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("Increases the strength by intensityScale.")]
        [SerializeField] private int intensity = -1;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The duration + the added intensity.")]
        [SerializeField] private float duration;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ReadOnly]
#endif
        [Tooltip("The strength + the added intensity.")]
        [SerializeField] private float3 strength;
#if UNITY_EDITOR
        [Tooltip("Prints the calculated shake values that are being used for the current shake.")]
        [SerializeField] private bool printShakeValues;
#endif
        #endregion
        
        #region Fields
        /// <summary>
        /// The <see cref="Transform.position"/> of the <see cref="CameraHelper.Camera"/> before the Shake was started.
        /// </summary>
        private static Vector3 originPosition;
        /// <summary>
        /// The <see cref="Tween"/> that shakes the <see cref="CameraHelper.Camera"/>.
        /// </summary>
        [CanBeNull] private static Tween shakeTween;
        #endregion
        
        #region Properties
        protected override InitializationMethod InitializationMethod => InitializationMethod.Awake;
        #endregion
        
        #region Methods
#if UNITY_EDITOR && ODIN_INSPECTOR
        [Tooltip("Shakes the camera.")]
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonStyle.FoldoutButton)]
        private static void Shake(float _MinDuration = .175f, float _MaxDuration = .5f, float _MinStrength = .1f, float _MaxStrength = .5f, int _Vibrato = 50, float _StartingIntensity = 0f, ushort _FreezeFrame = 1, uint _MaxIntensitySteps = 10)
        {
            var _durationIntensityStep = (_MaxDuration - _MinDuration) / _MaxIntensitySteps;
            var _strengthIntensityStep = (_MaxStrength - _MinStrength) / _MaxIntensitySteps;
            
#pragma warning disable CS4014
            Shake((_MinDuration, _MaxDuration), (_MinStrength, _MaxStrength), _Vibrato, _StartingIntensity, (_durationIntensityStep, _strengthIntensityStep), _FreezeFrame);
#pragma warning restore CS4014
        }
#endif
        
        /// <summary>
        /// Shakes the <see cref="CameraHelper.Camera"/>.
        /// </summary>
        /// <param name="_Duration">
        /// The duration of the shake. <br/>
        /// <b>Min:</b> The minimum duration in seconds. <br/>
        /// <b>Max:</b> The maximum duration in seconds.
        /// </param>
        /// <param name="_Strength">
        /// How far the <see cref="CameraHelper.Camera"/> <see cref="Transform.position"/> will move from the origin <see cref="Transform.position"/>. <br/>
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
            Cysharp.Threading.Tasks.UniTask
#else
            System.Threading.Tasks.Task 
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
                await Cysharp.Threading.Tasks.UniTask.DelayFrame(_FreezeTime.Value);
#else
                await System.Threading.Tasks.Task.Delay(_FreezeTime.Value);
#endif
                Time.timeScale = 1f;
            }
            
            KillTween();
            
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

            Shake(_duration, _strength, _Vibrato);
        }

        /// <summary>
        /// Starts the <see cref="Camera"/> shake with the given values.
        /// </summary>
        /// <param name="_Duration">The duration in seconds.</param>
        /// <param name="_Strength">How far the <see cref="Transform.position"/> can move.</param>
        /// <param name="_MaxStrength">The maximum value <c>_Strength</c> can have.</param>
        /// <param name="_Vibrato">How much the <see cref="Camera"/> will vibrate.</param>
        /// <param name="_Intensity">
        /// Multiplier for the <c>_Strength</c>. <br/>
        /// <i>The lowest value should start with <c>1</c>.</i>
        /// </param>
        public static void Shake(float _Duration, Vector3 _Strength, Vector3 _MaxStrength, int _Vibrato, int _Intensity)
        {
            KillTween();
            
            Instance!.intensity = _Intensity;

            var _strength = (_Strength * _Intensity).Clamp(Vector3.zero, _MaxStrength);
            
            Shake(_Duration, _strength, _Vibrato);
        }

        /// <summary>
        /// Kills the <see cref="shakeTween"/> and resets the <see cref="CameraHelper.Camera"/> <see cref="Transform.position"/> to the <see cref="originPosition"/>.
        /// </summary>
        private static void KillTween()
        {
            if (shakeTween is not null)
            {
                shakeTween.Kill();
                CameraHelper.Camera.transform.position = originPosition;
            }
            else
            {
                originPosition = CameraHelper.Camera.transform.position;
            }
        }

        /// <summary>
        /// Starts the <see cref="Camera"/> shake with the given values.
        /// </summary>
        /// <param name="_Duration">The duration in seconds.</param>
        /// <param name="_Strength">How far the <see cref="Transform.position"/> can move.</param>
        /// <param name="_Vibrato">How much the <see cref="Camera"/> will vibrate.</param>
        private static void Shake(float _Duration, Vector3 _Strength, int _Vibrato)
        {
#if UNITY_EDITOR
            if (Instance!.printShakeValues)
            {
                Debug.Log($"Duration: {_Duration.ToString(CultureInfo.InvariantCulture)} | Strength: {_Strength.Average().ToString(CultureInfo.InvariantCulture)} | Intensity: {Instance.intensity.ToString()}");
            }
#endif
            Instance.duration = _Duration;
            Instance.strength = _Strength;
            
            shakeTween = CameraHelper.Camera.transform.DOShakePosition(_Duration, _Strength, _Vibrato).OnComplete(() =>
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
    }
}
#endif
