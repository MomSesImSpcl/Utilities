using System;
using System.Reflection;

namespace MomSesImSpcl.Utilities.Logging
{
    /// <summary>
    /// Contains helper methods for the Unity Console.
    /// </summary>
    public static class Console
    {
#if UNITY_EDITOR
        #region Methods
        /// <summary>
        /// Clears the unity console.
        /// </summary>
        public static void Clear()
        {
            var _logEntries = Type.GetType($"{nameof(UnityEditor)}.LogEntries, {nameof(UnityEditor)}");
            var _clearMethod = _logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            
            _clearMethod!.Invoke(null, null);
        }
        #endregion
#endif
    }
}