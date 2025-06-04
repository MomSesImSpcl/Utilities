#if REWIRED
using System.Runtime.CompilerServices;
using Rewired;
using UnityEngine.EventSystems;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="InputActionEventData"/>.
    /// </summary>
    public static class InputActionEventDataExtensions
    {
        #region Methods
        /// <summary>
        /// Converts horizontal Rewired input into <see cref="MoveDirection"/>.
        /// </summary>
        /// <param name="_InputActionEventData"><see cref="InputActionEventData"/>.</param>
        /// <returns><see cref="MoveDirection.Right"/> when the axis input is positive, otherwise <see cref="MoveDirection.Left"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MoveDirection HorizontalDirection(this InputActionEventData _InputActionEventData)
        {
            return _InputActionEventData.GetAxis() > 0 ? MoveDirection.Right : MoveDirection.Left;
        }
        
        /// <summary>
        /// Converts vertical Rewired input into <see cref="MoveDirection"/>.
        /// </summary>
        /// <param name="_InputActionEventData"><see cref="InputActionEventData"/>.</param>
        /// <returns><see cref="MoveDirection.Up"/> when the axis input is positive, otherwise <see cref="MoveDirection.Down"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MoveDirection VerticalDirection(this InputActionEventData _InputActionEventData)
        {
            return _InputActionEventData.GetAxis() > 0 ? MoveDirection.Up : MoveDirection.Down;
        }
        #endregion
    }
}
#endif