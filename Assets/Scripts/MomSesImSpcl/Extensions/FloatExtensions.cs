using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float"/>.
    /// </summary>
    public static class FloatExtensions
    {
        #region Methods
        /// <summary>
        /// Compares two <see cref="float"/>s and returns <c>true</c> if they are similar.
        /// </summary>
        /// <param name="_Float1">The <see cref="float"/> to compare.</param>
        /// <param name="_Float2">The <see cref="float"/> to compare with.</param>
        /// <returns><c>true</c> if the two <see cref="float"/>s are similar, otherwise, <c>false</c>.</returns>
        public static bool Approximately(this float _Float1, float _Float2)
        {
            return Mathf.Approximately(_Float1, _Float2);
        }
        
        /// <summary>
        /// Converts the given <see cref="float"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to convert.</param>
        /// <returns><c>true</c> if the <see cref="float"/> can be converted to a <see cref="bool"/> representation, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool AsBool(this float _Float)
        {
            var _int = (int)_Float;
            return *(bool*)&_int;
        }

        /// <summary>
        /// Determines whether the given <see cref="float"/> has a sign (is negative).
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to check.</param>
        /// <returns><c>true</c> if the <see cref="float"/> is negative; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(float _Float) => !(_Float >= 0f);

        /// <summary>
        /// Oscillates around this <see cref="float"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_NoiseMultiplier">Scales the Perlin noise, affecting the randomness.</param>
        /// <param name="_SineFrequency">Determines the frequency of the sine wave.</param>
        /// <param name="_Amplitude">Scales the amplitude of the final oscillated value.</param>
        /// <returns>This <see cref="float"/> with the applied oscillation.</returns>
        public static float Oscillate(this float _Float, float _OscillationSpeed, float _NoiseMultiplier = .5f, float _SineFrequency = 1.5f, float _Amplitude = .5f)
        {
            var _scaledTime = Time.time * _OscillationSpeed;
            var _noise = Mathf.PerlinNoise1D(_scaledTime * _NoiseMultiplier) * 2 - 1;
            var _sin = math.sin(_scaledTime * _SineFrequency);
            
            return _Float + (_noise + _sin) * _Amplitude;
        }
        
        /// <summary>
        /// Oscillates around this <see cref="float"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_ClampBetween">Clamps the final value between the negative and positive of this value.</param>
        /// <param name="_NoiseMultiplier">Scales the Perlin noise, affecting the randomness.</param>
        /// <param name="_SineFrequency">Determines the frequency of the sine wave.</param>
        /// <param name="_Amplitude">Scales the amplitude of the final oscillated value.</param>
        /// <returns>This <see cref="float"/> with the applied oscillation.</returns>
        public static float OscillateClamped(this float _Float, float _OscillationSpeed, float _ClampBetween, float _NoiseMultiplier = .5f, float _SineFrequency = 1.5f, float _Amplitude = .5f)
        {
            return math.clamp(_Float.Oscillate(_OscillationSpeed, _NoiseMultiplier, _SineFrequency, _Amplitude), -_ClampBetween, _ClampBetween);
        }
        #endregion
    }
}
