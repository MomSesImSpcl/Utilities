using System;
using IfLoooop.Utilities;
using UnityEngine;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Vector2"/>.
    /// </summary>
    public static class Vector2Extensions
    {
        #region Methods
        /// <summary>
        /// Divides <c>_Vector2</c> by <c>_Value</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to divide the value for.</param>
        /// <param name="_Axis">The axis on which to divide the value.</param>
        /// <param name="_Value">The value to divide from <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the divided value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        public static Vector2 Divide(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x / _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y / _Value),
            Axis.XY => new Vector2(_Vector2.x / _Value, _Vector2.y / _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Subtracts a value from <c>_Vector2</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to subtract the value from.</param>
        /// <param name="_Axis">The axis on which to subtract the value.</param>
        /// <param name="_Value">The value to subtract from <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the subtracted value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        public static Vector2 Minus(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x - _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y - _Value),
            Axis.XY => new Vector2(_Vector2.x - _Value, _Vector2.y - _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Multiplies <c>_Vector2</c> by <c>_Value</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to multiply the value for.</param>
        /// <param name="_Axis">The axis on which to multiply the value.</param>
        /// <param name="_Value">The value to multiply with <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the multiplied value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        public static Vector2 Multiply(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x * _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y * _Value),
            Axis.XY => new Vector2(_Vector2.x * _Value, _Vector2.y * _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Adds <c>_Value</c> to <c>_Vector2</c> on the given <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to add the value to.</param>
        /// <param name="_Axis">The axis on which to add the value.</param>
        /// <param name="_Value">The value to add to <c>_Vector2</c>.</param>
        /// <returns>This <c>_Vector2</c> with the added value/s.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        public static Vector2 Plus(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Vector2.x + _Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Vector2.y + _Value),
            Axis.XY => new Vector2(_Vector2.x + _Value, _Vector2.y + _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Sets the value of <c>_Vector2</c> on the specified <c>_Axis</c>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to set the value for.</param>
        /// <param name="_Axis">The axis on which to set the value.</param>
        /// <param name="_Value">The value to set for the specified <c>_Axis</c>.</param>
        /// <returns>A new <see cref="Vector2"/> with the set value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Axis</c> is any other value than: <see cref="Axis.X"/>, <see cref="Axis.Y"/> or <see cref="Axis.XY"/>.</exception>
        public static Vector2 Set(this Vector2 _Vector2, Axis _Axis, float _Value) => _Axis switch
        {
            Axis.X => new Vector2(_Value, _Vector2.y),
            Axis.Y => new Vector2(_Vector2.x, _Value),
            Axis.XY => new Vector2(_Value, _Value),
            _ => throw ArgumentOutOfRangeException(nameof(_Axis), _Axis)
        };

        /// <summary>
        /// Creates an <see cref="ArgumentOutOfRangeException"/> for the specified parameter and axis.
        /// </summary>
        /// <param name="_ParameterName">The name of the parameter that caused the exception.</param>
        /// <param name="_Axis">The <see cref="Axis"/> value that caused the exception.</param>
        /// <returns>An <see cref="ArgumentOutOfRangeException"/> with a detailed message.</returns>
        private static ArgumentOutOfRangeException ArgumentOutOfRangeException(string _ParameterName, Axis _Axis)
        {
            return new ArgumentOutOfRangeException(_ParameterName, _Axis, $"The value of [{nameof(_Axis)}]:{_Axis}, is not allowed.");
        }

        /// <summary>
        /// Sets or adds the x-coordinate of the given <c>Vector2</c> based on the <c>_Add</c> flag.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_X">The x-coordinate to set or add.</param>
        /// <param name="_Add">If <c>true</c>, the method adds <c>_X</c> to the current x-coordinate; otherwise, it sets the x-coordinate to <c>_X</c>.</param>
        /// <returns>This <c>Vector2</c> with the modified x-coordinate.</returns>
        public static Vector2 WithX(this Vector2 _Vector, float _X, bool _Add = false)
        {
            _Vector.x = _Add == false ? _X : _Vector.x + _X;
            return _Vector;
        }

        /// <summary>
        /// Sets the y-component of <c>_Vector</c> to <c>_Y</c>. If <c>_Add</c> is true, adds <c>_Y</c> to the current y-component.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector2"/> to modify the y-component for.</param>
        /// <param name="_Y">The value to set or add to the y-component.</param>
        /// <param name="_Add">If true, adds <c>_Y</c> to the current y-component; otherwise, sets the y-component to <c>_Y</c>.</param>
        /// <returns>This <c>_Vector</c> with the modified y-component.</returns>
        public static Vector2 WithY(this Vector2 _Vector, float _Y, bool _Add = false)
        {
            _Vector.y = _Add == false ? _Y : _Vector.y + _Y;
            return _Vector;
        }

        /// <summary>
        /// Returns a new <see cref="Vector2"/> with the specified <c>X</c> and <c>Y</c> values modified.
        /// </summary>
        /// <param name="_Vector">The original <see cref="Vector2"/> to modify.</param>
        /// <param name="_X">The new value for the <c>X</c> component.</param>
        /// <param name="_Y">The new value for the <c>Y</c> component.</param>
        /// <param name="_Add">If <c>true</c>, adds the <c>X</c> and <c>Y</c> values to the current components; otherwise, sets the components to the new values.</param>
        /// <returns>A new <see cref="Vector2"/> with the modified <c>X</c> and <c>Y</c> values.</returns>
        public static Vector2 WithXY(this Vector2 _Vector, float _X, float _Y, bool _Add = false)
        {
            _Vector.x = _Add == false ? _X : _Vector.x + _X;
            _Vector.y = _Add == false ? _Y : _Vector.y + _Y;
            return _Vector;
        }
        #endregion
    }
}