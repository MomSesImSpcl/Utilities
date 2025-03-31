#if ODIN_INSPECTOR
using System;
using System.Diagnostics;
using UnityEngine;

namespace MomSesImSpcl.Attributes
{
    /// <summary>
    /// Changes the label of a field/property in the <see cref="Type"/> this attribute is attached to.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    public sealed class ChildLabelAttribute : PropertyAttribute
    {
        #region Properties
        /// <summary>
        /// The field/property to change the label of.
        /// </summary>
        public string Member { get; }
        /// <summary>
        /// The new label name.
        /// </summary>
        public string NewLabel { get; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="ChildLabelAttribute"/>.
        /// </summary>
        /// <param name="_Member"><see cref="Member"/>.</param>
        /// <param name="_NewLabel"><see cref="NewLabel"/>.</param>
        public ChildLabelAttribute(string _Member, string _NewLabel)
        {
            this.Member = _Member;
            this.NewLabel = _NewLabel;
        }
        #endregion
    }
}
#endif