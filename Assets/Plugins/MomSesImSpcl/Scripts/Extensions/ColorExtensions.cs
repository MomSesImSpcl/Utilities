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
            return new Color
            (
                _Color.r + (_TargetColor.r - _Color.r) * _Percentage,
                _Color.g + (_TargetColor.g - _Color.g) * _Percentage,
                _Color.b + (_TargetColor.b - _Color.b) * _Percentage
                
            ).ToHex();
        }

        /// <summary>
        /// Converts a <see cref="Color"/> <see cref="object"/> that was initialized with <see cref="byte"/> values <c>0-255</c>, into <see cref="float"/> values <c>0-1</c>.
        /// </summary>
        /// <param name="_Color">Must have been initialized with values from <c>0-255</c>.</param>
        /// <returns>A new <see cref="Color"/> <see cref="object"/> with values converted from <c>0-255</c> to <c>0-1</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ByteToFloat(this Color _Color)
        {
            return new Color(_Color.r / 255f, _Color.g / 255f, _Color.b / 255f, _Color.a / 255f);;
        }
        
        /// <summary>
        /// Converts the current color to its hexadecimal string representation.
        /// </summary>
        /// <param name="_Color">The color to convert.</param>
        /// <return>The hexadecimal string representation of the color.</return>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this Color _Color)
        {
            var _r = (int)math.round(_Color.r * 255);
            var _g = (int)math.round(_Color.g * 255);
            var _b = (int)math.round(_Color.b * 255);
    
            return $"#{_r:X2}{_g:X2}{_b:X2}";
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
