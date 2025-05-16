using System.Runtime.CompilerServices;
using MomSesImSpcl.Utilities;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Corner"/>.
    /// </summary>
    public static class CornerExtensions
    {
        #region Methods
#pragma warning disable CS8524
        /// <summary>
        /// Returns the <see cref="RectTransform.pivot"/> for the given <see cref="Corner"/>.
        /// </summary>
        /// <param name="_Corner">The <see cref="Corner"/> to get the <see cref="RectTransform.pivot"/> for.</param>
        /// <returns>The <see cref="RectTransform.pivot"/> for the given <see cref="Corner"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPivot(this Corner _Corner) => _Corner switch
        {
            Corner.UpperLeft => new Vector2(0, 1),
            Corner.UpperCenter => new Vector2(.5f, 1),
            Corner.UpperRight => new Vector2(1, 1),
            Corner.MiddleLeft => new Vector2(0, .5f),
            Corner.MiddleCenter => new Vector2(.5f, .5f),
            Corner.MiddleRight => new Vector2(1, .5f),
            Corner.LowerLeft => new Vector2(0, 0),
            Corner.LowerCenter => new Vector2(.5f, 0),
            Corner.LowerRight => new Vector2(1, 0)
        };
#pragma warning restore CS8524
        #endregion
    }
}