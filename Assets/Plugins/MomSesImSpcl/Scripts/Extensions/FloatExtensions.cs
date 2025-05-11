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
            return _Float != 0f;
        }

        /// <summary>
        /// Casts this <see cref="float"/> into an <see cref="int"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to cast into an <see cref="int"/>.</param>
        /// <returns>This <see cref="float"/> as an <see cref="int"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsInt(this float _Float)
        {
            return (int)_Float;
        }
        
        /// <summary>
        /// Casts this <see cref="float"/> into an <see cref="uint"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to cast into an <see cref="uint"/>.</param>
        /// <returns>This <see cref="float"/> as an <see cref="uint"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint AsUInt(this float _Float)
        {
            return (uint)_Float;
        }
        
        /// <summary>
        /// Clamps this <see cref="float"/> between the given min and max value.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to clamp.</param>
        /// <param name="_ClampMin">The minimum value this <see cref="float"/> can have.</param>
        /// <param name="_ClampMax">The maximum value this <see cref="float"/> can have.</param>
        /// <returns>This <see cref="float"/> clamped between the given min and max value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            return _Float < 0f;
        }
        
        /// <summary>
        /// Oscillates around this <see cref="float"/>.
        /// </summary>
        /// <param name="_Float">The <see cref="float"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_Amplitude">Controls how far the value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_SineFrequency">Controls the oscillation speed.</param>
        /// <returns>This <see cref="float"/> with the applied oscillation.</returns>
        public static float Oscillate(this float _Float, float _OscillationSpeed, float _Amplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _SineFrequency = 1.5f)
        {
            var _amplitude = Mathf.Max((_Amplitude - Mathf.Abs(_Float)) * .5f, 0f);
            var _scaledTime = Time.realtimeSinceStartup * _OscillationSpeed;
            var _noise = Mathf.PerlinNoise1D(_scaledTime * _NoiseMultiplier) * 2 - 1;
            var _sin = Mathf.Sin(_scaledTime * _SineFrequency);
            var _direction = _InvertDirection.Reverse().AsSignedInt();
            
            return _Float * _direction + (_noise + _sin) * _amplitude;
        }
        #endregion
    }
}
