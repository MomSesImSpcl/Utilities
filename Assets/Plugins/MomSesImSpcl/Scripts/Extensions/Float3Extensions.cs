using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="float3"/>.
    /// </summary>
    public static class Float3Extensions
    {
        #region Methods
        /// <summary>
        /// Returns this <see cref="float3"/> as a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to convert.</param>
        /// <returns>This <see cref="float3"/> as a <see cref="Vector3"/>.</returns>
        [BurstCompile]
        public static Vector3 AsVector3(this float3 _Float3)
        {
            return new Vector3(_Float3.x, _Float3.y, _Float3.z);
        }
        
        /// <summary>
        /// Normalizes this <see cref="float3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to normalize.</param>
        /// <returns>This <see cref="float3"/> with normalized values.</returns>
        public static float3 Normalize(this float3 _Float3)
        {
            return math.normalize(_Float3);
        }
        
        /// <summary>
        /// Oscillates around this <see cref="float3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to oscillate around.</param>
        /// <param name="_RealtimeSinceStartup"><see cref="Time"/>.<see cref="Time.realtimeSinceStartup"/>.</param>
        /// <param name="_StartTime">
        /// When the oscillation is stopped and started frequently, the start can look janky if <see cref="Time.realtimeSinceStartup"/> has a high value. <br/>
        /// In this case cache the timestamp when this method is first started again and pass it in.
        /// </param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="float3.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="float3.y"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="float3.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="float3.y"/>-axis.</param>
        /// <returns>This <see cref="Vector2"/> with the applied oscillation.</returns>
        [BurstCompile]
        public static float3 Oscillate(this float3 _Float3, float _RealtimeSinceStartup, float _StartTime, float _OscillationSpeed, float _XAmplitude, float _YAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.3f, float _YFrequency = 1.7f)
        {
            var _time = (_RealtimeSinceStartup - _StartTime) * _OscillationSpeed;
            var _sinX = math.sin(_time * _XFrequency);
            var _sinY = math.sin(_time * _YFrequency);
            var _noise = _time * _NoiseMultiplier;
            var _noiseX = noise.snoise(new float2(_noise, 0f)) * 2 - 1;
            var _noiseY = noise.snoise(new float2(0f, _noise)) * 2 - 1;
            var _direction = _InvertDirection.Reverse().AsSignedInt();
            var _x = _Float3.x + (_sinX + _noiseX) * (_XAmplitude * .5f) * _direction;
            var _y = _Float3.y + (_sinY + _noiseY) * (_YAmplitude * .5f) * _direction;

            return new float3(_x, _y, _Float3.z);
        }

        /// <summary>
        /// Converts this <see cref="float3"/> to a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to convert.</param>
        /// <returns>This <see cref="float3"/> as a <see cref="Quaternion"/>.</returns>
        [BurstCompile]
        public static quaternion ToQuaternion(this float3 _Float3)
        {
            return quaternion.Euler(math.radians(_Float3));
        }
        #endregion
    }
}
