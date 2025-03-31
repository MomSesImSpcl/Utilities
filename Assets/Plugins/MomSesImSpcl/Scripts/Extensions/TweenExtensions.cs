#if DOTWEEN
using System;
using DG.Tweening;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Tween"/>.
    /// </summary>
    public static class TweenExtensions
    {
        #region Methods
        /// <summary>
        /// Adds the give <see cref="Action"/> to the <c>OnKill()</c> and <c>OnComplete()</c> callbacks.
        /// </summary>
        /// <param name="_Tween">The <see cref="Tween"/> to add the callback to.</param>
        /// <param name="_Action">The <see cref="Action"/> to invoke.</param>
        /// <typeparam name="T">Must be a <see cref="Tween"/>.</typeparam>
        /// <returns>This <see cref="Tween"/>.</returns>
        public static T OnFinish<T>(this T _Tween, Action _Action) where T : Tween
        {
            var _onKill = _Tween.onKill;
            var _onComplete = _Tween.onComplete;
            
            return _Tween.OnKill(() =>
            {
                _onKill?.Invoke();
                _Action();

            }).OnComplete(() =>
            {
                _onComplete?.Invoke();
                _Action();
                
            });
        }
        #endregion
    }
}
#endif
