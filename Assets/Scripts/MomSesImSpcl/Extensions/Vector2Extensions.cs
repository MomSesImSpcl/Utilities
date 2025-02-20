using System;
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
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Gets a new <see cref="Vector2"/> with the values of this <see cref="Vector2"/> for the given <see cref="Axis"/>. <br/>
        /// <i>ALl other <see cref="Axis"/> values will be <c>0</c>.</i>
        /// </summary>
        /// <param name="_Vector3">The <see cref="Vector2"/> to get the value from.</param>
        /// <param name="_Axis">The <see cref="Axis"/> from which to get the value.</param>
        /// <returns>The values for the given <see cref="Axis"/> of this <see cref="Vector2"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When the given <see cref="Axis"/> is not handled by this method.</exception>
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
        public static Quaternion Get2DLookAtRotation(this Vector2 _Source, Vector2 _Target, float _OffsetDegrees = 0f, Quaternion? _DefaultRotation = null)
        {
            var _direction = _Target - _Source;

            if (_direction.sqrMagnitude < math.EPSILON)
            {
                return _DefaultRotation ?? Quaternion.identity;
            }

            var _angle = math.atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            
            return Quaternion.Euler(0f, 0f, _angle + _OffsetDegrees);
        }
        
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
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
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
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
        };

        /// <summary>
        /// Performs a mathematical <see cref="Utilities.Operation"/> on the given <see cref="Axis"/> of this <see cref="Vector3"/>.
        /// </summary>
        /// <param name="_Vector2">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_Operation">The mathematical operation to apply.</param>
        /// <param name="_Axis">The <see cref="Axis"/> on which to apply the <see cref="Utilities.Operation"/>.</param>
        /// <param name="_Value">The value to use for the <see cref="Utilities.Operation"/>.</param>
        /// <returns>A new <see cref="Vector2"/> computed with the specified <see cref="Utilities.Operation"/> applied to the given <see cref="Axis"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="Utilities.Operation"/> is not valid.</exception>
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
            _ => throw new ArgumentOutOfRangeException(nameof(_Axis), _Axis, null)
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
        /// Sets the x component of a <see cref="Vector2"/> to a specified value.
        /// </summary>
        /// <param name="_Vector">The <see cref="Vector2"/> to modify.</param>
        /// <param name="_X">The x-coordinate to set or add.</param>
        /// <returns>This <c>Vector2</c> with the modified x-coordinate.</returns>
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
        public static Vector2 WithXY(this Vector2 _Vector, float _Value)
        {
            _Vector.x = _Value;
            _Vector.y = _Value;

            return _Vector;
        }
        #endregion
    }
}
