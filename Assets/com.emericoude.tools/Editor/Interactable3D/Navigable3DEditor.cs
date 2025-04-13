using System;
using Emericoude.Helpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(Navigable3D), true)]
    public class Navigable3DEditor : Editor
    {
        private const string EDITOR_PREF_KEY = "EMERICOUDE_NAVIGABLE_3D_EDITOR";
        [SerializeField] private bool eventFoldoutOpen = false;

        private void OnEnable()
        {
            string serializedData = EditorPrefs.GetString(EDITOR_PREF_KEY, JsonUtility.ToJson(this, false));
            JsonUtility.FromJsonOverwrite(serializedData, this);
        }

        private void OnDisable()
        {
            string serializedData = JsonUtility.ToJson(this, false);
            EditorPrefs.SetString(EDITOR_PREF_KEY, serializedData);
        }

        public override VisualElement CreateInspectorGUI()
        {
            return new Navigable3DVisualElement(this, this.serializedObject);
        }

        protected sealed class Navigable3DVisualElement : VisualElement
        {
            public VisualElement unityEventsContainer;
            public Navigable3DEditor editor;

            public Navigable3DVisualElement(Navigable3DEditor editor, SerializedObject serializedObject)
            {
                this.editor = editor;
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
                this.unityEventsContainer = this.AddEditorFoldoutContainer("Events", "", editor.eventFoldoutOpen, this.OnEventFoldoutValueChanged);
                this.unityEventsContainer.AddPropertyField(onSelectProperty);
                this.unityEventsContainer.AddPropertyField(onDeselectProperty);
                this.unityEventsContainer.AddPropertyField(onHoverEnterProperty);
                this.unityEventsContainer.AddPropertyField(onHoverExitProperty);
            }

            private void OnEventFoldoutValueChanged(ChangeEvent<bool> changed)
            {
                this.editor.eventFoldoutOpen = changed.newValue;
            }
        }
    }
}