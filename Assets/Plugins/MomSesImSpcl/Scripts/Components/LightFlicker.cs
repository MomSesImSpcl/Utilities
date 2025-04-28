#if DOTWEEN
using DG.Tweening;
using MomSesImSpcl.Attributes;
using MomSesImSpcl.Data;
using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Flicker animation for a <see cref="Light"/> component.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public sealed class LightFlicker : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Settings")]
        [Tooltip("Max % decrease from the default intensity (0.25 = 25% lower).")]
        [Range(0, float.MaxValue)]
        [SerializeField] private float minIntensityMultiplier = .25f;
        [Tooltip("Max % increase from the default intensity (0.1 = 10% higher).")]
        [Range(0, float.MaxValue)]
        [SerializeField] private float maxIntensityMultiplier = .1f;
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
#endif
        [Tooltip("The minimum and maximum duration in seconds each flicker can take.")]
        [TupleLabels("Min", "Max")]
        [SerializeField] private SerializedTuple<float> duration = new(.05f, .1f);
        [Tooltip("The easing curve to use for the flicker animation.")]
        [SerializeField] private Ease ease = Ease.InOutFlash;
        #endregion
        
        #region Fields
        /// <summary>
        /// The <see cref="Light"/> component to flicker.
        /// </summary>
        private new Light light;
        /// <summary>
        /// The default <see cref="Light.intensity"/> of the <see cref="light"/>.
        /// </summary>
        private float defaultIntensity;
        #endregion
        
        #region Methods
        private void Awake()
        {
            this.light = base.GetComponent<Light>();
            this.defaultIntensity = this.light.intensity;
        }

        private void OnEnable()
        {
            this.Flicker();
        }

        /// <summary>
        /// Randomly increases/decreases the <see cref="Light.intensity"/> of the <see cref="light"/>.
        /// </summary>
        private void Flicker()
        {
            var _minIntensity = this.defaultIntensity - this.defaultIntensity * this.minIntensityMultiplier;
            var _maxIntensity = this.defaultIntensity + this.defaultIntensity * this.maxIntensityMultiplier;

            var _intensity = Random.Range(_minIntensity, _maxIntensity);
            var _duration = Random.Range(this.duration.Item1, this.duration.Item2);

            this.light.DOIntensity(_intensity, _duration).SetEase(this.ease).OnComplete(this.Flicker);
        }
        #endregion
    }
}
#endif