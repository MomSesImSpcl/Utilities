using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Vector3"/>.
    /// </summary>
    public static class Vector3Extensions
    {
        #region Methods
        /// <summary>
        /// Calculates the angle between this <see cref="Vector3"/> and the given target.
        /// </summary>
        /// <param name="_Origin">The <see cref="Transform.position"/> to calculate the angle from.</param>
        /// <param name="_Target">The <see cref="Transform.position"/> to calculate the angle to.</param>
        /// <returns>The angle between this <see cref="Vector3"/> and the given target.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(this Vector3 _Origin, Vector3 _Target)
        {
            var _direction = _Origin.Direction(_Target);
            var _mouseAngle = math.atan2(-_direction.y, _direction.x) * math.TODEGREES;

            return _mouseAngle;
        }
        
        /// <summary>
        /// Returns the average value of all three axes of this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to get the axis average from.</param>
        /// <returns>The average value of all three axes of this <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Average(this Vector3 _Vector3)
        {
            return (_Vector3.x + _Vector3.y + _Vector3.z) / 3f;
        }

        /// <summary>
        /// Clamps this <see cref="Vector3"/> between the min and max of the given <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to clamp.</param>
        /// <param name="_Min">The min values for this <see cref="Vector3"/>.</param>
        /// <param name="_Max">The max values for this <see cref="Vector3"/>.</param>
        /// <returns>This <see cref="Vector3"/> clamped between the given min and max values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Clamp(this Vector3 _Vector3, Vector3 _Min, Vector3 _Max)
        {
            var _x = math.clamp(_Vector3.x, _Min.x, _Max.x);
            var _y = math.clamp(_Vector3.y, _Min.y, _Max.y);
            var _z = math.clamp(_Vector3.z, _Min.z, _Max.z);
            
            return new Vector3(_x, _y, _z);
        }
        
        /// <summary>
        /// Computes the direction vector from this <see cref="Vector3"/> to the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_From">Origin <see cref="Transform.position"/>.</param>
        /// <param name="_To">Target <see cref="Transform.position"/>.</param>
        /// <returns>The direction vector from this <see cref="Vector3"/> to the given <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Direction(this Vector3 _From, Vector3 _To)
        {
            return _To - _From;
        }
        
        /// <summary>
        /// Calculates the distance from this <see cref="Vector3"/> to the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_From">From where to calculate the distance.</param>
        /// <param name="_To">To where to calculate the distance.</param>
        /// <returns>The distance between the two <see cref="Vector3"/>s.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(this Vector3 _From, Vector3 _To)
        {
            return math.distance(_From, _To);
        }
        
        /// <summary>
        /// Divides the components of a <see cref="Vector3"/> by a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be divided.</param>
        /// <param name="_Axis">The axis along which to divide the component(s).</param>
        /// <param name="_Value">The value by which to divide the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the divided values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x / _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y / _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z / _Value),
            Axis.XY => new Vector3(_Vector3.x / _Value, _Vector3.y / _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x / _Value, _Vector3.y, _Vector3.z / _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y / _Value, _Vector3.z / _Value),
            Axis.XYZ => new Vector3(_Vector3.x / _Value, _Vector3.y / _Value, _Vector3.z / _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };
        
        /// <summary>
        /// Divides the components of a Vector3 by a specified value.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be divided from.</param>
        /// <param name="_Value">The value to divide from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the divided value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(this Vector3 _Vector3, float _Value)
        {
            return new Vector3(_Vector3.x / _Value, _Vector3.y / _Value, _Vector3.z / _Value);
        }
        
        /// <summary>
        /// Divides the components of a Vector3 by a specified values.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be divided from.</param>
        /// <param name="_Values">The values to divide from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the divided values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(this Vector3 _Vector3, Vector3 _Values)
        {
            return new Vector3(_Vector3.x / _Values.x, _Vector3.y / _Values.y, _Vector3.z / _Values.z);
        }
        
        /// <summary>
        /// Gets a new <see cref="Vector3"/> with the values of this <see cref="Vector3"/> for the given <see cref="Axis"/>. <br/>
        /// <i>ALl other <see cref="Axis"/> values will be <c>0</c>.</i>
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to get the value from.</param>
        /// <param name="_Axis">The <see cref="Axis"/> from which to get the value.</param>
        /// <returns>The values for the given <see cref="Axis"/> of this <see cref="Vector3"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Get(this Vector3 _Vector3, Axis _Axis) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x, 0, 0),
            Axis.Y => new Vector3(0, _Vector3.y, 0),
            Axis.Z => new Vector3(0, 0, _Vector3.z),
            Axis.XY => new Vector3(_Vector3.x, _Vector3.y, 0),
            Axis.XZ => new Vector3(_Vector3.x, 0, _Vector3.z),
            Axis.YZ => new Vector3(0, _Vector3.y, _Vector3.z),
            Axis.XYZ => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };
        
        /// <summary>
        /// Calculates the 2D <see cref="Transform.rotation"/> needed to look at a target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="_Source">Current <see cref="Transform.position"/>.</param>
        /// <param name="_Target">Target <see cref="Transform.position"/> to look at.</param>
        /// <param name="_OffsetDegrees">Additional angular offset (e.g., -90 if sprite faces up).</param>
        /// <param name="_DefaultRotation"><see cref="Transform.rotation"/> to return if <see cref="Transform.position"/>s are identical.</param>
        /// <returns>The <see cref="Transform.rotation"/> that is needed to look at the given <c>_Target</c>.</returns>
        public static Quaternion Get2DLookAtRotation(this Vector3 _Source, Vector3 _Target, float _OffsetDegrees = -90f, Quaternion? _DefaultRotation = null)
        {
            var _direction = _Target - _Source;

            if (_direction.sqrMagnitude < math.EPSILON)
            {
                return _DefaultRotation ?? Quaternion.identity;
            }

            var _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            
            return Quaternion.Euler(0f, 0f, _angle + _OffsetDegrees);
        }
        
        /// <summary>
        /// Calculates the <see cref="Transform.rotation"/> needed to align a specific local axis with a target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="_Source">Current <see cref="Transform.position"/> in world space.</param>
        /// <param name="_Target">Target <see cref="Transform.position"/> to look at.</param>
        /// <param name="_LocalAxis">Local axis to align with target direction (default: <see cref="Vector3"/>.<see cref="Vector3.forward"/>).</param>
        /// <param name="_Up">World <see cref="Vector3.up"/> vector for orientation (default: <see cref="Vector3"/>.<see cref="Vector3.up"/>).</param>
        /// <param name="_DefaultRotation">Fallback <see cref="Transform.rotation"/> if <see cref="Transform.position"/>s are identical.</param>
        /// <returns>The <see cref="Transform.rotation"/> that is needed to look at the given <c>_Target</c>.</returns>
        public static Quaternion Get3DLookAtRotation(this Vector3 _Source, Vector3 _Target, Vector3 _LocalAxis = default, Vector3 _Up = default, Quaternion? _DefaultRotation = null)
        {
            var _direction = _Target - _Source;

            // Handle zero direction case.
            if (_direction.sqrMagnitude < math.EPSILON)
            {
                return _DefaultRotation ?? Quaternion.identity;
            }

            _direction.Normalize();

            // Set defaults if not specified.
            _LocalAxis = _LocalAxis == default ? Vector3.forward : _LocalAxis.normalized;
            _Up = _Up == default ? Vector3.up : _Up.normalized;

            // Handle collinear direction and up vectors.
            if (Mathf.Abs(math.dot(_direction, _Up)) > .9999f)
            {
                _Up = math.cross(_direction, Vector3.right).Normalize();
                
                if (_Up.sqrMagnitude < .01f)
                {
                    _Up = math.cross(_direction, Vector3.forward).Normalize();
                }
            }

            // Calculate target rotation.
            var _lookRotation = Quaternion.LookRotation(_direction, _Up);
        
            // Calculate axis alignment adjustment.
            var _axisAlignment = Quaternion.Inverse(Quaternion.LookRotation(_LocalAxis, _Up));

            return _lookRotation * _axisAlignment;
        }
        
        /// <summary>
        /// Returns a <see cref="Vector3"/> that consists of the biggest values from every <see cref="Vector3"/> in the given  <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to get the values from.</param>
        /// <returns>A <see cref="Vector3"/> that consists of the biggest values from every <see cref="Vector3"/> in the given  <see cref="IEnumerable{T}"/>.</returns>
        public static Vector3 GetBiggest(this IEnumerable<Vector3> _IEnumerable)
        {
            var _x = 0f;
            var _y = 0f;
            var _z = 0f;
            
            foreach (var _vector3 in _IEnumerable)
            {
                _x = Mathf.Max(_x, _vector3.x);
                _y = Mathf.Max(_y, _vector3.y);
                _z = Mathf.Max(_z, _vector3.z);
            }

            return new Vector3(_x, _y, _z);
        }
        
        /// <summary>
        /// Gets a points on the circumference of a circle around the <see cref="Transform.position"/> of this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Center">The center of the circle to get the point around.</param>
        /// <param name="_Direction">The direction around which the point will be calculated.</param>
        /// <param name="_Radius">Distance from the center to the target point.</param>
        /// <param name="_Angle">
        /// The angle at which to get the point. <br/>
        /// <i><c>0</c> will be to the right of the center <see cref="Transform.position"/>.</i> <br/>
        /// <i>Positive values will be added anti-clockwise, negative will be added clockwise.</i>
        /// </param>
        /// <param name="_Visualize">If set to <c>true</c>, the point will be visualized.</param>
        /// <returns>A points on the circumference of a circle around the <see cref="Transform.position"/> of this <see cref="Vector2"/>.</returns>
        public static Vector3 GetPointAround(this Vector3 _Center, Vector3 _Direction, float _Radius, float _Angle, bool _Visualize = false)
        {
            if (_Direction == Vector3.zero)
            {
                Debug.LogError("The direction can't be zero.");
                return _Center;
            }
            
            var _normal = math.normalize(_Direction);
            math.orthonormal_basis(_normal, out var _tangent, out var _bitangent);
            var _radians = _Angle * math.PI / 180f;
            Vector3 _offset = _Radius * (Mathf.Cos(_radians) * _tangent + Mathf.Sin(_radians) * _bitangent);
            
#if UNITY_EDITOR
            if (_Visualize)
            {
                Draw.Angle(_Center, _Direction, _Radius, _Angle);
            }
#endif
            return _Center + _offset;
        }
        
        /// <summary>
        /// Returns a <see cref="Vector3"/> that consists of the smallest values from every <see cref="Vector3"/> in the given  <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to get the values from.</param>
        /// <returns>A <see cref="Vector3"/> that consists of the smallest values from every <see cref="Vector3"/> in the given  <see cref="IEnumerable{T}"/>.</returns>
        public static Vector3 GetSmallest(this IEnumerable<Vector3> _IEnumerable)
        {
            var _x = 0f;
            var _y = 0f;
            var _z = 0f;
            
            foreach (var _vector3 in _IEnumerable)
            {
                _x = Mathf.Min(_x, _vector3.x);
                _y = Mathf.Min(_y, _vector3.y);
                _z = Mathf.Min(_z, _vector3.z);
            }

            return new Vector3(_x, _y, _z);
        }
        
        /// <summary>
        /// Checks if any value of this <see cref="Vector3"/> is greater than the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">This <see cref="Vector3"/>.</param>
        /// <param name="_Other">The <see cref="Vector3"/> to compare to.</param>
        /// <returns><c>true</c> if this <see cref="Vector3"/> is greater, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Greater(this Vector3 _Vector3, Vector3 _Other)
        {
            return _Vector3.x > _Other.x || _Vector3.y > _Other.y || _Vector3.z > _Other.z;
        }
        
        /// <summary>
        /// Determines whether this <see cref="Vector3"/> has reached or exceeded the given <c>_TargetPosition</c>, based on the direction of the given <c>_OriginPosition</c>.
        /// </summary>
        /// <param name="_CurrentPosition">The current <see cref="Transform.position"/> of the <see cref="Vector3"/>.</param>
        /// <param name="_OriginPosition">The original <see cref="Transform.position"/> from where the direction should be calculated from.</param>
        /// <param name="_TargetPosition">The target <see cref="Transform.position"/> to where the direction should be calculated to.</param>
        /// <param name="_PercentageOffset">
        /// A percentual offset for the <c>_TargetPosition</c>, how far away the current position can be, for this method to return <c>true</c>. <br/>
        /// <b>Must be between <c>0</c> and <c>1</c>.</b>
        /// </param>
        /// <returns><c>true</c> if this <see cref="Vector3"/> has reached or exceeded the given <c>_TargetPosition</c>, based on the direction of the given <c>_OriginPosition</c>; otherwise, <c>false</c>.</returns>
        public static bool HasReachedTarget(this Vector3 _CurrentPosition, Vector3 _OriginPosition, Vector3 _TargetPosition, float _PercentageOffset = 1f)
        {
            // Calculate vectors from the origin to the current and target positions.
            var _toCurrent = _CurrentPosition - _OriginPosition;
            var _toTarget = _TargetPosition - _OriginPosition;
            var _direction = _TargetPosition - _OriginPosition;

            // Project these vectors onto the direction vector.
            var _currentProjection = math.dot(_toCurrent, _direction);
            var _targetProjection = math.dot(_toTarget, _direction) * _PercentageOffset;
            
            // If the current projection exceeds the target projection, it has moved beyond.
            return _currentProjection >= _targetProjection;
        }
        
        /// <summary>
        /// Returns the maximum value among the x, y, and z components of the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to evaluate.</param>
        /// <returns>The maximum value among the x, y, and z components.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this Vector3 _Vector3)
        {
            var _value = _Vector3.x;
            
            if (_Vector3.y > _value)
            {
                _value = _Vector3.x;
            }
            if (_Vector3.z > _value)
            {
                _value = _Vector3.x;
            }

            return _value;
        }

        /// <summary>
        /// Calculates the midpoint (average) of two <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="_First">First <see cref="Vector3"/>.</param>
        /// <param name="_Second">Second <see cref="Vector3"/>.</param>
        /// <returns>The midpoint (average) of two <see cref="Vector3"/>s.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 MidPoint(this Vector3 _First, Vector3 _Second)
        {
            return (_First + _Second) * .5f;
        }
        
        /// <summary>
        /// Subtracts the specified value from the components of a <see cref="Vector3"/> along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be subtracted from.</param>
        /// <param name="_Axis">The axis along which to subtract the component(s).</param>
        /// <param name="_Value">The value to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the subtracted values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Minus(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x - _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y - _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z - _Value),
            Axis.XY => new Vector3(_Vector3.x - _Value, _Vector3.y - _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x - _Value, _Vector3.y, _Vector3.z - _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y - _Value, _Vector3.z - _Value),
            Axis.XYZ => new Vector3(_Vector3.x - _Value, _Vector3.y - _Value, _Vector3.z - _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Subtracts the components of a Vector3 by a specified value.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be subtracted from.</param>
        /// <param name="_Value">The value to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the subtracted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Minus(this Vector3 _Vector3, float _Value)
        {
            return new Vector3(_Vector3.x - _Value, _Vector3.y - _Value, _Vector3.z - _Value);
        }
        
        /// <summary>
        /// Subtracts the components of a Vector3 by a specified values.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be subtracted from.</param>
        /// <param name="_Values">The values to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the subtracted values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Minus(this Vector3 _Vector3, Vector3 _Values)
        {
            return new Vector3(_Vector3.x - _Values.x, _Vector3.y - _Values.y, _Vector3.z - _Values.z);
        }
        
        /// <summary>
        /// Multiplies the components of a Vector3 by a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The Vector3 to be multiplied.</param>
        /// <param name="_Axis">The axis along which to multiply the component(s).</param>
        /// <param name="_Value">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector3 with the multiplied values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x * _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y * _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z * _Value),
            Axis.XY => new Vector3(_Vector3.x * _Value, _Vector3.y * _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x * _Value, _Vector3.y, _Vector3.z * _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y * _Value, _Vector3.z * _Value),
            Axis.XYZ => new Vector3(_Vector3.x * _Value, _Vector3.y * _Value, _Vector3.z * _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Multiplies the components of a Vector3 by a specified value.
        /// </summary>
        /// <param name="_Vector3">The Vector3 to be multiplied.</param>
        /// <param name="_Value">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector3 with the multiplied values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Vector3 _Vector3, float _Value)
        {
            return new Vector3(_Vector3.x * _Value, _Vector3.y * _Value, _Vector3.z * _Value);
        }
        
        /// <summary>
        /// Multiplies the components of a Vector3 by a specified values.
        /// </summary>
        /// <param name="_Vector3">The Vector3 to be multiplied.</param>
        /// <param name="_Values">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector3 with the multiplied values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Vector3 _Vector3, Vector3 _Values)
        {
            return new Vector3(_Vector3.x * _Values.x, _Vector3.y * _Values.y, _Vector3.z * _Values.z);
        }
        
        /// <summary>
        /// Oscillates around this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="Vector3.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="Vector3.y"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="Vector3.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="Vector3.y"/>-axis.</param>
        /// <returns>This <see cref="Vector3"/> with the applied oscillation.</returns>
        public static Vector3 Oscillate(this Vector3 _Vector3, float _OscillationSpeed, float _XAmplitude, float _YAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.3f, float _YFrequency = 1.7f)
        {
            return _Vector3.Oscillate(0, _OscillationSpeed, _XAmplitude, _YAmplitude, _InvertDirection, _NoiseMultiplier, _XFrequency, _YFrequency);
        }
        
        /// <summary>
        /// Oscillates around this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to oscillate around.</param>
        /// <param name="_StartTime">
        /// When the oscillation is stopped and started frequently, the start can look janky if <see cref="Time.realtimeSinceStartup"/> has a high value. <br/>
        /// In this case cache the timestamp when this method is first started again and pass it in.
        /// </param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="Vector3.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="Vector3.y"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="Vector3.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="Vector3.y"/>-axis.</param>
        /// <returns>This <see cref="Vector2"/> with the applied oscillation.</returns>
        public static Vector3 Oscillate(this Vector3 _Vector3, float _StartTime, float _OscillationSpeed, float _XAmplitude, float _YAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.3f, float _YFrequency = 1.7f)
        {
            var _time = (Time.realtimeSinceStartup - _StartTime) * _OscillationSpeed;
            var _sinX = Mathf.Sin(_time * _XFrequency);
            var _sinY = Mathf.Sin(_time * _YFrequency);
            var _noise = _time * _NoiseMultiplier;
            var _noiseX = Mathf.PerlinNoise(_noise, 0f) * 2 - 1;
            var _noiseY = Mathf.PerlinNoise(0f, _noise) * 2 - 1;
            var _direction = _InvertDirection.Reverse().AsSignedInt();
            var _x = _Vector3.x + (_sinX + _noiseX) * (_XAmplitude * .5f) * _direction;
            var _y = _Vector3.y + (_sinY + _noiseY) * (_YAmplitude * .5f) * _direction;

            return _Vector3.WithXY(_x, _y);
        }
        
        /// <summary>
        /// Performs a mathematical <see cref="Utilities.Operation"/> on the given <see cref="Axis"/> of this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to modify.</param>
        /// <param name="_Operation">The mathematical operation to apply.</param>
        /// <param name="_Axis">The <see cref="Axis"/> on which to apply the <see cref="Utilities.Operation"/>.</param>
        /// <param name="_Value">The value to use for the <see cref="Utilities.Operation"/>.</param>
        /// <returns>A new <see cref="Vector3"/> computed with the specified <see cref="Utilities.Operation"/> applied to the given <see cref="Axis"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="Utilities.Operation"/> is not valid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Operation(this Vector3 _Vector3, Operation _Operation, Axis _Axis, float _Value)
        {
            return _Operation switch
            {
                Utilities.Operation.Add => _Vector3.Plus(_Axis, _Value),
                Utilities.Operation.Subtract => _Vector3.Minus(_Axis, _Value),
                Utilities.Operation.Multiply => _Vector3.Multiply(_Axis, _Value),
                Utilities.Operation.Divide => _Vector3.Divide(_Axis, _Value),
                _ => throw new ArgumentOutOfRangeException(nameof(_Operation), _Operation, null)
            };
        }
        
        /// <summary>
        /// Performs a mathematical <see cref="Utilities.Operation"/> with the given <c>_Values</c>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to modify.</param>
        /// <param name="_Operation">The mathematical operation to apply.</param>
        /// <param name="_Values">
        /// The values to use for the <see cref="Utilities.Operation"/>. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <returns>A new <see cref="Vector3"/> computed with the specified <see cref="Utilities.Operation"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="Utilities.Operation"/> is not valid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Operation(this Vector3 _Vector3, Operation _Operation, Vector3 _Values)
        {
            return _Operation switch
            {
                Utilities.Operation.Add => _Vector3 + _Values,
                Utilities.Operation.Subtract => _Vector3 - _Values,
                Utilities.Operation.Multiply => new Vector3(_Vector3.x * _Values.x, _Vector3.y * _Values.y, _Vector3.z * _Values.z),
                Utilities.Operation.Divide => new Vector3(_Vector3.x / _Values.x, _Vector3.y / _Values.y, _Vector3.z / _Values.z),
                _ => throw new ArgumentOutOfRangeException(nameof(_Operation), _Operation, null)
            };
        }
        
        /// <summary>
        /// Adds a specified value to the components of a <see cref="Vector3"/> along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to which the value will be added.</param>
        /// <param name="_Axis">The axis along which to add the value to the component(s).</param>
        /// <param name="_Value">The value to add to the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the added values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Plus(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x + _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y + _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z + _Value),
            Axis.XY => new Vector3(_Vector3.x + _Value, _Vector3.y + _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x + _Value, _Vector3.y, _Vector3.z + _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y + _Value, _Vector3.z + _Value),
            Axis.XYZ => new Vector3(_Vector3.x + _Value, _Vector3.y + _Value, _Vector3.z + _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };
        
        /// <summary>
        /// Adds the components of a Vector3 by a specified value.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to add to.</param>
        /// <param name="_Value">The value to add to the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the added value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Plus(this Vector3 _Vector3, float _Value)
        {
            return new Vector3(_Vector3.x + _Value, _Vector3.y + _Value, _Vector3.z + _Value);
        }
        
        /// <summary>
        /// Adds the components of a Vector3 by a specified values.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to add to.</param>
        /// <param name="_Values">The values to add to the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the added values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Plus(this Vector3 _Vector3, Vector3 _Values)
        {
            return new Vector3(_Vector3.x + _Values.x, _Vector3.y + _Values.y, _Vector3.z + _Values.z);
        }
        
        /// <summary>
        /// Sets the values of <c>_Vector3</c>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to set the values for.</param>
        /// <param name="_Values">The values to set.</param>
        /// <returns>A new <see cref="Vector3"/> with the set values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Set(this Vector3 _Vector3, Vector3 _Values)
        {
            return new Vector3(_Values.x, _Values.y, _Values.z);
        }
        
        /// <summary>
        /// Sets the component(s) of a <see cref="Vector3"/> to a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be modified.</param>
        /// <param name="_Axis">The axis along which to set the component(s).</param>
        /// <param name="_Value">The value to set the component(s) to.</param>
        /// <returns>A new <see cref="Vector3"/> with the set values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Set(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Value),
            Axis.XY => new Vector3(_Value, _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Value, _Vector3.y, _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Value, _Value),
            Axis.XYZ => new Vector3(_Value, _Value, _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Sets the component(s) of this <see cref="Vector3"/> to a specified value(s) along the given axes.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be modified.</param>
        /// <param name="_Axis">The axes along which to set the component(s).</param>
        /// <param name="_Value">The value(s) to set the component(s) to.</param>
        /// <returns>A new <see cref="Vector3"/> with the set values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Set(this Vector3 _Vector3, Axis _Axis, Vector3 _Value)
        {
            var _vector3 = _Value.Get(_Axis);

            return _Axis switch
            {
                Axis.X => _Vector3.Set(Axis.X, _vector3.x),
                Axis.Y => _Vector3.Set(Axis.Y, _vector3.y),
                Axis.Z => _Vector3.Set(Axis.Z, _vector3.z),
                Axis.XY => _Vector3.Set(Axis.X, _vector3.x).Set(Axis.Y, _vector3.y),
                Axis.XZ => _Vector3.Set(Axis.X, _vector3.x).Set(Axis.Z, _vector3.z),
                Axis.YZ => _Vector3.Set(Axis.Y, _vector3.y).Set(Axis.Z, _vector3.z),
                Axis.XYZ => _Vector3.Set(Axis.X, _vector3.x).Set(Axis.Y, _vector3.y).Set(Axis.Z, _vector3.z),
                _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
            };
        }

        /// <summary>
        /// Checks if any value of this <see cref="Vector3"/> is smaller than the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">This <see cref="Vector3"/>.</param>
        /// <param name="_Other">The <see cref="Vector3"/> to compare to.</param>
        /// <returns><c>true</c> if this <see cref="Vector3"/> is smaller, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Smaller(this Vector3 _Vector3, Vector3 _Other)
        {
            return _Vector3.x < _Other.x || _Vector3.y < _Other.y || _Vector3.z < _Other.z;
        }
        
        /// <summary>
        /// Converts this <see cref="Vector3"/> to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to convert.</param>
        /// <returns>A new <see cref="Vector2"/> with the value of this <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(this Vector3 _Vector3)
        {
            return new Vector2(_Vector3.x, _Vector3.y);
        }
        
        /// <summary>
        /// Converts this <see cref="Vector3"/> to a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to convert to a <see cref="Quaternion"/>.</param>
        /// <returns>This <see cref="Vector3"/> converted to a <see cref="Quaternion"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ToQuaternion(this Vector3 _Vector3)
        {
            return Quaternion.Euler(_Vector3);
        }
        
        /// <summary>
        /// Sets the x component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <returns>A new <see cref="Vector3"/> with the modified x component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 _Vector, float _X)
        {
            _Vector.x = _X;
            return _Vector;
        }

        /// <summary>
        /// Sets the y component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> whose Y component is to be modified.</param>
        /// <param name="_Y">The value to set or add to the Y component.</param>
        /// <returns>A new <see cref="Vector3"/> with the modified Y component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 _Vector, float _Y)
        {
            _Vector.y = _Y;
            return _Vector;
        }

        /// <summary>
        /// Sets the z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> to modify.</param>
        /// <param name="_Z">The value to set or add to the Z component.</param>
        /// <returns>A new <see cref="Vector3"/> with the modified Z component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 _Vector, float _Z)
        {
            _Vector.z = _Z;
            return _Vector;
        }

        /// <summary>
        /// Sets the x and y component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The original <see cref="Vector3"/> to be updated.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <returns>A new <see cref="Vector3"/> with the updated x and y values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXY(this Vector3 _Vector, float _X, float _Y)
        {
            _Vector.x = _X;
            _Vector.y = _Y;

            return _Vector;
        }

        /// <summary>
        /// Sets the x and y component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXY(this Vector3 _Vector, float _Value)
        {
            _Vector.x = _Value;
            _Vector.y = _Value;

            return _Vector;
        }
        
        /// <summary>
        /// Sets the x and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The Vector3 to be adjusted.</param>
        /// <param name="_X">The value to set or add to the x component.</param>
        /// <param name="_Z">The value to set or add to the z component.</param>
        /// <returns>A new Vector3 with the adjusted x and z components.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXZ(this Vector3 _Vector, float _X, float _Z)
        {
            _Vector.x = _X;
            _Vector.z = _Z;

            return _Vector;
        }

        /// <summary>
        /// Sets the x and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXZ(this Vector3 _Vector, float _Value)
        {
            _Vector.x = _Value;
            _Vector.z = _Value;

            return _Vector;
        }
        
        /// <summary>
        /// Sets the y and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> to be modified.</param>
        /// <param name="_Y">The value to set (or add to) the Y component.</param>
        /// <param name="_Z">The value to set (or add to) the Z component.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithYZ(this Vector3 _Vector, float _Y, float _Z)
        {
            _Vector.y = _Y;
            _Vector.z = _Z;

            return _Vector;
        }

        /// <summary>
        /// Sets the y and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithYZ(this Vector3 _Vector, float _Value)
        {
            _Vector.y = _Value;
            _Vector.z = _Value;

            return _Vector;
        }
        
        /// <summary>
        /// Sets the x, y and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> instance to modify.</param>
        /// <param name="_X">The new x value or the value to add to the current x component.</param>
        /// <param name="_Y">The new y value or the value to add to the current y component.</param>
        /// <param name="_Z">The new z value or the value to add to the current z component.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXYZ(this Vector3 _Vector, float _X, float _Y, float _Z)
        {
            _Vector.x = _X;
            _Vector.y = _Y;
            _Vector.z = _Z;

            return _Vector;
        }
        
        /// <summary>
        /// Sets the x, y and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> instance to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithXYZ(this Vector3 _Vector, float _Value)
        {
            _Vector.x = _Value;
            _Vector.y = _Value;
            _Vector.z = _Value;

            return _Vector;
        }
        #endregion
    }
}
