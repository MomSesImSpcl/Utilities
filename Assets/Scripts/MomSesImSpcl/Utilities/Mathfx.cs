using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// A static class that provides a variety of mathematical functions, including interpolation, bouncing, damping, and more.
    /// </summary>
    public static class MathFx
    {
        /// <summary>
        /// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Targeted vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 BErp(Vector2 _Start, Vector2 _End, float _InterpolationValue)
        {
            return new Vector2(BErp(_Start.x, _End.x, _InterpolationValue), BErp(_Start.y, _End.y, _InterpolationValue));
        }

        /// <summary>
        /// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Targeted vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated vector value between <paramref name="_Start" /> and <paramref name="_End" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 BErp(Vector3 _Start, Vector3 _End, float _InterpolationValue)
        {
            return new Vector3(BErp(_Start.x, _End.x, _InterpolationValue), BErp(_Start.y, _End.y, _InterpolationValue), BErp(_Start.z, _End.z, _InterpolationValue));
        }

        /// <summary>
        /// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        /// <param name="_Start">Starting float value.</param>
        /// <param name="_End">Targeted float value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float BErp(float _Start, float _End, float _InterpolationValue)
        {
            _InterpolationValue = Mathf.Clamp01(_InterpolationValue);
            _InterpolationValue = (math.sin(_InterpolationValue * math.PI * (0.2f + 2.5f * _InterpolationValue * _InterpolationValue * _InterpolationValue)) * math.pow(1f - _InterpolationValue, 2.2f) + _InterpolationValue) * (1f + 1.2f * (1f - _InterpolationValue));
            return _Start + (_End - _Start) * _InterpolationValue;
        }

        /// <summary>
        /// Applies a bouncing effect to each component of the given vector.
        /// </summary>
        /// <param name="_Vector">The vector to which the bounce effect will be applied.</param>
        /// <returns>A new vector with the bounce effect applied to each component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Bounce(Vector2 _Vector)
        {
            return new Vector2(Bounce(_Vector.x), Bounce(_Vector.y));
        }

        /// <summary>
        /// Applies a bouncing effect to a given vector by modifying each of its components.
        /// </summary>
        /// <param name="_Vector">The vector to which the bouncing effect will be applied.</param>
        /// <returns>A new vector with the bouncing effect applied to each of its components.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Bounce(Vector3 _Vector)
        {
            return new Vector3(Bounce(_Vector.x), Bounce(_Vector.y), Bounce(_Vector.z));
        }

        /// <summary>
        /// Applies a bounce effect to the given value, creating a bouncing motion.
        /// </summary>
        /// <param name="_Value">The input value to apply the bounce effect to.</param>
        /// <returns>The bounced value of <paramref name="_Value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Bounce(float _Value)
        {
            return math.abs(math.sin(6.28f * (_Value + 1f) * (_Value + 1f)) * (1f - _Value));
        }

        /// <summary>
        /// Interpolates between angles specified in degrees, ensuring the shortest path is taken around the circular range from 0 to 360 degrees.
        /// </summary>
        /// <param name="_Start">Starting angle in degrees.</param>
        /// <param name="_End">Target angle in degrees.</param>
        /// <param name="_InterpolationValue">Interpolation factor, typically between 0 and 1.</param>
        /// <returns>The interpolated angle in degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CLerp(float _Start, float _End, float _InterpolationValue)
        {
            const float _MIN = 0.0f;
            const float _MAX = 360.0f;
            var _half = math.abs((_MAX - _MIN) / 2.0f);
            float _returnValue;
            float _difference;

            if (_End - _Start < -_half)
            {
                _difference = (_MAX - _Start + _End) * _InterpolationValue;
                _returnValue = _Start + _difference;
            }
            else if (_End - _Start > _half)
            {
                _difference = -(_MAX - _End + _Start) * _InterpolationValue;
                _returnValue = _Start + _difference;
            }
            else _returnValue = _Start + (_End - _Start) * _InterpolationValue;
            
            return _returnValue;
        }

        /// <summary>
        /// Performs cosine interpolation between two Vector2 values.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Targeted vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 CosErp(Vector2 _Start, Vector2 _End, float _InterpolationValue)
        {
            return new Vector2(CosErp(_Start.x, _End.x, _InterpolationValue), CosErp(_Start.y, _End.y, _InterpolationValue));
        }

        /// <summary>
        /// Performs cosine interpolation between the start and end vector values.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Ending vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor, typically between 0 (start) and 1 (end).</param>
        /// <returns>The interpolated vector value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 CosErp(Vector3 _Start, Vector3 _End, float _InterpolationValue)
        {
            return new Vector3(CosErp(_Start.x, _End.x, _InterpolationValue), CosErp(_Start.y, _End.y, _InterpolationValue), CosErp(_Start.z, _End.z, _InterpolationValue));
        }

        /// <summary>
        /// Applies a cosine interpolation to the provided float values, creating a smooth transition.
        /// </summary>
        /// <param name="_Start">Starting float value.</param>
        /// <param name="_End">Targeted float value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CosErp(float _Start, float _End, float _InterpolationValue)
        {
            return math.lerp(_Start, _End, 1.0f - math.cos(_InterpolationValue * math.PI * 0.5f));
        }

        /// <summary>
        /// Smoothly interpolates a float value from the start towards the end using exponential smoothing.
        /// </summary>
        /// <param name="_Start">The starting float value.</param>
        /// <param name="_End">The target float value.</param>
        /// <param name="_Smoothing">The smoothing factor where higher values result in slower transitions.</param>
        /// <param name="_Speed">The speed factor influencing the rate of change.</param>
        /// <param name="_SnapEpsilon">The threshold at which the interpolated value snaps to the end value.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Damp(float _Start, float _End, float _Smoothing, float _Speed, float _SnapEpsilon = 0.01f)
        {
            var _value = math.lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
            if (math.abs(_value - _End) < _SnapEpsilon)
                _value = _End;

            return _value;
        }

        /// <summary>
        /// Smoothly interpolates between two Vector2 values, approaching the target more quickly at first and then slowing down.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Targeted vector value.</param>
        /// <param name="_Smoothing">Factor determining the interpolation smoothness.</param>
        /// <param name="_Speed">Speed at which to reach the target value.</param>
        /// <returns>The interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Damp(Vector2 _Start, Vector2 _End, float _Smoothing, float _Speed)
        {
            return math.lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
        }

        /// <summary>
        /// Performs a damped interpolation between two vector values.
        /// </summary>
        /// <param name="_Start">The starting vector value.</param>
        /// <param name="_End">The target vector value.</param>
        /// <param name="_Smoothing">The smoothing factor for the interpolation.</param>
        /// <param name="_Speed">The speed at which the interpolation should occur.</param>
        /// <returns>The damped interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Damp(Vector3 _Start, Vector3 _End, float _Smoothing, float _Speed)
        {
            return math.lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
        }

        /// <summary>
        /// Smoothly transitions a float value over time, accounting for smoothing and speed factors.
        /// </summary>
        /// <param name="_Start">Starting float value.</param>
        /// <param name="_End">Targeted float value.</param>
        /// <param name="_Smoothing">Smoothing factor applied to the interpolation.</param>
        /// <param name="_Speed">Speed at which the interpolation occurs.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>, adjusted by the smoothing and speed factors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Damp(Vector4 _Start, Vector4 _End, float _Smoothing, float _Speed)
        {
            return math.lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
        }

        /// <summary>
        /// Interpolates between a start and end value using a damping function, useful for smooth, asymptotic transitions.
        /// </summary>
        /// <param name="_Start">The initial value of the interpolation.</param>
        /// <param name="_End">The target value of the interpolation.</param>
        /// <param name="_Smoothing">The smoothing factor that determines the curvature of the interpolation.</param>
        /// <param name="_Speed">The speed at which the interpolation progresses.</param>
        /// <returns>The damped interpolation value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Damp(Quaternion _Start, Quaternion _End, float _Smoothing, float _Speed)
        {
            return Quaternion.Lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
        }

        /// <summary>
        /// Smoothly interpolates between the starting value and the end value based on the specified smoothing and speed parameters.
        /// </summary>
        /// <param name="_Start">Starting float value.</param>
        /// <param name="_End">Targeted float value.</param>
        /// <param name="_Smoothing">Smoothing factor, controls how smoothly the interpolation occurs.</param>
        /// <param name="_Speed">Speed of the interpolation process.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Damp(Color _Start, Color _End, float _Smoothing, float _Speed)
        {
            return Color.Lerp(_Start, _End, 1 - math.pow(_Smoothing, _Speed));
        }

        /// <summary>
        /// Computes a Hermite spline interpolation between two vectors.
        /// </summary>
        /// <param name="_Start">The starting vector value.</param>
        /// <param name="_End">The ending vector value.</param>
        /// <param name="_InterpolationValue">The interpolation factor, typically between 0 and 1.</param>
        /// <returns>The interpolated vector value between <paramref name="_Start"/> and <paramref name="_End"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Hermite(Vector2 _Start, Vector2 _End, float _InterpolationValue)
        {
            return new Vector2(Hermite(_Start.x, _End.x, _InterpolationValue), Hermite(_Start.y, _End.y, _InterpolationValue));
        }

        /// <summary>
        /// Performs Hermite interpolation between two points, resulting in a smooth
        /// curve that passes through the start and end points with zero derivatives at both.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Ending vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Hermite(Vector3 _Start, Vector3 _End, float _InterpolationValue)
        {
            return new Vector3(Hermite(_Start.x, _End.x, _InterpolationValue), Hermite(_Start.y, _End.y, _InterpolationValue), Hermite(_Start.z, _End.z, _InterpolationValue));
        }

        /// <summary>
        /// Performs Hermite interpolation between the start and end values, providing a smooth transition with ease-in and ease-out characteristics.
        /// </summary>
        /// <param name="_Start">Starting float value.</param>
        /// <param name="_End">Targeted float value.</param>
        /// <param name="_InterpolationValue">Interpolation factor in the range [0, 1].</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Hermite(float _Start, float _End, float _InterpolationValue)
        {
            return math.lerp(_Start, _End, _InterpolationValue * _InterpolationValue * (3.0f - 2.0f * _InterpolationValue));
        }

        /// <summary>
        /// Linearly interpolates between two float values.
        /// </summary>
        /// <param name="_Start">The starting float value.</param>
        /// <param name="_End">The ending float value.</param>
        /// <param name="_InterpolationValue">The interpolation value between 0 and 1.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float _Start, float _End, float _InterpolationValue)
        {
            return (1.0f - _InterpolationValue) * _Start + _InterpolationValue * _End;
        }

        /// <summary>
        /// Applies a sine-based interpolation, easing the transition from the start value to the end value using a sine function.
        /// </summary>
        /// <param name="_Start">The starting float value.</param>
        /// <param name="_End">The ending float value.</param>
        /// <param name="_InterpolationValue">The interpolation factor, typically between 0 and 1.</param>
        /// <returns>The interpolated float value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SinErp(float _Start, float _End, float _InterpolationValue)
        {
            return math.lerp(_Start, _End, math.sin(_InterpolationValue * math.PI * 0.5f));
        }

        /// <summary>
        /// Performs a sine-interpolated transition from a starting vector value to a targeted vector value.
        /// </summary>
        /// <param name="_Start">Starting vector value.</param>
        /// <param name="_End">Targeted vector value.</param>
        /// <param name="_InterpolationValue">Interpolation factor.</param>
        /// <returns>The sine-interpolated vector value between <c>_Start</c> and <c>_End</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SinErp(Vector2 _Start, Vector2 _End, float _InterpolationValue)
        {
            return new Vector2(math.lerp(_Start.x, _End.x, math.sin(_InterpolationValue * math.PI * 0.5f)), math.lerp(_Start.y, _End.y, math.sin(_InterpolationValue * math.PI * 0.5f)));
        }

        /// <summary>
        /// Applies a sine-based interpolation between two Vector3 values to achieve easing motion.
        /// </summary>
        /// <param name="_Start">The starting vector value.</param>
        /// <param name="_End">The targeted vector value.</param>
        /// <param name="_InterpolationValue">The interpolation factor, typically between 0 and 1.</param>
        /// <returns>The interpolated vector value between <c>_Start</c> and <c>_End</c> based on a sine function.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SinErp(Vector3 _Start, Vector3 _End, float _InterpolationValue)
        {
            return new Vector3(math.lerp(_Start.x, _End.x, math.sin(_InterpolationValue * math.PI * 0.5f)), math.lerp(_Start.y, _End.y, math.sin(_InterpolationValue * math.PI * 0.5f)), math.lerp(_Start.z, _End.z, math.sin(_InterpolationValue * math.PI * 0.5f)));
        }

        /// <summary>
        /// Provides smooth sinusoidal interpolation between two values, giving the effect of a smooth transition that follows a sine wave.
        /// </summary>
        /// <param name="_Start">The starting value of the interpolation.</param>
        /// <param name="_End">The ending value of the interpolation.</param>
        /// <param name="_InterpolationValue">The interpolation factor, typically between 0 and 1.</param>
        /// <returns>The interpolated value between <c>_Start</c> and <c>_End</c> following a smooth sine wave pattern.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SmoothSin(float _Start, float _End, float _InterpolationValue)
        {
            return math.lerp(_Start, _End, math.sin(_InterpolationValue * 2 * math.PI + math.PI * 0.5f));
        }

        /// <summary>
        /// Interpolates smoothly between 0 and 1 based on the input value, _Value, clamped between _Min and _Max.
        /// </summary>
        /// <param name="_Vector">The input vector to interpolate.</param>
        /// <param name="_Min">The minimum value of the input range.</param>
        /// <param name="_Max">The maximum value of the input range.</param>
        /// <returns>A smoothly interpolated value between 0 and 1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SmoothStep(Vector2 _Vector, float _Min, float _Max)
        {
            return new Vector2(SmoothStep(_Vector.x, _Min, _Max), SmoothStep(_Vector.y, _Min, _Max));
        }

        /// <summary>
        /// Interpolates each component of a vector smoothly between 0 and 1 over the specified range using a cubic equation.
        /// </summary>
        /// <param name="_Vector">The vector to be interpolated.</param>
        /// <param name="_Min">The lower edge of the range.</param>
        /// <param name="_Max">The upper edge of the range.</param>
        /// <returns>A vector with each component smoothly interpolated between 0 and 1 based on the specified range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SmoothStep(Vector3 _Vector, float _Min, float _Max)
        {
            return new Vector3(SmoothStep(_Vector.x, _Min, _Max), SmoothStep(_Vector.y, _Min, _Max), SmoothStep(_Vector.z, _Min, _Max));
        }

        /// <summary>
        /// Interpolates the input value smoothly between a specified range using a Hermite polynomial.
        /// </summary>
        /// <param name="_Value">The input value to be interpolated.</param>
        /// <param name="_Min">The minimum bound of the range.</param>
        /// <param name="_Max">The maximum bound of the range.</param>
        /// <returns>The interpolated value between the specified minimum and maximum bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SmoothStep(float _Value, float _Min, float _Max)
        {
            _Value = math.clamp(_Value, _Min, _Max);
            var _v1 = (_Value - _Min) / (_Max - _Min);
            var _v2 = (_Value - _Min) / (_Max - _Min);
            return -2 * _v1 * _v1 * _v1 + 3 * _v2 * _v2;
        }
    }
}
