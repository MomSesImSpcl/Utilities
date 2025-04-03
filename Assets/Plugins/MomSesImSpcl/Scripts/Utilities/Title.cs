#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Utility class to display a title without a field in the inspector.
    /// </summary>
    [Serializable, HideLabel, InlineProperty]
    public sealed class Title
    {
#if UNITY_EDITOR
        #region Inspector Fields
#pragma warning disable CS0414
        [TitleGroup("$titleName", Alignment = TitleAlignments.Centered, BoldTitle = true, HorizontalLine = true, HideWhenChildrenAreInvisible = false)]
        [HideIf(nameof(titleField))]
        [ShowInInspector] private static bool titleField = true;
#pragma warning restore CS0414
        #endregion
        
        #region Fields
        /// <summary>
        /// The title that will be displayed in the inspector
        /// </summary>
        private readonly string titleName;
        #endregion
        
        #region Constructors
        /// <summary>
        /// <see cref="Title"/>.
        /// </summary>
        /// <param name="_TitleName"><see cref="titleName"/>.</param>
        public Title(string _TitleName)
        {
            this.titleName = _TitleName;
        }
        #endregion
#endif
    }
}
#endif