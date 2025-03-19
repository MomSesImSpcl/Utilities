using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Utility struct to calculate the current frames per second.
    /// </summary>
    public struct FPSCounter
    {
        #region Constants
        /// <summary>
        /// The default smoothing factor.
        /// </summary>
        private const float SMOOTING_FACTOR = .1f;
        #endregion
        
        #region Fields
        /// <summary>
        /// Stores the smoothed <see cref="Time.deltaTime"/>, representing the duration of aframe.
        /// </summary>
        private float deltaTime;
        #endregion
        
        #region Operators
        /// <summary>
        /// Implicitly returns the value of <see cref="FloatFPS"/> as a <see cref="float"/>.
        /// </summary>
        /// <param name="_FPSCounter">The <see cref="FPSCounter"/> to calculate the FPS with.</param>
        /// <returns>The value of <see cref="FloatFPS"/> as a <see cref="float"/>.</returns>
        public static implicit operator float(FPSCounter _FPSCounter) => _FPSCounter.FloatFPS();
        /// <summary>
        /// Implicitly returns the value of <see cref="FloatFPS"/> as an <see cref="int"/>.
        /// </summary>
        /// <param name="_FPSCounter">The <see cref="FPSCounter"/> to calculate the FPS with.</param>
        /// <returns>The value of <see cref="FloatFPS"/> as an <see cref="int"/>.</returns>
        public static implicit operator int(FPSCounter _FPSCounter) => _FPSCounter.IntFPS();
        /// <summary>
        /// Implicitly returns the value of <see cref="FloatFPS"/> as an <see cref="uint"/>.
        /// </summary>
        /// <param name="_FPSCounter">The <see cref="FPSCounter"/> to calculate the FPS with.</param>
        /// <returns>The value of <see cref="FloatFPS"/> as an <see cref="uint"/>.</returns>
        public static implicit operator uint(FPSCounter _FPSCounter) => (uint)_FPSCounter.IntFPS();
        /// <summary>
        /// Implicitly returns the value of <see cref="FloatFPS"/> as a <see cref="string"/>.
        /// </summary>
        /// <param name="_FPSCounter">The <see cref="FPSCounter"/> to calculate the FPS with.</param>
        /// <returns>The value of <see cref="FloatFPS"/> as a <see cref="string"/>.</returns>
        public static implicit operator string(FPSCounter _FPSCounter) => _FPSCounter.ToString();
        #endregion
        
        #region Methods
        /// <summary>
        /// Calculates the current Frames Per Second.
        /// </summary>
        /// <param name="_SmoothingFactor">
        /// Higher values make the calculation more sensitive to changes, lower values make it more stable over time. <br/>
        /// <b>Should be between <c>0</c> and <c>1</c>.</b>
        /// </param>
        /// <returns>The current Frames Per Second.</returns>
        public float FloatFPS(float _SmoothingFactor = SMOOTING_FACTOR)
        {
            return 1 / (this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * _SmoothingFactor);
        }

        /// <summary>
        /// Calculates the current Frames Per Second.
        /// </summary>
        /// <param name="_SmoothingFactor">
        /// Higher values make the calculation more sensitive to changes, lower values make it more stable over time. <br/>
        /// <b>Should be between <c>0</c> and <c>1</c>.</b>
        /// </param>
        /// <returns>The current Frames Per Second.</returns>
        public int IntFPS(float _SmoothingFactor = SMOOTING_FACTOR)
        {
            return Mathf.RoundToInt(this.FloatFPS(_SmoothingFactor));
        }
        
        public override string ToString()
        {
            return this.IntFPS().ToString();
        }
        #endregion
    }
}