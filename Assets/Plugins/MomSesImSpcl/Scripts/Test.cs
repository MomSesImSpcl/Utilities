using MomSesImSpcl.Components;
using MomSesImSpcl.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MomSesImSpcl
{
    /// <summary>
    /// For editor tests.
    /// </summary>
    internal sealed class Test : EditorMonoBehaviour
    {
#if UNITY_EDITOR
        private readonly ShuffleBag<bool> shuffleBag = new(true, false, .9f);
        
        [Button]
        private void TestButton()
        {
            Debug.Log(this.shuffleBag.Draw().ToString());
        }
#endif
    }
}