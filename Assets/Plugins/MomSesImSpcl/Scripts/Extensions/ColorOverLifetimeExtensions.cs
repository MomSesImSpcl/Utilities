using UnityEngine;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="ParticleSystem.ColorOverLifetimeModule"/>.
    /// </summary>
    public static class ColorOverLifetimeExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the <see cref="Color"/> of every <see cref="GradientColorKey"/> in this <see cref="ParticleSystem.ColorOverLifetimeModule"/> to the given <see cref="Color"/>.
        /// </summary>
        /// <param name="_ColorOverLifetimeModule">The <see cref="ParticleSystem.ColorOverLifetimeModule"/> to set the <see cref="Gradient"/> of.</param>
        /// <param name="_Color">The target <see cref="Color"/>.</param>
        public static void SetColors(this ParticleSystem.ColorOverLifetimeModule _ColorOverLifetimeModule, Color _Color)
        {
            var _gradient = _ColorOverLifetimeModule.color.gradient;
            var _colorKeys = _gradient.colorKeys;

            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _colorKeys.Length; i++)
            {
                _colorKeys[i].color = _Color;
            }
            
            _gradient.colorKeys = _colorKeys;
            _ColorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(_gradient);
        }
        
        /// <summary>
        /// Sets the <see cref="Color"/> of the <see cref="GradientColorKey"/> at the given <c>_Index</c> in this <see cref="ParticleSystem.ColorOverLifetimeModule"/> to the given <see cref="Color"/>.
        /// </summary>
        /// <param name="_ColorOverLifetimeModule">The <see cref="ParticleSystem.ColorOverLifetimeModule"/> to set the <see cref="Gradient"/> of.</param>
        /// <param name="_Index">Index in <see cref="Gradient.colorKeys"/> to set the <see cref="Color"/> of.</param>
        /// <param name="_Color">The target <see cref="Color"/>.</param>
        public static void SetColor(this ParticleSystem.ColorOverLifetimeModule _ColorOverLifetimeModule, int _Index, Color _Color)
        {
            var _gradient = _ColorOverLifetimeModule.color.gradient;
            var _colorKeys = _gradient.colorKeys;

            _colorKeys[_Index].color = _Color;
            
            _gradient.colorKeys = _colorKeys;
            _ColorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(_gradient);
        }
        #endregion
    }
}