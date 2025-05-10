#if FMOD
using System.Runtime.CompilerServices;
using FMOD.Studio;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="PARAMETER_DESCRIPTION"/>.
    /// </summary>
    public static class ParameterDescriptionExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the actual parameter name of this <see cref="PARAMETER_DESCRIPTION"/>.
        /// </summary>
        /// <param name="_ParameterDescription">The <see cref="PARAMETER_DESCRIPTION"/> to get the name of.</param>
        /// <returns>The actual parameter name of this <see cref="PARAMETER_DESCRIPTION"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(this PARAMETER_DESCRIPTION _ParameterDescription)
        {
            return _ParameterDescription.name;
        }
        #endregion
    }
}
#endif
