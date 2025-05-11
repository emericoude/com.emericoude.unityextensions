    using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(Navigable3D), true)]
    public class Navigable3DEditor : SerializedEditor
    {
        protected override string EditorPrefKey => EDITOR_PREF_KEY;
        private const string EDITOR_PREF_KEY = "EMERICOUDE_NAVIGABLE_3D_EDITOR";
        [SerializeField] private bool eventFoldoutOpen = false;

        public override VisualElement CreateInspectorGUI()
        {
            return new Navigable3DVisualElement(this, this.serializedObject);
        }

        protected sealed class Navigable3DVisualElement : VisualElement
        {
            public readonly VisualElement EventsContainer;
            public readonly Navigable3DEditor TargetEditor;

            public Navigable3DVisualElement(Navigable3DEditor targetEditor, SerializedObject serializedObject)
            {
                this.TargetEditor = targetEditor;
                this.ApplyCustomEditorRootStyle();

                var interactableProperty = serializedObject.FindProperty("interactable");
                var cameraProperty = serializedObject.FindProperty("camera");
                var navigationProperty = serializedObject.FindProperty("navigation");
                
                this.AddToggleField(interactableProperty);
                this.AddObjectField<Camera>(cameraProperty);
                this.AddPropertyField(navigationProperty);

                var onSelectProperty = serializedObject.FindProperty("onSelect");
                var onDeselectProperty = serializedObject.FindProperty("onDeselect");
                var onHoverEnterProperty = serializedObject.FindProperty("onHoverEnter");
                var onHoverExitProperty = serializedObject.FindProperty("onHoverExit");

                //TODO: remember foldout value
                this.EventsContainer = this.AddEditorFoldoutContainer(targetEditor.eventFoldoutOpen, "Events", "", this.OnEventFoldoutValueChanged);
                this.EventsContainer.AddPropertyField(onSelectProperty);
                this.EventsContainer.AddPropertyField(onDeselectProperty);
                this.EventsContainer.AddPropertyField(onHoverEnterProperty);
                this.EventsContainer.AddPropertyField(onHoverExitProperty);
            }

            private void OnEventFoldoutValueChanged(ChangeEvent<bool> changed)
            {
                this.TargetEditor.eventFoldoutOpen = changed.newValue;
            }
        }
    }
}