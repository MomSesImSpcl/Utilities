using System;
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
        /// Calculates a world-space <see cref="Transform.position"/> offset from the <see cref="Transform"/>'s <see cref="Transform.localPosition"/> in the specified <see cref="TransformDirection"/> and distance.
        /// </summary>
        /// <param name="_Transform">The <see cref="Transform"/> whose <see cref="Transform.localPosition"/> will be used as a reference.</param>
        /// <param name="_Direction">The direction in local space to move the position.</param>
        /// <param name="_Value">The distance to move in the specified direction.</param>
        /// <param name="_Operation">The mathematical <see cref="Operation"/> to perform.</param>
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <see cref="TransformDirection"/> is not a valid <see cref="TransformDirection"/>.</exception>
        public static Vector3 Local(this Transform _Transform, TransformDirection _Direction, float _Value, Operation _Operation = Operation.Add)
        {
            return _Transform.TransformPoint(_Direction switch
            {
                TransformDirection.Up => Vector3.zero.Operation(_Operation, Axis.Y, _Value),
                TransformDirection.Down => Vector3.zero.Operation(_Operation, Axis.Y, _Value),
                TransformDirection.Left => Vector3.zero.Operation(_Operation, Axis.X, _Value),
                TransformDirection.Right => Vector3.zero.Operation(_Operation, Axis.X, _Value),
                TransformDirection.UpLeft => Vector3.zero.Operation(_Operation, Axis.Y, _Value).Minus(Axis.X, _Value),
                TransformDirection.UpRight => Vector3.zero.Operation(_Operation, Axis.Y, _Value).Plus(Axis.X, _Value),
                TransformDirection.DownLeft => Vector3.zero.Operation(_Operation, Axis.Y, _Value).Minus(Axis.X, _Value),
                TransformDirection.DownRight => Vector3.zero.Operation(_Operation, Axis.Y, _Value).Plus(Axis.X, _Value),
                _ => throw new ArgumentOutOfRangeException(nameof(_Direction), _Direction, null)
            });
        }
        #endregion
    }
}
