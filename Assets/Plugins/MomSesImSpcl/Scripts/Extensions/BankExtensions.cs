#if FMOD
using FMOD;
using FMOD.Studio;

namespace MomSesImSpcl.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="Bank"/>.
    /// </summary>
    public static class BankExtensions
    {
        #region Methods
        /// <summary>
        /// Returns the name of this <see cref="Bank"/>.
        /// </summary>
        /// <param name="_Bank">The <see cref="Bank"/> to get the name of.</param>
        /// <returns>The name of this <see cref="Bank"/> or <see cref="string"/>.<see cref="string.Empty"/> on error.</returns>
        public static string GetBankName(this Bank _Bank)
        {
            if (_Bank.getPath(out var _bankPath) is var _pathResult && _pathResult != RESULT.OK)
            {
                UnityEngine.Debug.LogError(_pathResult);
                return string.Empty;
            }

            return _bankPath.ExtractAfter("/");
        }
        #endregion
    }
}
#endif