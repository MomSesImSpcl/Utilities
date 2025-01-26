using System;
using MomSesImSpcl.Utilities;
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
        /// Divides the components of a <see cref="Vector3"/> by a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be divided.</param>
        /// <param name="_Axis">The axis along which to divide the component(s).</param>
        /// <param name="_Value">The value by which to divide the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the divided values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static Vector3 Divide(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x / _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y / _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z / _Value),
            Axis.XY => new Vector3(_Vector3.x / _Value, _Vector3.y / _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x / _Value, _Vector3.y, _Vector3.z / _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y / _Value, _Vector3.z / _Value),
            Axis.XYZ => new Vector3(_Vector3.x / _Value, _Vector3.y / _Value, _Vector3.z / _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Get the value of the given <see cref="Axis"/> from this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to get the value from.</param>
        /// <param name="_Axis">The <see cref="Axis"/> from which to get the value.</param>
        /// <returns>The value of the given <see cref="Axis"/> from this <see cref="Vector3"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static float Get(this Vector3 _Vector3, Axis _Axis) => _Axis switch
        {
            Axis.X => _Vector3.x,
            Axis.Y => _Vector3.y,
            Axis.Z => _Vector3.z,
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };
        
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
            var _currentProjection = Vector3.Dot(_toCurrent, _direction);
            var _targetProjection = Vector3.Dot(_toTarget, _direction) * _PercentageOffset;
            
            // If the current projection exceeds the target projection, it has moved beyond.
            return _currentProjection >= _targetProjection;
        }
        
        /// <summary>
        /// Returns the maximum value among the x, y, and z components of the given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to evaluate.</param>
        /// <returns>The maximum value among the x, y, and z components.</returns>
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
        /// Subtracts the specified value from the components of a <see cref="Vector3"/> along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be subtracted from.</param>
        /// <param name="_Axis">The axis along which to subtract the component(s).</param>
        /// <param name="_Value">The value to subtract from the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the subtracted values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static Vector3 Minus(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x - _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y - _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z - _Value),
            Axis.XY => new Vector3(_Vector3.x - _Value, _Vector3.y - _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x - _Value, _Vector3.y, _Vector3.z - _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y - _Value, _Vector3.z - _Value),
            Axis.XYZ => new Vector3(_Vector3.x - _Value, _Vector3.y - _Value, _Vector3.z - _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Multiplies the components of a Vector3 by a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The Vector3 to be multiplied.</param>
        /// <param name="_Axis">The axis along which to multiply the component(s).</param>
        /// <param name="_Value">The value by which to multiply the component(s).</param>
        /// <returns>A new Vector3 with the multiplied values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static Vector3 Multiply(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x * _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y * _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z * _Value),
            Axis.XY => new Vector3(_Vector3.x * _Value, _Vector3.y * _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x * _Value, _Vector3.y, _Vector3.z * _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y * _Value, _Vector3.z * _Value),
            Axis.XYZ => new Vector3(_Vector3.x * _Value, _Vector3.y * _Value, _Vector3.z * _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Adds a specified value to the components of a <see cref="Vector3"/> along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to which the value will be added.</param>
        /// <param name="_Axis">The axis along which to add the value to the component(s).</param>
        /// <param name="_Value">The value to add to the component(s).</param>
        /// <returns>A new <see cref="Vector3"/> with the added values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static Vector3 Plus(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Vector3.x + _Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Vector3.y + _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Vector3.z + _Value),
            Axis.XY => new Vector3(_Vector3.x + _Value, _Vector3.y + _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Vector3.x + _Value, _Vector3.y, _Vector3.z + _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Vector3.y + _Value, _Vector3.z + _Value),
            Axis.XYZ => new Vector3(_Vector3.x + _Value, _Vector3.y + _Value, _Vector3.z + _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Sets the component(s) of a <see cref="Vector3"/> to a specified value along the given axis.
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector3"/> to be modified.</param>
        /// <param name="_Axis">The axis along which to set the component(s).</param>
        /// <param name="_Value">The value to set the component(s) to.</param>
        /// <returns>A new <see cref="Vector3"/> with the set values.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
        public static Vector3 Set(this Vector3 _Vector3, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector3(_Value, _Vector3.y, _Vector3.z),
            Axis.Y => new Vector3(_Vector3.x, _Value, _Vector3.z),
            Axis.Z => new Vector3(_Vector3.x, _Vector3.y, _Value),
            Axis.XY => new Vector3(_Value, _Value, _Vector3.z),
            Axis.XZ => new Vector3(_Value, _Vector3.y, _Value),
            Axis.YZ => new Vector3(_Vector3.x, _Value, _Value),
            Axis.XYZ => new Vector3(_Value, _Value, _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Creates a new <see cref="ArgumentOutOfRangeException"/> for the specified parameter and axis.
        /// </summary>
        /// <param name="_ParameterName">The name of the parameter that caused the exception.</param>
        /// <param name="_Axis">The axis enumeration value that is not allowed.</param>
        /// <returns>An instance of <see cref="ArgumentOutOfRangeException"/>.</returns>
        private static ArgumentOutOfRangeException ArgumentOutOfRangeException(string _ParameterName, Axis _Axis)
        {
            return new ArgumentOutOfRangeException(_ParameterName, _Axis, $"The value of [{nameof(_Axis)}]:{_Axis}, is not allowed.");
        }

        /// <summary>
        /// Sets the x component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <returns>A new <see cref="Vector3"/> with the modified x component.</returns>
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
        public static Vector3 WithXY(this Vector3 _Vector, float _X, float _Y)
        {
            _Vector.x = _X;
            _Vector.y = _Y;

            return _Vector;
        }

        /// <summary>
        /// Sets the x and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The Vector3 to be adjusted.</param>
        /// <param name="_X">The value to set or add to the x component.</param>
        /// <param name="_Z">The value to set or add to the z component.</param>
        /// <returns>A new Vector3 with the adjusted x and z components.</returns>
        public static Vector3 WithXZ(this Vector3 _Vector, float _X, float _Z)
        {
            _Vector.x = _X;
            _Vector.z = _Z;

            return _Vector;
        }

        /// <summary>
        /// Sets the y and z component of a <see cref="Vector3"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector3"/> to be modified.</param>
        /// <param name="_Y">The value to set (or add to) the Y component.</param>
        /// <param name="_Z">The value to set (or add to) the Z component.</param>
        /// <returns>The modified <see cref="Vector3"/>.</returns>
        public static Vector3 WithYZ(this Vector3 _Vector, float _Y, float _Z)
        {
            _Vector.y = _Y;
            _Vector.z = _Z;

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
        public static Vector3 WithXYZ(this Vector3 _Vector, float _X, float _Y, float _Z)
        {
            _Vector.x = _X;
            _Vector.y = _Y;
            _Vector.z = _Z;

            return _Vector;
        }
        #endregion
    }
}
