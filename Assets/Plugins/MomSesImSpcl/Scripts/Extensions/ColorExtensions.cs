using System.Runtime.CompilerServices;
using Unity.Mathematics;
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
        /// <param name="_Percentage">
        /// The percentage of blending between the original and target colors. <br/>
        /// <i><c>0</c>-<c>1</c>.</i>
        /// </param>
        /// <return>The blended color as a hexadecimal string.</return>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string BlendColors(this Color _Color, Color _TargetColor, float _Percentage)
        {
            var _red = (int)math.round((_Color.r + (_TargetColor.r - _Color.r) * _Percentage) * 255);
            var _green = (int)math.round((_Color.g + (_TargetColor.g - _Color.g) * _Percentage) * 255);
            var _blue = (int)math.round((_Color.b + (_TargetColor.b - _Color.b) * _Percentage) * 255);
    
            return $"#{_red:X2}{_green:X2}{_blue:X2}";
        }

        /// <summary>
        /// Converts the given <see cref="byte"/> <c>RGBA</c> values <c>0-255</c>, into <see cref="float"/> values <c>0-1</c>.
        /// </summary>
        /// <param name="_Color"><see cref="Color"/>.</param>
        /// <param name="_R"><see cref="Color.r"/>.</param>
        /// <param name="_G"><see cref="Color.g"/>.</param>
        /// <param name="_B"><see cref="Color.b"/>.</param>
        /// <param name="_A"><see cref="Color.a"/>.</param>
        /// <returns>A new <see cref="Color"/> <see cref="object"/> with the given values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromByte(this Color _Color, byte _R, byte _G, byte _B, byte _A = 0)
        {
            return new Color(_R / 255f, _G / 255f, _B / 255f, _A / 255f);;
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
