using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Selectable"/>.
    /// </summary>
    public static class SelectableExtensions
    {
        #region Methods
        /// <summary>
        /// Find the next <see cref="Selectable.interactable"/> <see cref="Selectable"/> in the given <see cref="MoveDirection"/>.
        /// </summary>
        /// <param name="_Selectable">The <see cref="Selectable"/> from where to find the next <see cref="Selectable"/>.</param>
        /// <param name="_Direction">The direction to search the next <see cref="Selectable"/>.</param>
        /// <returns>The next <see cref="Selectable"/> that is <see cref="Selectable.interactable"/>, or <c>null</c> if no <see cref="Selectable"/> could be found.</returns>
        [CanBeNull]
        public static Selectable GetNextActive(this Selectable _Selectable, MoveDirection _Direction)
        {
            return _Selectable.GetNextActive(_Direction, out _);
        }
        
        /// <summary>
        /// Find the next <see cref="Selectable.interactable"/> <see cref="Selectable"/> in the given <see cref="MoveDirection"/>.
        /// </summary>
        /// <param name="_Selectable">The <see cref="Selectable"/> from where to find the next <see cref="Selectable"/>.</param>
        /// <param name="_Direction">The direction to search the next <see cref="Selectable"/>.</param>
        /// <param name="_Previous">The <see cref="Selectable"/> that came before the returned <see cref="Selectable"/> in the given <see cref="MoveDirection"/>.</param>
        /// <returns>The next <see cref="Selectable"/> that is <see cref="Selectable.interactable"/>, or <c>null</c> if no <see cref="Selectable"/> could be found.</returns>
        [CanBeNull]
        public static Selectable GetNextActive(this Selectable _Selectable, MoveDirection _Direction, out Selectable _Previous)
        {
            _Previous = _Selectable;
            
            while (true)
            {
                var _nextSelectable = _Direction switch
                {
                    MoveDirection.Left => _Previous.FindSelectableOnLeft(),
                    MoveDirection.Up => _Previous.FindSelectableOnUp(),
                    MoveDirection.Right => _Previous.FindSelectableOnRight(),
                    MoveDirection.Down => _Previous.FindSelectableOnDown(),
                    _ => _Previous
                };
                
                if (!_nextSelectable)
                {
                    return null;
                }
                
                if (_nextSelectable.interactable && _nextSelectable.gameObject.activeInHierarchy)
                {
                    return _nextSelectable;
                }
                
                // In case of a navigation loop.
                if (_nextSelectable == _Selectable)
                {
                    return _Selectable.interactable ? _Selectable : null;
                }
                
                _Previous = _nextSelectable;
            }
        }
        #endregion
    }
}
