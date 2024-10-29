using System.Diagnostics;
using UnityEngine;

namespace IfLoooop.Attributes
{
    /// <summary>
    /// ElementNamesAttribute is a custom attribute used to assign custom names to elements in an array or list
    /// for display in the Unity Editor.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public sealed class ElementNamesAttribute : PropertyAttribute
    {
        #region Properties
        /// <summary>
        /// Gets the custom names assigned to the elements in an array or list for display in the Unity Editor.
        /// </summary>
        public string[] ElementNames { get; }
        /// <summary>
        /// Indicates whether to display the index of each element in the array or list alongside its custom name in the Unity Editor.
        /// </summary>
        public bool DisplayIndex { get; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ElementNameAttribute"/>.
        /// </summary>
        /// <param name="_ElementNames"><see cref="ElementNames"/>.</param>
        public ElementNamesAttribute(params string[] _ElementNames)
        {
            this.ElementNames = _ElementNames;
            this.DisplayIndex = false;
        }
        
        /// <summary>
        /// <see cref="ElementNameAttribute"/>.
        /// </summary>
        /// <param name="_DisplayIndex"><see cref="DisplayIndex"/>.</param>
        /// <param name="_ElementNames"><see cref="ElementNames"/>.</param>
        public ElementNamesAttribute(bool _DisplayIndex, params string[] _ElementNames)
        {
            this.ElementNames = _ElementNames;
            this.DisplayIndex = _DisplayIndex;
        }
        #endregion
    }
}