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
        public static Vector3 AsVector3(this float3 _Float3)
        {
            return new Vector3(_Float3.x, _Float3.y, _Float3.z);
        }
        
        /// <summary>
        /// Normalizes this <see cref="float3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to normalize.</param>
        /// <returns>This <see cref="float3"/> with normalized values.</returns>
        [BurstCompile]
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
        /// <param name="_FadeInDuration">Time in seconds it takes to fade into the starting oscillation value.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="float3.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="float3.y"/> value can move from its original value.</param>
        /// <param name="_ZAmplitude">Controls how far the <see cref="float3.z"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="float3.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="float3.y"/>-axis.</param>
        /// <param name="_ZFrequency">Controls the oscillation speed on the <see cref="float3.z"/>-axis.</param>
        /// <returns>This <see cref="Vector2"/> with the applied oscillation.</returns>
        [BurstCompile]
        public static float3 Oscillate(this float3 _Float3, float _RealtimeSinceStartup, float _StartTime, float _OscillationSpeed, float _FadeInDuration, float _XAmplitude, float _YAmplitude, float _ZAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.75f, float _YFrequency = 1.25f, float _ZFrequency = .5f)
        {
            var _timeSinceStart = _RealtimeSinceStartup - _StartTime;
            var _fadeInMultiplier = math.min(1f, _timeSinceStart / _FadeInDuration);
            
            var _time = _timeSinceStart * _OscillationSpeed;
            var _sinX = math.sin(_time * _XFrequency);
            var _sinY = math.sin(_time * _YFrequency);
            var _sinZ = math.sin(_time * _ZFrequency);
            var _noise = _time * _NoiseMultiplier;
            var _noiseX = noise.snoise(new float3(_noise, 0f, 0f)) * 2 - 1;
            var _noiseY = noise.snoise(new float3(0f, _noise, 0f)) * 2 - 1;
            var _noiseZ = noise.snoise(new float3(0f, 0f, _noise)) * 2 - 1;
            var _direction = _InvertDirection.Reverse().AsSignedInt();
            
            var _x = _Float3.x + (_sinX + _noiseX) * (_XAmplitude * .5f) * _direction * _fadeInMultiplier;
            var _y = _Float3.y + (_sinY + _noiseY) * (_YAmplitude * .5f) * _direction * _fadeInMultiplier;
            var _z = _Float3.z + (_sinZ + _noiseZ) * (_ZAmplitude * .5f) * _direction * _fadeInMultiplier;
            
            return new float3(_x, _y, _z);
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
