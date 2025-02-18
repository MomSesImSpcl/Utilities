using MomSesImSpcl.Utilities;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Transform"/>.
    /// </summary>
    public static class TransformExtensions
    {
        #region Methods
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Axis">The axis on which to apply the offset.</param>
        /// <param name="_Value">The distance to move in the specified direction.</param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Axis _Axis, float _Value, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Axis, _Value);
            return _Transform.Local(_localOffset, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Values">
        /// The distance to move in the specified axis. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Vector3 _Values, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Values);
            return _Transform.Local(_localOffset, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_LocalPosition">
        /// The distance to move in the specified axis. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        private static Vector3 Local(this Transform _Transform, Vector3 _LocalPosition, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _position = _Transform.TransformPoint(_LocalPosition);
            
#if UNITY_EDITOR
            if (_Visualize)
            {
                Draw.Sphere(_position, _Radius, Color.red, _Duration);
            }
#endif
            return _position;
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Axis">The axis on which to apply the offset.</param>
        /// <param name="_Value">The distance to move in the specified direction.</param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Axis _Axis, float _Value, Vector3 _Angles, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Axis, _Value);
            return _Transform.Local(_localOffset, _Angles, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Values">
        /// The distance to move in the specified axis. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Vector3 _Values, Vector3 _Angles, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Values);
            return _Transform.Local(_localOffset, _Angles, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_LocalPosition">The <see cref="Transform.localPosition"/> to calculate into world space.</param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i>
        /// </param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        private static Vector3 Local(this Transform _Transform, Vector3 _LocalPosition, Vector3 _Angles, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _scaledOffset = Vector3.Scale(_LocalPosition, _Transform.localScale);
            var _angleOffset = _Angles - _Transform.localEulerAngles;
            var _position = _Transform.position + Quaternion.Euler(_angleOffset) * _scaledOffset;
            
#if UNITY_EDITOR
            if (_Visualize)
            {
                Draw.Sphere(_position, _Radius, Color.red, _Duration);
            }
#endif
            return _position;
        }
        #endregion
    }
}
