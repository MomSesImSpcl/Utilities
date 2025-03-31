#if ODIN_INSPECTOR
using MomSesImSpcl.Attributes;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace MomSesImSpcl.Editor.PropertyDrawers
{
    /// <summary>
    /// <see cref="OdinAttributeDrawer{TAttribute}"/> for <see cref="ChildLabelAttribute"/>.
    /// </summary>
    public sealed class ChildLabelDrawer : OdinAttributeDrawer<ChildLabelAttribute>
    {
        #region Methods
        protected override void DrawPropertyLayout(GUIContent _Label)
        {
            var _attribute = base.Attribute;
            
            foreach (var _child in base.Property.Children)
            {
                foreach (var _nestedChild in _child.Children)
                {
                    if (_nestedChild.Name == _attribute.MemberName)
                    {
                        _nestedChild.Label.text = _attribute.NewLabel;
                    }
                }
                
                if (_child.Name == _attribute.MemberName)
                {
                    _child.Label.text = _attribute.NewLabel;
                }
                
                _child.Draw();
            }
        }
        #endregion
    }
}
#endif
