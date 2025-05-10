using System;
using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Quaternion"/>.
    /// </summary>
    public static class QuaternionExtensions
    {
        #region Methods
        /// <summary>
        /// Divides the specified axis of the given <c>_Quaternion</c> by <c>_Value</c>.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to be divided.</param>
        /// <param name="_Axis">The axis on which to divide the value.</param>
        /// <param name="_Value">The value by which to divide the axis on <c>_Quaternion</c>.</param>
        /// <returns>This <c>_Quaternion</c> with the divided value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value.</exception>
        public static Quaternion Divide(this Quaternion _Quaternion, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => Quaternion.Euler(_Quaternion.eulerAngles.x / _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z),
            Axis.Y => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y / _Value, _Quaternion.eulerAngles.z),
            Axis.Z => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z / _Value),
            Axis.XY => Quaternion.Euler(_Quaternion.eulerAngles.x / _Value, _Quaternion.eulerAngles.y / _Value, _Quaternion.eulerAngles.z),
            Axis.XZ => Quaternion.Euler(_Quaternion.eulerAngles.x / _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z / _Value),
            Axis.YZ => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y / _Value, _Quaternion.eulerAngles.z / _Value),
            Axis.XYZ => Quaternion.Euler(_Quaternion.eulerAngles.x / _Value, _Quaternion.eulerAngles.y / _Value, _Quaternion.eulerAngles.z / _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Subtracts the specified value from the given axis of the <c>_Quaternion</c>.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_Axis">The axis from which to subtract the value.</param>
        /// <param name="_Value">The value to subtract from the specified axis on the <c>_Quaternion</c>.</param>
        /// <returns>This <c>_Quaternion</c> with the modified value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value.</exception>
        public static Quaternion Minus(this Quaternion _Quaternion, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => Quaternion.Euler(_Quaternion.eulerAngles.x - _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z),
            Axis.Y => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y - _Value, _Quaternion.eulerAngles.z),
            Axis.Z => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z - _Value),
            Axis.XY => Quaternion.Euler(_Quaternion.eulerAngles.x - _Value, _Quaternion.eulerAngles.y - _Value, _Quaternion.eulerAngles.z),
            Axis.XZ => Quaternion.Euler(_Quaternion.eulerAngles.x - _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z - _Value),
            Axis.YZ => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y - _Value, _Quaternion.eulerAngles.z - _Value),
            Axis.XYZ => Quaternion.Euler(_Quaternion.eulerAngles.x - _Value, _Quaternion.eulerAngles.y - _Value, _Quaternion.eulerAngles.z - _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Multiplies the specified axis of the given <c>_Quaternion</c> by <c>_Value</c>.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to be multiplied.</param>
        /// <param name="_Axis">The axis on which to multiply the value.</param>
        /// <param name="_Value">The value by which to multiply the axis on <c>_Quaternion</c>.</param>
        /// <returns>This <c>_Quaternion</c> with the multiplied value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value.</exception>
        public static Quaternion Multiply(this Quaternion _Quaternion, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => Quaternion.Euler(_Quaternion.eulerAngles.x * _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z),
            Axis.Y => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y * _Value, _Quaternion.eulerAngles.z),
            Axis.Z => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z * _Value),
            Axis.XY => Quaternion.Euler(_Quaternion.eulerAngles.x * _Value, _Quaternion.eulerAngles.y * _Value, _Quaternion.eulerAngles.z),
            Axis.XZ => Quaternion.Euler(_Quaternion.eulerAngles.x * _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z * _Value),
            Axis.YZ => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y * _Value, _Quaternion.eulerAngles.z * _Value),
            Axis.XYZ => Quaternion.Euler(_Quaternion.eulerAngles.x * _Value, _Quaternion.eulerAngles.y * _Value, _Quaternion.eulerAngles.z * _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Adds the specified value to the given axis of the <c>_Quaternion</c>.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to be modified.</param>
        /// <param name="_Axis">The axis on which to add the value.</param>
        /// <param name="_Value">The value to add to the specified axis of <c>_Quaternion</c>.</param>
        /// <returns>This <c>_Quaternion</c> with the added value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value.</exception>
        public static Quaternion Plus(this Quaternion _Quaternion, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => Quaternion.Euler(_Quaternion.eulerAngles.x + _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z),
            Axis.Y => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y + _Value, _Quaternion.eulerAngles.z),
            Axis.Z => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z + _Value),
            Axis.XY => Quaternion.Euler(_Quaternion.eulerAngles.x + _Value, _Quaternion.eulerAngles.y + _Value, _Quaternion.eulerAngles.z),
            Axis.XZ => Quaternion.Euler(_Quaternion.eulerAngles.x + _Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z + _Value),
            Axis.YZ => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y + _Value, _Quaternion.eulerAngles.z + _Value),
            Axis.XYZ => Quaternion.Euler(_Quaternion.eulerAngles.x + _Value, _Quaternion.eulerAngles.y + _Value, _Quaternion.eulerAngles.z + _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Sets the specified axis of the given <c>Quaternion</c> to the provided value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to be modified.</param>
        /// <param name="_Axis">The axis on which to set the value.</param>
        /// <param name="_Value">The value to set on the specified axis of <c>_Quaternion</c>.</param>
        /// <returns>This <c>_Quaternion</c> with the modified value(s).</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value.</exception>
        public static Quaternion Set(this Quaternion _Quaternion, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => Quaternion.Euler(_Value, _Quaternion.eulerAngles.y, _Quaternion.eulerAngles.z),
            Axis.Y => Quaternion.Euler(_Quaternion.eulerAngles.x, _Value, _Quaternion.eulerAngles.z),
            Axis.Z => Quaternion.Euler(_Quaternion.eulerAngles.x, _Quaternion.eulerAngles.y, _Value),
            Axis.XY => Quaternion.Euler(_Value, _Value, _Quaternion.eulerAngles.z),
            Axis.XZ => Quaternion.Euler(_Value, _Quaternion.eulerAngles.y, _Value),
            Axis.YZ => Quaternion.Euler(_Quaternion.eulerAngles.x, _Value, _Value),
            Axis.XYZ => Quaternion.Euler(_Value, _Value, _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Creates a new instance of the <see cref="ArgumentOutOfRangeException"/> class with a specified error message.
        /// </summary>
        /// <param name="_ParameterName">The name of the parameter that caused the exception.</param>
        /// <param name="_Axis">The axis value that caused the exception.</param>
        /// <returns>A new instance of the <see cref="ArgumentOutOfRangeException"/> class.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> has an invalid value not defined in the enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ArgumentOutOfRangeException ArgumentOutOfRangeException(string _ParameterName, Axis _Axis)
        {
            return new ArgumentOutOfRangeException(_ParameterName, _Axis, $"The value of [{nameof(_Axis)}]:{_Axis}, is not allowed.");
        }
        
        /// <summary>
        /// Sets the x component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified x component.</returns>
        public static Quaternion WithX(this Quaternion _Quaternion, float _X)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithX(_X);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the y component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified y component.</returns>
        public static Quaternion WithY(this Quaternion _Quaternion, float _Y)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithY(_Y);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the z component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_Z">The new value for the z component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified z component.</returns>
        public static Quaternion WithZ(this Quaternion _Quaternion, float _Z)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithY(_Z);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the x and y component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified x and y component.</returns>
        public static Quaternion WithXY(this Quaternion _Quaternion, float _X, float _Y)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithX(_X);
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithY(_Y);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the x and z component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <param name="_Z">The new value for the z component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified x and z component.</returns>
        public static Quaternion WithXZ(this Quaternion _Quaternion, float _X, float _Z)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithX(_X);
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithZ(_Z);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the y and z component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <param name="_Z">The new value for the z component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified y and z component.</returns>
        public static Quaternion WithYZ(this Quaternion _Quaternion, float _Y, float _Z)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithY(_Y);
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithZ(_Z);
            
            return _Quaternion;
        }

        /// <summary>
        /// Sets the x, y and z component of a <see cref="Quaternion"/> to a specified value.
        /// </summary>
        /// <param name="_Quaternion">The <see cref="Quaternion"/> to modify.</param>
        /// <param name="_X">The new value for the x component.</param>
        /// <param name="_Y">The new value for the y component.</param>
        /// <param name="_Z">The new value for the z component.</param>
        /// <returns>A new <see cref="Quaternion"/> with the modified x, y and z component.</returns>
        public static Quaternion WithXYZ(this Quaternion _Quaternion, float _X, float _Y, float _Z)
        {
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithX(_X);
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithY(_Y);
            _Quaternion.eulerAngles = _Quaternion.eulerAngles.WithZ(_Z);
            
            return _Quaternion;
        }
        #endregion
    }
}
