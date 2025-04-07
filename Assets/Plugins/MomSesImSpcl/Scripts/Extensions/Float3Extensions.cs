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
        /// Returns the average value of all three axes of this <see cref="float3"/>.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to get the axis avcerage from.</param>
        /// <returns>The average value of all three axes of this <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float Average(this float3 _Float3)
        {
            return (_Float3.x + _Float3.y + _Float3.z) / 3f;
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
        
        /// <summary>
        /// Sets the x component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <returns>A new <see cref="float3"/> with the modified x component.</returns>
        [BurstCompile]
        public static float3 WithX(this float3 _Float3, float _X)
        {
            _Float3.x = _X;
            return _Float3;
        }

        /// <summary>
        /// Sets the y component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> whose Y component is to be modified.</param>
        /// <param name="_Y">The value to set or add to the Y component.</param>
        /// <returns>A new <see cref="float3"/> with the modified Y component.</returns>
        [BurstCompile]
        public static float3 WithY(this float3 _Float3, float _Y)
        {
            _Float3.y = _Y;
            return _Float3;
        }

        /// <summary>
        /// Sets the z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to modify.</param>
        /// <param name="_Z">The value to set or add to the Z component.</param>
        /// <returns>A new <see cref="float3"/> with the modified Z component.</returns>
        [BurstCompile]
        public static float3 WithZ(this float3 _Float3, float _Z)
        {
            _Float3.z = _Z;
            return _Float3;
        }

        /// <summary>
        /// Sets the x and y component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The original <see cref="float3"/> to be updated.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <returns>A new <see cref="float3"/> with the updated x and y values.</returns>
        [BurstCompile]
        public static float3 WithXY(this float3 _Float3, float _X, float _Y)
        {
            _Float3.x = _X;
            _Float3.y = _Y;

            return _Float3;
        }

        /// <summary>
        /// Sets the x and y component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithXY(this float3 _Float3, float _Value)
        {
            _Float3.x = _Value;
            _Float3.y = _Value;

            return _Float3;
        }
        
        /// <summary>
        /// Sets the x and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The Vector3 to be adjusted.</param>
        /// <param name="_X">The value to set or add to the x component.</param>
        /// <param name="_Z">The value to set or add to the z component.</param>
        /// <returns>A new Vector3 with the adjusted x and z components.</returns>
        [BurstCompile]
        public static float3 WithXZ(this float3 _Float3, float _X, float _Z)
        {
            _Float3.x = _X;
            _Float3.z = _Z;

            return _Float3;
        }

        /// <summary>
        /// Sets the x and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithXZ(this float3 _Float3, float _Value)
        {
            _Float3.x = _Value;
            _Float3.z = _Value;

            return _Float3;
        }
        
        /// <summary>
        /// Sets the y and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> to be modified.</param>
        /// <param name="_Y">The value to set (or add to) the Y component.</param>
        /// <param name="_Z">The value to set (or add to) the Z component.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithYZ(this float3 _Float3, float _Y, float _Z)
        {
            _Float3.y = _Y;
            _Float3.z = _Z;

            return _Float3;
        }

        /// <summary>
        /// Sets the y and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithYZ(this float3 _Float3, float _Value)
        {
            _Float3.y = _Value;
            _Float3.z = _Value;

            return _Float3;
        }
        
        /// <summary>
        /// Sets the x, y and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> instance to modify.</param>
        /// <param name="_X">The new x value or the value to add to the current x component.</param>
        /// <param name="_Y">The new y value or the value to add to the current y component.</param>
        /// <param name="_Z">The new z value or the value to add to the current z component.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithXYZ(this float3 _Float3, float _X, float _Y, float _Z)
        {
            _Float3.x = _X;
            _Float3.y = _Y;
            _Float3.z = _Z;

            return _Float3;
        }
        
        /// <summary>
        /// Sets the x, y and z component of a <see cref="float3"/> to a specified value.
        /// </summary>
        /// <param name="_Float3">The <see cref="float3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="float3"/>.</returns>
        [BurstCompile]
        public static float3 WithXYZ(this float3 _Float3, float _Value)
        {
            _Float3.x = _Value;
            _Float3.y = _Value;
            _Float3.z = _Value;

            return _Float3;
        }
        #endregion
    }
}
