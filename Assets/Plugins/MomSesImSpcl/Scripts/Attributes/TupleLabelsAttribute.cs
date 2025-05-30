#if ODIN_INSPECTOR
using System;
using System.Diagnostics;
using UnityEngine;

namespace MomSesImSpcl.Attributes
{
    /// <summary>
    /// Attribute to specify custom labels for tuple items in the Unity Inspector.
    /// This attribute is used in conjunction with the SerializedTupleDrawer
    /// to display custom labels for each item of the tuple.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class TupleLabelsAttribute : PropertyAttribute
    {
        #region Properties
        /// <summary>
        /// Gets the label for the first item of the tuple.
        /// </summary>
        internal string Item1Label { get; }
        /// <summary>
        /// Gets the label for the second item of the tuple.
        /// </summary>
        internal string Item2Label { get; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="TupleLabelsAttribute"/>.
        /// </summary>
        /// <param name="_Item1Label"><see cref="Item1Label"/>.</param>
        /// <param name="_Item2Label"><see cref="Item2Label"/>.</param>
        public TupleLabelsAttribute(string _Item1Label, string _Item2Label)
        {
            this.Item1Label = _Item1Label;
            this.Item2Label = _Item2Label;
        }
        #endregion
    } 
}
#endif
