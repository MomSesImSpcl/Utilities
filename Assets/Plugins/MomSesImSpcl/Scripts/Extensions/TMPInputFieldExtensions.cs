#if TMP
using TMPro;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="TMP_InputField"/>.
    /// </summary>
    public static class TMPInputFieldExtensions
    {
        #region Methods
        /// <summary>
        /// Deselects the <see cref="TMP_InputField"/> and releases it from focus.
        /// </summary>
        /// <param name="_InputField">The <see cref="TMP_InputField"/> to deselect.</param>
        public static void Deselect(this TMP_InputField _InputField)
        {
            // Must be in this order.
            _InputField.DeactivateInputField();
            _InputField.ReleaseSelection();
        }
        #endregion
    }
}
#endif