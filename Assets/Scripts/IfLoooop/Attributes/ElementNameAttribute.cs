using System.Diagnostics;
using UnityEngine;

namespace IfLoooop.Attributes
{
    /// <summary>
    /// Used to annotate elements with a custom name in the Unity inspector.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public class ElementNameAttribute : PropertyAttribute
    {
        #region Properties
        /// <summary>
        /// The custom name to display for the annotated element in the Unity inspector.
        /// </summary>
        public string ElementName { get; }
        /// <summary>
        /// Indicates whether the index of the element should be displayed alongside its custom name in the Unity inspector.
        /// </summary>
        public bool DisplayIndex { get; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ElementNameAttribute"/>.
        /// </summary>
        /// <param name="_ElementName"><see cref="ElementName"/>.</param>
        /// <param name="_DisplayIndex"><see cref="DisplayIndex"/>.</param>
        public ElementNameAttribute(string _ElementName, bool _DisplayIndex = true)
        {
            this.ElementName = _ElementName;
            this.DisplayIndex = _DisplayIndex;
        }
        #endregion
    }
}