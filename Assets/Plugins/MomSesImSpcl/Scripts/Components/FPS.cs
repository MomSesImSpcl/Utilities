#if TMP
using MomSesImSpcl.Utilities;
using TMPro;
using UnityEngine;

namespace MomSesImSpcl.Components
{
    /// <summary>
    /// Displays the current fps in a <see cref="TextMeshProUGUI"/> <see cref="Component"/>.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class FPS : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// The <see cref="TextMeshProUGUI"/> <see cref="Component"/> that displays the FPS.
        /// </summary>
        private TextMeshProUGUI fpsText;
        /// <summary>
        /// <see cref="FPSCounter"/>.
        /// </summary>
        private FPSCounter fpsCounter;
        #endregion
        
        #region Methods
        private void Awake()
        {
            this.fpsText = base.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            this.fpsText.text = this.fpsCounter.ToString();
        }
        #endregion
    }
}
#endif