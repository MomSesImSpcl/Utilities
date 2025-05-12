using MomSesImSpcl.Attributes;
using MomSesImSpcl.Data;
using MomSesImSpcl.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Enables letterbox bars if the <see cref="Screen"/> aspect ratio exceeds the maximum allowed one.
    /// </summary>
    [ExecuteInEditMode, RequireComponent(typeof(Canvas))]
    public sealed class Letterbox : MonoBehaviour
    {
        #region Inspector Fields
        [Header("References")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ChildGameObjectsOnly]
#endif
        [Tooltip("The images that will be used for the letter box bars.")]
        [ElementNames("Left", "Top", "Right", "Bottom")]
        [SerializeField] private Image[] borders;
        
        [Header("Settings")]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
#endif
        [Tooltip("The maximum aspect ratio after which letterboxing will be applied.")]
        [TupleLabels("Width", "Height")]
        [SerializeField] private SerializedTuple<byte> maxAspectRatio = new(21, 9);
        [Tooltip("The color of the letterbox bars.")]
        [SerializeField] private Color color = Color.black;
        #endregion
        
        #region Properties
        /// <summary>
        /// Anything above this aspect ratio will be letterboxed.
        /// </summary>
        private float MaxAspect => this.maxAspectRatio.Item1.AsFloat() / this.maxAspectRatio.Item2.AsFloat();
        #endregion
        
        #region Methods
        private void Start()
        {
            this.AdjustLetterBoxBars();
        }
        
        private void OnEnable()
        {
            CameraHelper.OnAspectRatioChanged += this.AdjustLetterBoxBars;
        }

        private void OnDisable()
        {
            CameraHelper.OnAspectRatioChanged -= this.AdjustLetterBoxBars;
        }
        
        /// <summary>
        /// Adjusts the size of the <see cref="borders"/>.
        /// </summary>
        private void AdjustLetterBoxBars()
        {
            var _currentWidth = Screen.width;
            var _currentHeight = Screen.height;
            var _currentAspect = _currentWidth.AsFloat() / _currentHeight.AsFloat();
            
            var _left = this.borders[0].rectTransform;
            var _top = this.borders[1].rectTransform;
            var _right = this.borders[2].rectTransform;
            var _bottom = this.borders[3].rectTransform;

            if (_currentAspect > this.MaxAspect)
            {
                // Letterbox left/right.
                var _contentWidth = _currentHeight * this.MaxAspect;
                var _borderWidth = (_currentWidth - _contentWidth) / 2f;

                _left.sizeDelta = new Vector2(_borderWidth, 0);
                _right.sizeDelta = new Vector2(_borderWidth, 0);
                _top.sizeDelta = new Vector2(0, 0);
                _bottom.sizeDelta = new Vector2(0, 0);
                
                _left.gameObject.SetActive(true);
                _right.gameObject.SetActive(true);
            }
            else
            {
                // No bars needed for narrower/taller screens.
                _left.sizeDelta = Vector2.zero;
                _right.sizeDelta = Vector2.zero;
                _top.sizeDelta = Vector2.zero;
                _bottom.sizeDelta = Vector2.zero;
                
                _left.gameObject.SetActive(false);
                _right.gameObject.SetActive(false);
            }

            _top.gameObject.SetActive(false);
            _bottom.gameObject.SetActive(false);
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Cached <see cref="color"/> value to detect changes.
        /// </summary>
        [SerializeField, HideInInspector] private Color cachedColor;
        /// <summary>
        /// Cached <see cref="maxAspectRatio"/> value to detect changes.
        /// </summary>
        [SerializeField, HideInInspector] private SerializedTuple<byte> cachedAspectRatio;

        private void OnValidate()
        {
            if (this.color != this.cachedColor)
            {
                this.cachedColor = this.color;

                foreach (var _image in this.borders)
                {
                    _image.color = this.color;
                }
            }

            if (this.maxAspectRatio != this.cachedAspectRatio)
            {
                this.cachedAspectRatio = this.maxAspectRatio;
                
                UnityEditor.EditorApplication.delayCall += this.AdjustLetterBoxBars;
            }
        }
#endif
        #endregion
    }
}
