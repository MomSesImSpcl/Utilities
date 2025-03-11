using System;
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
        public static bool AsBool(this float _Float)
        {
            return BitConverter.SingleToInt32Bits(_Float) != 0;
        }
        
        /// <summary>
        /// Clamps this <see cref="float"/> between the given min and max value.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to clamp.</param>
        /// <param name="_ClampMin">The minimum value this <see cref="float"/> can have.</param>
        /// <param name="_ClampMax">The maximum value this <see cref="float"/> can have.</param>
        /// <returns>This <see cref="float"/> clamped between the given min and max value.</returns>
        public static float Clamp(this float _Float, float _ClampMin, float _ClampMax)
        {
            return math.clamp(_Float, _ClampMin, _ClampMax);
        }

        /// <summary>
        /// Determines whether the given <see cref="float"/> has a sign (is negative).
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to check.</param>
        /// <returns><c>true</c> if the <see cref="float"/> is negative; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasSign(this float _Float)
        {
            return BitConverter.SingleToInt32Bits(_Float).HasSign();
        }

        /// <summary>
        /// Oscillates around this <see cref="float"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Scales the Perlin noise, affecting the randomness.</param>
        /// <param name="_SineFrequency">Determines the frequency of the sine wave.</param>
        /// <param name="_Amplitude">Scales the amplitude of the final oscillated value.</param>
        /// <returns>This <see cref="float"/> with the applied oscillation.</returns>
        public static float Oscillate(this float _Float, float _OscillationSpeed, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _SineFrequency = 1.5f, float _Amplitude = .5f)
        {
            var _scaledTime = Time.realtimeSinceStartup * _OscillationSpeed;
            var _noise = Mathf.PerlinNoise1D(_scaledTime * _NoiseMultiplier) * 2 - 1;
            var _sin = math.sin(_scaledTime * _SineFrequency);
            var _direction = (!_InvertDirection).AsSignedInt();
            
            return _Float * _direction + (_noise + _sin) * _Amplitude;
        }
        
        /// <summary>
        /// Oscillates around this <see cref="float"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_ClampBetween">Clamps the final value between the negative and positive of this value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Scales the Perlin noise, affecting the randomness.</param>
        /// <param name="_SineFrequency">Determines the frequency of the sine wave.</param>
        /// <param name="_Amplitude">Scales the amplitude of the final oscillated value.</param>
        /// <returns>This <see cref="float"/> with the applied oscillation.</returns>
        public static float OscillateClamped(this float _Float, float _OscillationSpeed, float _ClampBetween, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _SineFrequency = 1.5f, float _Amplitude = .5f)
        {
            var _maxAmplitude = math.max((_ClampBetween - math.abs(_Float)) * .5f, 0f);
            var _scaledAmplitude = math.min(_Amplitude, _maxAmplitude);

            return _Float.Oscillate(_OscillationSpeed, _InvertDirection, _NoiseMultiplier, _SineFrequency, _scaledAmplitude);
        }
        #endregion
    }
}
