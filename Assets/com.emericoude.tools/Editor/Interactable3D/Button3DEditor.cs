using UnityEditor;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(Button3D))]
    public class Button3DEditor : Navigable3DEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var visualElement = (Navigable3DVisualElement)base.CreateInspectorGUI();
            visualElement.EventsContainer.AddPropertyField(this.serializedObject.FindProperty("onClicked"));
            return visualElement;
        }
    }
}