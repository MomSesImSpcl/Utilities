#if REWIRED
using System.ComponentModel;
using MomSesImSpcl.Utilities;
using Rewired.Editor;
using UnityEditor;

namespace MomSesImSpcl.Editor.CustomEditors
{
    /// <summary>
    /// Custom editor for <see cref="CustomRewiredStandaloneInputModule"/>.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [CustomEditor(typeof(CustomRewiredStandaloneInputModule))]
    public sealed class CustomRewiredStandaloneInputModuleEditor : RewiredStandaloneInputModuleInspector { }
}
#endif