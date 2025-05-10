using System.Runtime.CompilerServices;
using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Color"/>
    /// </summary>
    public static class ColorExtensions
    {
        #region Methods
        /// <summary>
        /// Blends the current color with the target color based on the specified percentage.
        /// </summary>
        /// <param name="_Color">The original color.</param>
        /// <param name="_TargetColor">The target color in hexadecimal format.</param>
        /// <param name="_Percentage">The percentage of blending between the original and target colors.</param>
        /// <return>The blended color as a hexadecimal string.</return>
        public static string BlendColors(this Color _Color, string _TargetColor, float _Percentage)
        {
            return _Color.BlendColors(_TargetColor.HexToRGB(), _Percentage);
        }

        /// <summary>
        /// Blends the current color with the target color based on the specified percentage.
        /// </summary>
        /// <param name="_Color">The original color.</param>
        /// <param name="_TargetColor">The target color to blend to.</param>
        /// <param name="_Percentage">The percentage of blending between the original and target colors.</param>
        /// <return>The blended color as a hexadecimal string.</return>
        public static string BlendColors(this Color _Color, Color _TargetColor, float _Percentage)
        {
            var _red = (int)(_Color.r + (_TargetColor.r - _Color.r) * _Percentage);
            var _green = (int)(_Color.g + (_TargetColor.g - _Color.g) * _Percentage);
            var _blue = (int)(_Color.b + (_TargetColor.b - _Color.b) * _Percentage);
        
            return $"#{_red:X2}{_green:X2}{_blue:X2}";
        }
        
        /// <summary>
        /// Converts the current color to its hexadecimal string representation.
        /// </summary>
        /// <param name="_Color">The color to convert.</param>
        /// <return>The hexadecimal string representation of the color.</return>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this Color _Color)
        {
            return $"#{_Color.r:X2}{_Color.g:X2}{_Color.b:X2}";
        }
        
        /// <summary>
        /// Sets the alpha value of the given color.
        /// </summary>
        /// <param name="_Color">The original color.</param>
        /// <param name="_AlphaValue">The alpha value to be set, ranging from 0 to 1.</param>
        /// <return>The color with the updated alpha value.</return>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithAlpha(this Color _Color, float _AlphaValue)
        {
            _Color.a = _AlphaValue;
            return _Color;
        }
        #endregion
    }
}
