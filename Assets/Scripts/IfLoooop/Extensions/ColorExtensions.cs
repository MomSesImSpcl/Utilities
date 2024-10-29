using UnityEngine;

namespace IfLoooop.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Color"/>
    /// </summary>
    public static class ColorExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the alpha value of the given color.
        /// </summary>
        /// <param name="_Color">The original color.</param>
        /// <param name="_AlphaValue">The alpha value to be set, ranging from 0 to 1.</param>
        /// <return>The color with the updated alpha value.</return>
        public static Color WithAlpha(this Color _Color, float _AlphaValue)
        {
            _Color.a = _AlphaValue;
            return _Color;
        }
        #endregion
    }
}