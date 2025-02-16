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
        /// <returns>The new world-space <see cref="Transform.position"/> after applying the offset in the given direction.</returns>
        public static Vector3 Local(this Transform _Transform, Axis _Axis, float _Value, Operation _Operation = Operation.Add)
        {
            return _Transform.TransformPoint(Vector3.zero.Operation(_Operation, _Axis, _Value));
        }
        #endregion
    }
}
