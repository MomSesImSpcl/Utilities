using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Provides utility methods for performing various mathematical operations.
    /// </summary>
    public static class Math
    {
        #region Methods
        /// <summary>
        /// Tests if a floating-point value is approximately equal to another value within a specified range.
        /// </summary>
        /// <param name="_Value">The value to be compared.</param>
        /// <param name="_About">The target value to compare against.</param>
        /// <param name="_Range">The acceptable range within which the values are considered approximately equal.</param>
        /// <returns>True if the difference between the values is less than the specified range; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approx(float _Value, float _About, float _Range)
        {
            return Mathf.Abs(_Value - _About) < _Range;
        }

        /// <summary>
        /// Tests if a Vector3 value is approximately equal to another Vector3 value within a specified range.
        /// </summary>
        /// <param name="_Value">The Vector3 value to be compared.</param>
        /// <param name="_About">The target Vector3 value to compare against.</param>
        /// <param name="_Range">The acceptable range within which the Vector3 values are considered approximately equal.</param>
        /// <returns>True if the squared magnitude of the difference between the Vector3 values is less than the squared range; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public static bool Approx(Vector3 _Value, Vector3 _About, float _Range)
        {
            return (_Value - _About).sqrMagnitude < _Range * _Range;
        }

        /// <summary>
        /// Computes the ceiling of the absolute value of a floating-point number.
        /// </summary>
        /// <param name="_Value">The floating-point value whose absolute value ceiling is to be computed.</param>
        /// <returns>The ceiling of the absolute value of the specified floating-point value.</returns>
        public static float CeilAbsolute(float _Value)
        {
            var _absoluteValue = Mathf.Abs(_Value);
            var _ceiled = Mathf.Ceil(_absoluteValue);

            return _ceiled;
        }

        /// <summary>
        /// Returns a normalized value by "ceiling" the input to -1, 0, or 1 based on its sign.
        /// </summary>
        /// <param name="_Value">The floating-point value to be normalized.</param>
        /// <returns>1.0 if the value is positive, -1.0 if the value is negative, and 0.0 if the value is zero.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CeilNormal(float _Value)
        {
            if (_Value > 0)
                return 1.0f;
            if (_Value < 0)
                return -1.0f;
            return 0.0f;
        }

        /// <summary>
        /// Converts a value in decibels to a linear scale.
        /// </summary>
        /// <param name="_Decibel">The value in decibels to be converted.</param>
        /// <returns>The linear scale equivalent of the decibel value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DecibelToLinear(float _Decibel)
        {
            return _Decibel * Mathf.Pow(10.0f, _Decibel / 20.0f);
        }

        /// <summary>
        /// Compares two integers and returns the larger of the two.
        /// </summary>
        /// <param name="_Number1">The first integer to compare.</param>
        /// <param name="_Number2">The second integer to compare.</param>
        /// <returns>The larger of the two integers.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBigger(int _Number1, int _Number2) => _Number1 > _Number2 ? _Number1 : _Number2;

        /// <summary>
        /// Converts a linear value to its corresponding decibel value.
        /// </summary>
        /// <param name="_Linear">The linear value to be converted to decibels.</param>
        /// <returns>The decibel equivalent of the linear value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LinearToDecibel(float _Linear)
        {
            float _decibel;

            if (_Linear != 0)
                _decibel = 20.0f * Mathf.Log10(_Linear);
            else
                _decibel = -144.0f;

            return _decibel;
        }

        /// <summary>
        /// Calculates the nearest point on a line segment to a given point, clamping the result within the segment's endpoints.
        /// </summary>
        /// <param name="_LineStart">The starting point of the line segment.</param>
        /// <param name="_LineEnd">The ending point of the line segment.</param>
        /// <param name="_Point">The point to which the nearest point on the segment is to be found.</param>
        /// <returns>The nearest point on the line segment to the given point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NearestPointClamped(Vector3 _LineStart, Vector3 _LineEnd, Vector3 _Point)
        {
            var _fullDirection = _LineEnd - _LineStart;
            var _lineDirection = Vector3.Normalize(_fullDirection);
            var _closestPoint = Vector3.Dot(_Point - _LineStart, _lineDirection) / Vector3.Dot(_lineDirection, _lineDirection);
            return _LineStart + Mathf.Clamp(_closestPoint, 0.0f, Vector3.Magnitude(_fullDirection)) * _lineDirection;
        }

        /// <summary>
        /// Finds the nearest point on an infinite line to a given point.
        /// </summary>
        /// <param name="_LineStart">The start point of the line.</param>
        /// <param name="_LineEnd">The end point of the line.</param>
        /// <param name="_Point">The point for which the nearest point on the line is calculated.</param>
        /// <returns>The nearest point on the line to the given point.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NearestPointLine(Vector3 _LineStart, Vector3 _LineEnd, Vector3 _Point)
        {
            var _lineDirection = Vector3.Normalize(_LineEnd - _LineStart);
            var _closestPoint = Vector3.Dot(_Point - _LineStart, _lineDirection) / Vector3.Dot(_lineDirection, _lineDirection);
            return _LineStart + _closestPoint * _lineDirection;
        }

        /// <summary>
        /// Converts a signed angle to an equivalent unsigned angle in the range [0, 360).
        /// </summary>
        /// <param name="_Angle">The signed angle to be converted.</param>
        /// <returns>The equivalent unsigned angle.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedToUnsignedAngle(float _Angle)
        {
            _Angle %= 360;

            if (_Angle < 0)
            {
                return _Angle + 360;
            }

            return _Angle;
        }

        /// <summary>
        /// Converts an unsigned angle to its signed equivalent.
        /// </summary>
        /// <param name="_Angle">The unsigned angle to be converted.</param>
        /// <returns>The signed angle equivalent.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float UnsignedToSignedAngle(float _Angle)
        {
            _Angle %= 360;

            if (_Angle > 180)
            {
                return _Angle - 360;
            }

            return _Angle;
        }
        #endregion
    }
}