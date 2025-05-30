using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Vector2"/>.
    /// </summary>
    public static class Vector2Extensions
    {
        #region
        /// <summary>
        /// Calculates the angle between this <see cref="Vector2"/> and the given target.
        /// </summary>
        /// <param name="_Origin">The <see cref="Transform.position"/> to calculate the angle from.</param>
        /// <param name="_Target">The <see cref="Transform.position"/> to calculate the angle to.</param>
        /// <returns>The angle between this <see cref="Vector2"/> and the given target.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(this Vector2 _Origin, Vector2 _Target)
        {
            var _direction = _Origin.Direction(_Target);
            var _mouseAngle = math.atan2(-_direction.y, _direction.x) * math.TODEGREES;

            return _mouseAngle;
        }
        
        /// <summary>
        /// Returns the average value of all three axes of this <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to get the axis average from.</param>
        /// <returns>The average value of all three axes of this <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Average(this Vector2 _Vector2)
        {
            return (_Vector2.x + _Vector2.y) / 2f;
        }
        
        /// <summary>
        /// Clamps this <see cref="Vector2"/> between the given min and max value.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="_ClampMin">The minimum value this <see cref="Vector2"/> can have.</param>
        /// <param name="_ClampMax">The maximum value this <see cref="Vector2"/> can have.</param>
        /// <returns>This <see cref="Vector2"/> clamped between the given min and max value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp(this Vector2 _Vector2, float _ClampMin, float _ClampMax)
        {
            var _x = _Vector2.x.Clamp(_ClampMin, _ClampMax);
            var _y = _Vector2.y.Clamp(_ClampMin, _ClampMax);
            
            return new Vector2(_x, _y);
        }
        
        /// <summary>
        /// Clamps this <see cref="Vector2"/> between the given min and max values.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to clamp.</param>
        /// <param name="_XMin">The minimum value on the <see cref="Vector2.x"/> axis.</param>
        /// <param name="_XMax">The maximum value on the <see cref="Vector2.x"/> axis.</param>
        /// <param name="_YMin">The minimum value on the <see cref="Vector2.y"/> axis.</param>
        /// <param name="_YMax">The maximum value on the <see cref="Vector2.y"/> axis.</param>
        /// <returns>This <see cref="Vector2"/> clamped between the given min and max values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Clamp(this Vector2 _Vector2, float _XMin, float _XMax, float _YMin, float _YMax)
        {
            var _x = _Vector2.x.Clamp(_XMin, _XMax);
            var _y = _Vector2.y.Clamp(_YMin, _YMax);
            
            return new Vector2(_x, _y);
        }

        /// <summary>
        /// Computes the diagonal lenght of this <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to compute the diagonal lenght for.</param>
        /// <returns>The diagonal length of this <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Diagonal(this Vector2 _Vector2)
        {
            return math.sqrt(_Vector2.x * _Vector2.x + _Vector2.y * _Vector2.y);
        }
        
        /// <summary>
        /// Computes the direction vector from this <see cref="Vector2"/> to the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_From">Origin <see cref="Transform.position"/>.</param>
        /// <param name="_To">Target <see cref="Transform.position"/>.</param>
        /// <returns>The direction vector from this <see cref="Vector2"/> to the given <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Direction(this Vector2 _From, Vector2 _To)
        {
            return _To - _From;
        }
        
        /// <summary>
        /// Calculates the distance from this <see cref="Vector2"/> to the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_From">From where to calculate the distance.</param>
        /// <param name="_To">To where to calculate the distance.</param>
        /// <returns>The distance between the two <see cref="Vector2"/>s.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(this Vector2 _From, Vector2 _To)
        {
            return math.distance(_From, _To);
        }
        
        /// <summary>
        /// Divides <c>_Vector2</c> by <c>_Value</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to divide the value for.</param>
        /// <param name="_Axis">The axis on which to divide the value.</param>
        /// <param name="_Value">The value to divide from <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the divided value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Divide(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x / _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y / _Value),
            Axis.XY => new Vector2(_Vector2.x / _Value, _Vector2.y / _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Divides the components of a Vector2 by a specified value.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to be divided from.</param>
        /// <param name="_Value">The value to divide from the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the divided value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Divide(this Vector2 _Vector2, float _Value)
        {
            return new Vector2(_Vector2.x / _Value, _Vector2.y / _Value);
        }
        
        /// <summary>
        /// Divides the components of a Vector2 by a specified values.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to be divided from.</param>
        /// <param name="_Values">The values to divide from the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the divided values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Divide(this Vector2 _Vector2, Vector2 _Values)
        {
            return new Vector2(_Vector2.x / _Values.x, _Vector2.y / _Values.y);
        }
        
        /// <summary>
        /// Gets a new <see cref="Vector2"/> with the values of this <see cref="Vector2"/> for the given <see cref="Axis"/>. <br/>
        /// <i>ALl other <see cref="Axis"/> values will be <c>0</c>.</i>
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector2"/> to get the value from.</param>
        /// <param name="_Axis">The <see cref="Axis"/> from which to get the value.</param>
        /// <returns>The values for the given <see cref="Axis"/> of this <see cref="Vector2"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Get(this Vector2 _Vector3, Axis _Axis) => _Axis switch
        {
            Axis.X => new Vector2(_Vector3.x, 0),
            Axis.Y => new Vector2(0, _Vector3.y),
            Axis.XY => new Vector2(_Vector3.x, _Vector3.y),
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
        public static Quaternion Get2DLookAtRotation(this Vector2 _Source, Vector2 _Target, float _OffsetDegrees = -90f, Quaternion? _DefaultRotation = null)
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
        /// Returns a <see cref="Vector2"/> that consists of the biggest values from every <see cref="Vector2"/> in the given  <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to get the values from.</param>
        /// <returns>A <see cref="Vector2"/> that consists of the biggest values from every <see cref="Vector2"/> in the given  <see cref="IEnumerable{T}"/>.</returns>
        public static Vector2 GetBiggest(this IEnumerable<Vector2> _IEnumerable)
        {
            var _x = 0f;
            var _y = 0f;
            
            foreach (var _vector2 in _IEnumerable)
            {
                _x = Mathf.Max(_x, _vector2.x);
                _y = Mathf.Max(_y, _vector2.y);
            }

            return new Vector2(_x, _y);
        }
        
        /// <summary>
        /// Gets a points on the circumference of a circle around the <see cref="Transform.position"/> of this <see cref="Vector2"/>.
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
        public static Vector2 GetPointAround(this Vector2 _Center, Vector3 _Direction, float _Radius, float _Angle, bool _Visualize = false)
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
            return _Center + _offset.ToVector2();
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> that consists of the smallest values from every <see cref="Vector2"/> in the given  <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="_IEnumerable">The <see cref="IEnumerable{T}"/> to get the values from.</param>
        /// <returns>A <see cref="Vector2"/> that consists of the smallest values from every <see cref="Vector2"/> in the given  <see cref="IEnumerable{T}"/>.</returns>
        public static Vector2 GetSmallest(this IEnumerable<Vector2> _IEnumerable)
        {
            var _x = 0f;
            var _y = 0f;
            
            foreach (var _vector2 in _IEnumerable)
            {
                _x = Mathf.Min(_x, _vector2.x);
                _y = Mathf.Min(_y, _vector2.y);
            }

            return new Vector2(_x, _y);
        }
        
        /// <summary>
        /// Checks if any value of this <see cref="Vector2"/> is greater than the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">This <see cref="Vector2"/>.</param>
        /// <param name="_Other">The <see cref="Vector2"/> to compare to.</param>
        /// <returns><c>true</c> if this <see cref="Vector2"/> is greater, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Greater(this Vector2 _Vector2, Vector2 _Other)
        {
            return _Vector2.x > _Other.x || _Vector2.y > _Other.y;
        }
        
        /// <summary>
        /// Calculates the midpoint (average) of two <see cref="Vector2"/>s.
        /// </summary>
        /// <param name="_First">First <see cref="Vector2"/>.</param>
        /// <param name="_Second">Second <see cref="Vector2"/>.</param>
        /// <returns>The midpoint (average) of two <see cref="Vector2"/>s.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 MidPoint(this Vector2 _First, Vector2 _Second)
        {
            return (_First + _Second) * .5f;
        }
        
        /// <summary>
        /// Subtracts a value from <c>_Vector2</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to subtract the value from.</param>
        /// <param name="_Axis">The axis on which to subtract the value.</param>
        /// <param name="_Value">The value to subtract from <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the subtracted value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Minus(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x - _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y - _Value),
            Axis.XY => new Vector2(_Vector2.x - _Value, _Vector2.y - _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Subtracts the components of a Vector2 by a specified value.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to be subtracted from.</param>
        /// <param name="_Value">The value to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the subtracted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Minus(this Vector2 _Vector2, float _Value)
        {
            return new Vector2(_Vector2.x - _Value, _Vector2.y - _Value);
        }
        
        /// <summary>
        /// Subtracts the components of a Vector2 by a specified values.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to be subtracted from.</param>
        /// <param name="_Values">The values to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the subtracted values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Minus(this Vector2 _Vector2, Vector2 _Values)
        {
            return new Vector2(_Vector2.x - _Values.x, _Vector2.y - _Values.y);
        }
        
        /// <summary>
        /// Multiplies <c>_Vector2</c> by <c>_Value</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to multiply the value for.</param>
        /// <param name="_Axis">The axis on which to multiply the value.</param>
        /// <param name="_Value">The value to multiply with <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the multiplied value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Multiply(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x * _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y * _Value),
            Axis.XY => new Vector2(_Vector2.x * _Value, _Vector2.y * _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Multiplies the components of a Vector2 by a specified value.
        /// </summary>
        /// <param name="_Vector2">The Vector2 to be multiplied.</param>
        /// <param name="_Value">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector2 with the multiplied values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Multiply(this Vector2 _Vector2, float _Value)
        {
            return new Vector2(_Vector2.x * _Value, _Vector2.y * _Value);
        }
        
        /// <summary>
        /// Multiplies the components of a Vector2 by a specified values.
        /// </summary>
        /// <param name="_Vector2">The Vector2 to be multiplied.</param>
        /// <param name="_Values">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector2 with the multiplied values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Multiply(this Vector2 _Vector2, Vector2 _Values)
        {
            return new Vector2(_Vector2.x * _Values.x, _Vector2.y * _Values.y);
        }
        
        /// <summary>
        /// Performs a mathematical <see cref="Utilities.Operation"/> on the given <see cref="Axis"/> of this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_Operation">The mathematical operation to apply.</param>
        /// <param name="_Axis">The <see cref="Axis"/> on which to apply the <see cref="Utilities.Operation"/>.</param>
        /// <param name="_Value">The value to use for the <see cref="Utilities.Operation"/>.</param>
        /// <returns>A new <see cref="Vector2"/> computed with the specified <see cref="Utilities.Operation"/> applied to the given <see cref="Axis"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="Utilities.Operation"/> is not valid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Operation(this Vector2 _Vector2, Operation _Operation, Axis _Axis, float _Value)
        {
            return _Operation switch
            {
                Utilities.Operation.Add => _Vector2.Plus(_Axis, _Value),
                Utilities.Operation.Subtract => _Vector2.Minus(_Axis, _Value),
                Utilities.Operation.Multiply => _Vector2.Multiply(_Axis, _Value),
                Utilities.Operation.Divide => _Vector2.Divide(_Axis, _Value),
                _ => throw new ArgumentOutOfRangeException(nameof(_Operation), _Operation, null)
            };
        }
        
        /// <summary>
        /// Performs a mathematical <see cref="Utilities.Operation"/> with the given <c>_Values</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_Operation">The mathematical operation to apply.</param>
        /// <param name="_Values">
        /// The values to use for the <see cref="Utilities.Operation"/>. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector2"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector2"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <returns>A new <see cref="Vector2"/> computed with the specified <see cref="Utilities.Operation"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="Utilities.Operation"/> is not valid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Operation(this Vector2 _Vector2, Operation _Operation, Vector2 _Values)
        {
            return _Operation switch
            {
                Utilities.Operation.Add => _Vector2 + _Values,
                Utilities.Operation.Subtract => _Vector2 - _Values,
                Utilities.Operation.Multiply => new Vector2(_Vector2.x * _Values.x, _Vector2.y * _Values.y),
                Utilities.Operation.Divide => new Vector2(_Vector2.x / _Values.x, _Vector2.y / _Values.y),
                _ => throw new ArgumentOutOfRangeException(nameof(_Operation), _Operation, null)
            };
        }
        
        /// <summary>
        /// Oscillates around this <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to oscillate around.</param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="Vector2.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="Vector2.y"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="Vector2.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="Vector2.y"/>-axis.</param>
        /// <returns>This <see cref="Vector2"/> with the applied oscillation.</returns>
        public static Vector2 Oscillate(this Vector2 _Vector2, float _OscillationSpeed, float _XAmplitude, float _YAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.3f, float _YFrequency = 1.7f)
        {
            return _Vector2.Oscillate(0, _OscillationSpeed, _XAmplitude, _YAmplitude, _InvertDirection, _NoiseMultiplier, _XFrequency, _YFrequency);
        }
        
        /// <summary>
        /// Oscillates around this <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to oscillate around.</param>
        /// <param name="_StartTime">
        /// When the oscillation is stopped and started frequently, the start can look janky if <see cref="Time.realtimeSinceStartup"/> has a high value. <br/>
        /// In this case cache the timestamp when this method is first started again and pass it in.
        /// </param>
        /// <param name="_OscillationSpeed">Controls the speed of the oscillation.</param>
        /// <param name="_XAmplitude">Controls how far the <see cref="Vector2.x"/> value can move from its original value.</param>
        /// <param name="_YAmplitude">Controls how far the <see cref="Vector2.y"/> value can move from its original value.</param>
        /// <param name="_InvertDirection">Inverts the direction of the oscillation if <c>true</c>.</param>
        /// <param name="_NoiseMultiplier">Higher values increase the randomness, but also the speed.</param>
        /// <param name="_XFrequency">Controls the oscillation speed on the <see cref="Vector2.x"/>-axis.</param>
        /// <param name="_YFrequency">Controls the oscillation speed on the <see cref="Vector2.y"/>-axis.</param>
        /// <returns>This <see cref="Vector2"/> with the applied oscillation.</returns>
        public static Vector2 Oscillate(this Vector2 _Vector2, float _StartTime, float _OscillationSpeed, float _XAmplitude, float _YAmplitude, bool _InvertDirection = false, float _NoiseMultiplier = .5f, float _XFrequency = 1.3f, float _YFrequency = 1.7f)
        {
            var _time = (Time.realtimeSinceStartup - _StartTime) * _OscillationSpeed;
            var _sinX = Mathf.Sin(_time * _XFrequency);
            var _sinY = Mathf.Sin(_time * _YFrequency);
            var _noise = _time * _NoiseMultiplier;
            var _noiseX = Mathf.PerlinNoise(_noise, 0f) * 2 - 1;
            var _noiseY = Mathf.PerlinNoise(0f, _noise) * 2 - 1;
            var _direction = _InvertDirection.Reverse().AsSignedInt();
            var _x = _Vector2.x + (_sinX + _noiseX) * (_XAmplitude * .5f) * _direction;
            var _y = _Vector2.y + (_sinY + _noiseY) * (_YAmplitude * .5f) * _direction;

            return _Vector2.WithXY(_x, _y);
        }
        
        /// <summary>
        /// Adds <c>_Value</c> to <c>_Vector2</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to add the value to.</param>
        /// <param name="_Axis">The axis on which to add the value.</param>
        /// <param name="_Value">The value to add to <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the added value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Plus(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x + _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y + _Value),
            Axis.XY => new Vector2(_Vector2.x + _Value, _Vector2.y + _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Adds the components of a Vector2 by a specified value.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to add to.</param>
        /// <param name="_Value">The value to add to the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the added value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Plus(this Vector2 _Vector2, float _Value)
        {
            return new Vector2(_Vector2.x + _Value, _Vector2.y + _Value);
        }
        
        /// <summary>
        /// Adds the components of a Vector2 by a specified values.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to add to.</param>
        /// <param name="_Values">The values to add to the component(s).</param>
        /// <returns>A new <see cref="Vector2"/> with the added values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Plus(this Vector2 _Vector2, Vector2 _Values)
        {
            return new Vector2(_Vector2.x + _Values.x, _Vector2.y + _Values.y);
        }

        /// <summary>
        /// Sets the values of <c>_Vector2</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to set the values for.</param>
        /// <param name="_Values">The values to set.</param>
        /// <returns>A new <see cref="Vector2"/> with the set values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Set(this Vector2 _Vector2, Vector2 _Values)
        {
            return new Vector2(_Values.x, _Values.y);
        }
        
        /// <summary>
        /// Sets the value of <c>_Vector2</c> on the specified <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to set the value for.</param>
        /// <param name="_Axis">The axis on which to set the value.</param>
        /// <param name="_Value">The value to set for the specified <c>_Axis</c>.</param>
        /// <returns>A new <see cref="Vector2"/> with the set value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Set(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Value),
            Axis.XY => new Vector2(_Value, _Value),
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Sets the component(s) of this <see cref="Vector2"/> to a specified value(s) along the given axes.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to be modified.</param>
        /// <param name="_Axis">The axes along which to set the component(s).</param>
        /// <param name="_Value">The value(s) to set the component(s) to.</param>
        /// <returns>A new <see cref="Vector2"/> with the set values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Set(this Vector2 _Vector2, Axis _Axis, Vector2 _Value)
        {
            var _vector2 = _Value.Get(_Axis);

            return _Axis switch
            {
                Axis.X => _Vector2.Set(Axis.X, _vector2.x),
                Axis.Y => _Vector2.Set(Axis.Y, _vector2.y),
                Axis.XY => _Vector2.Set(Axis.X, _vector2.x).Set(Axis.Y, _vector2.y),
                _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
            };
        }

        /// <summary>
        /// Checks if any value of this <see cref="Vector2"/> is smaller than the given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Vector2">This <see cref="Vector2"/>.</param>
        /// <param name="_Other">The <see cref="Vector2"/> to compare to.</param>
        /// <returns><c>true</c> if this <see cref="Vector2"/> is smaller, otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Smaller(this Vector2 _Vector2, Vector2 _Other)
        {
            return _Vector2.x < _Other.x || _Vector2.y < _Other.y;
        }
        
        /// <summary>
        /// Converts this <see cref="Vector2"/> to a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to convert.</param>
        /// <returns>A new <see cref="Vector3"/> with the value of this <see cref="Vector2"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToVector3(this Vector2 _Vector2)
        {
            return new Vector3(_Vector2.x, _Vector2.y, 0);
        }

        /// <summary>
        /// Converts this <see cref="Vector2"/> to a <see cref="Quaternion"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to convert to a <see cref="Quaternion"/>.</param>
        /// <returns>This <see cref="Vector2"/> converted to a <see cref="Quaternion"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ToQuaternion(this Vector2 _Vector2)
        {
            return Quaternion.Euler(_Vector2);
        }
        
        /// <summary>
        /// Sets the x component of a <see cref="Vector2"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_X">The x-coordinate to set or add.</param>
        /// <returns>This <c>Vector2</c> with the modified x-coordinate.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithX(this Vector2 _Vector, float _X)
        {
            _Vector.x = _X;
            
            return _Vector;
        }

        /// <summary>
        /// Sets the y component of a <see cref="Vector2"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector2"/> to modify the y-component for.</param>
        /// <param name="_Y">The value to set or add to the y-component.</param>
        /// <returns>This <c>_Vector</c> with the modified y-component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithY(this Vector2 _Vector, float _Y)
        {
            _Vector.y = _Y;
            
            return _Vector;
        }

        /// <summary>
        /// Sets the x, y component of a <see cref="Vector2"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The original <see cref="Vector2"/> to modify.</param>
        /// <param name="_X">The new value for the <c>X</c> component.</param>
        /// <param name="_Y">The new value for the <c>Y</c> component.</param>
        /// <returns>A new <see cref="Vector2"/> with the modified <c>X</c> and <c>Y</c> values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithXY(this Vector2 _Vector, float _X, float _Y)
        {
            _Vector.x = _X;
            _Vector.y = _Y;
            
            return _Vector;
        }
        
        /// <summary>
        /// Sets the x, y component of a <see cref="Vector2"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The original <see cref="Vector2"/> to modify.</param>
        /// <param name="_Value">The new value to set to every axis.</param>
        /// <returns>A new <see cref="Vector2"/> with the modified <c>X</c> and <c>Y</c> values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithXY(this Vector2 _Vector, float _Value)
        {
            _Vector.x = _Value;
            _Vector.y = _Value;

            return _Vector;
        }
        #endregion
    }
}
