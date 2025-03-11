#nullable enable
using System;
using System.Collections.Generic;
using MomSesImSpcl.Utilities;
using Unity.Mathematics;
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
        /// Returns the <see cref="Transform.eulerAngles"/> as a <see cref="float3"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.eulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.eulerAngles"/> as a <see cref="float3"/></returns>
        public static float3 EulerAngles(this Transform _Transform)
        {
            return new float3(_Transform.eulerAngles.x, _Transform.eulerAngles.y, _Transform.eulerAngles.z);
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.localEulerAngles"/> as a <see cref="float3"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localEulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.localEulerAngles"/> as a <see cref="float3"/></returns>
        public static float3 LocalEulerAngles(this Transform _Transform)
        {
            return new float3(_Transform.localEulerAngles.x, _Transform.localEulerAngles.y, _Transform.localEulerAngles.z);
        }
        
        /// <summary>
        /// Searches for the given <see cref="Component"/> in the children of this <see cref="Transform"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="Transform"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/> of the given <see cref="Type"/>, or <c>null</c> if it couldn't be found.</returns>
        public static T? GetComponentInChildren<T>(this Transform _Transform, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            return _Transform.gameObject.GetComponentInChildren<T>(_IncludeInactive, _ExcludeSelf);
        }
        
        /// <summary>
        /// Searches for the given <see cref="Component"/>s in the children of this <see cref="Transform"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to search under.</param>
        /// <param name="_IncludeInactive">Whether to include inactive children in the search.</param>
        /// <param name="_ExcludeSelf">Set to <c>true</c> to only search for the <see cref="Component"/> in the children and not on this <see cref="Transform"/>.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="Component"/>.</typeparam>
        /// <returns>The <see cref="Component"/>s of the given <see cref="Type"/>, or <c>null</c> if none could be found.</returns>
        public static IEnumerable<T> GetComponentsInChildren<T>(this Transform _Transform, bool _IncludeInactive, bool _ExcludeSelf) where T : Component
        {
            return _Transform.gameObject.GetComponentsInChildren<T>(_IncludeInactive, _ExcludeSelf);
        }
        
        /// <summary>
        /// Gets the distance from this <see cref="Transform"/> to a point at <c>_TargetHeight</c> in the <see cref="Transform"/>s <see cref="Transform.forward"/> direction.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the distance from.</param>
        /// <param name="_TargetHeight">The <see cref="Vector3.y"/> coordinate of the point to get the distance to.</param>
        /// <returns>The distance from this <see cref="Transform"/> to a point at <c>_TargetHeight</c> in the <see cref="Transform"/>s <see cref="Transform.forward"/> direction.</returns>
        public static float GetDistanceToHeight(this Transform _Transform, float _TargetHeight)
        {
            var _heightDifference = _TargetHeight - _Transform.position.y;
            var _forwardY = _Transform.forward.y;
            
            if (Mathf.Approximately(_forwardY, 0f))
            {
                return float.PositiveInfinity;
            }

            return _heightDifference / _forwardY;
        }

        /// <summary>
        /// Get a point in <see cref="Direction"/> relative to this <see cref="Transform"/>s <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to calculate the point from.</param>
        /// <param name="_Distance">The distance the target point should have to this <see cref="Transform"/>.</param>
        /// <param name="_Direction">The direction of the target point relative to this <see cref="Transform"/>.</param>
        /// <returns>A point in <see cref="Direction"/> relative to this <see cref="Transform"/>s <see cref="Transform.position"/>.</returns>
        public static Vector3 GetPointAtDistance(this Transform _Transform, float _Distance, Direction _Direction = Direction.Forward)
        {
#pragma warning disable CS8524
            return _Transform.position + _Direction switch
#pragma warning restore CS8524
            {
                Direction.Up => _Transform.up,
                Direction.Down => -_Transform.up,
                Direction.Left => -_Transform.right,
                Direction.Right => _Transform.right,
                Direction.Forward => _Transform.forward,
                Direction.Back => -_Transform.forward
            } * _Distance;
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Axis">The axis on which to apply the offset.</param>
        /// <param name="_Offset">The distance to move in the specified direction.</param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Axis _Axis, float _Offset, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Axis, _Offset);
            return _Transform.Local(_localOffset, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Offset">
        /// The distance to move in the specified axis. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Vector3 _Offset, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Offset);
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
        /// <param name="_Offset">The distance to move in the specified direction.</param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i> <br/>
        /// <i>Values for not needed axes can be left at <c>0</c>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Axis _Axis, float _Offset, Vector3 _Angles, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Axis, _Offset);
            return _Transform.Local(_localOffset, _Angles, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Offset">
        /// The distance to move in the specified axis. <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Add"/> and <see cref="Utilities.Operation.Subtract"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>0</c>.</i> <br/>
        /// <i>For <see cref="Utilities.Operation"/> <see cref="Utilities.Operation.Multiply"/> and <see cref="Utilities.Operation.Divide"/> set the values inside the <see cref="Vector3"/> you don't want to change to <c>1</c>.</i>
        /// </param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i> <br/>
        /// <i>Values for not needed axes can be left at <c>0</c>.</i>
        /// </param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <param name="_Visualize">Set to <c>true</c> to draw a sphere at the computed <see cref="Transform.position"/>.</param>
        /// <param name="_Duration">Duration in seconds how long the sphere will be visible.</param>
        /// <param name="_Radius">The radius of the sphere.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Vector3 _Offset, Vector3 _Angles, Operation _Operation = Operation.Add, bool _Visualize = false, float _Duration = 1f, float _Radius = 1f)
        {
            var _localOffset = Vector3.zero.Operation(_Operation, _Offset);
            return _Transform.Local(_localOffset, _Angles, _Visualize, _Duration, _Radius);
        }
        
        /// <summary>
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> on the specified <see cref="Axis"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_LocalPosition">The <see cref="Transform.localPosition"/> to calculate into world space.</param>
        /// <param name="_Angles">
        /// The desired angles to calculate the <see cref="Transform.position"/> with. <br/>
        /// <i>Useful when the <see cref="Transform"/> is rotated, but the <see cref="Transform.position"/> should be calculated with no or a specific <see cref="Transform.rotation"/>.</i> <br/>
        /// <i>Values for not needed axes can be left at <c>0</c>.</i>
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

        /// <summary>
        /// Returns the <see cref="Transform.localPosition"/> as a <see cref="float2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localPosition"/> from.</param>
        /// <returns>The <see cref="Transform.localPosition"/> as a <see cref="float2"/></returns>
        public static float2 LocalPosition2(this Transform _Transform)
        {
            return new float2(_Transform.localPosition.x, _Transform.localPosition.y);
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.position"/> as a <see cref="float2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.position"/> from.</param>
        /// <returns>The <see cref="Transform.position"/> as a <see cref="float2"/></returns>
        public static float2 Position2(this Transform _Transform)
        {
            return new float2(_Transform.position.x, _Transform.position.y);
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.localPosition"/> as a <see cref="float3"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localPosition"/> from.</param>
        /// <returns>The <see cref="Transform.localPosition"/> as a <see cref="float3"/></returns>
        public static float3 LocalPosition3(this Transform _Transform)
        {
            return new float3(_Transform.localPosition.x, _Transform.localPosition.y, _Transform.localPosition.z);
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.position"/> as a <see cref="float3"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.position"/> from.</param>
        /// <returns>The <see cref="Transform.position"/> as a <see cref="float3"/></returns>
        public static float3 Position3(this Transform _Transform)
        {
            return new float3(_Transform.position.x, _Transform.position.y, _Transform.position.z);
        }

        /// <summary>
        /// Returns the <see cref="Transform.eulerAngles"/> rotation of this <see cref="Transform"/> with the <see cref="Vector3.z"/> value set to <c>0</c>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.eulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.eulerAngles"/> rotation as a <see cref="Quaternion"/>.</returns>
        public static Quaternion Rotation2D(this Transform _Transform)
        {
            return _Transform.eulerAngles.WithZ(0).ToQuaternion();
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.localEulerAngles"/> rotation of this <see cref="Transform"/> with the <see cref="Vector3.z"/> value set to <c>0</c>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localEulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.localEulerAngles"/> rotation as a <see cref="Quaternion"/>.</returns>
        public static Quaternion LocalRotation2D(this Transform _Transform)
        {
            return _Transform.localEulerAngles.WithZ(0).ToQuaternion();
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.eulerAngles"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.eulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.eulerAngles"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.</returns>
        public static Vector2 Vector2EulerAngles(this Transform _Transform)
        {
            return _Transform.eulerAngles.ToVector2();
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.localEulerAngles"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localEulerAngles"/> from.</param>
        /// <returns>The <see cref="Transform.localEulerAngles"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.</returns>
        public static Vector2 LocalVector2EulerAngles(this Transform _Transform)
        {
            return _Transform.localEulerAngles.ToVector2();
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.position"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.position"/> from.</param>
        /// <returns>The <see cref="Transform.position"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.</returns>
        public static Vector2 Vector2Position(this Transform _Transform)
        {
            return _Transform.position.ToVector2();
        }
        
        /// <summary>
        /// Returns the <see cref="Transform.localPosition"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> to get the <see cref="Transform.localPosition"/> from.</param>
        /// <returns>The <see cref="Transform.localPosition"/> of this <see cref="Transform"/> as a <see cref="Vector2"/>.</returns>
        public static Vector2 LocalVector2Position(this Transform _Transform)
        {
            return _Transform.localPosition.ToVector2();
        }
        #endregion
    }
}
