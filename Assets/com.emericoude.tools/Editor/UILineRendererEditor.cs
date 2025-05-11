using System;
using Emericoude.Helpers;
using Emericoude.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(UILineRenderer))]
    public class UILineRendererEditor : SerializedEditor
    {
        private const string EDITOR_PREF_KEY = "EMERICOUDE_UI_LINE_RENDERER_EDITOR";
        protected override string EditorPrefKey => EDITOR_PREF_KEY;
        
        [SerializeField] private bool graphicFoldoutOpen = false;
        [SerializeField] private bool pointsFoldoutOpen = false;
        [SerializeField] private bool styleSettingsOpen = false;
        
        public override VisualElement CreateInspectorGUI()
        {
            return new UILineRendererVisualElement(this, this.serializedObject);
        }
        
        private sealed class UILineRendererVisualElement : VisualElement
        {
            public readonly UILineRendererEditor TargetEditor;
            private readonly VisualElement splineStyleContainer;

            public UILineRendererVisualElement(UILineRendererEditor editor, SerializedObject serializedObject) : base() {
                this.TargetEditor = editor;
                this.ApplyCustomEditorRootStyle();
                
                #region Maskable Graphic Settings
                var materialProperty = serializedObject.FindProperty("m_Material");
                var colorProperty = serializedObject.FindProperty("m_Color");
                var raycastTargetProperty = serializedObject.FindProperty("m_RaycastTarget");
                var raycastPaddingProperty = serializedObject.FindProperty("m_RaycastPadding");
                var maskableProperty = serializedObject.FindProperty("m_Maskable");
                
                VisualElement maskableGraphicContainer1 = this.AddEditorFoldoutContainer(editor.graphicFoldoutOpen, "Maskable Graphic Settings", "", this.OnGraphicFoldoutValueChanged);
                maskableGraphicContainer1.AddPropertyField(materialProperty);
                maskableGraphicContainer1.AddPropertyField(colorProperty);
                maskableGraphicContainer1.AddToggleField(raycastTargetProperty);
                maskableGraphicContainer1.AddVector4Field(raycastPaddingProperty, "\u2190", "\u2193", "\u2192", "\u2191"); //left, bottom, right, top
                maskableGraphicContainer1.AddToggleField(maskableProperty);
                #endregion

                var useWorldSpaceProperty = serializedObject.FindProperty("m_UseWorldSpace");
                var pointsProperty = serializedObject.FindProperty("m_Points");
                
                VisualElement pointsContainer1 = this.AddEditorFoldoutContainer(editor.pointsFoldoutOpen, "Points", "", this.OnPointsFoldoutValueChanged);
                pointsContainer1.AddToggleField(useWorldSpaceProperty);
                pointsContainer1.AddPropertyField(pointsProperty);
                
                var spriteProperty = serializedObject.FindProperty("m_Sprite");
                var lineThicknessProperty = serializedObject.FindProperty("m_LineThickness");
                var cornerTypeProperty = serializedObject.FindProperty("m_CornerType");
                var lineCapsProperty = serializedObject.FindProperty("m_LineCaps");
                
                var drawWithSplineProperty = serializedObject.FindProperty("m_DrawWithSpline");
                var splineResolutionProperty = serializedObject.FindProperty("m_SplineResolution");
                var splineTangentModeProperty = serializedObject.FindProperty("m_SplineTangentMode");
                
                VisualElement lineStyleContainer1 = this.AddEditorFoldoutContainer(this.TargetEditor.styleSettingsOpen, "Line Style", "", this.OnLineStyleFoldoutValueChanged);
                lineStyleContainer1.AddPropertyField(spriteProperty);
                lineStyleContainer1.AddPropertyField(lineThicknessProperty);
                lineStyleContainer1.AddEnumField(cornerTypeProperty);
                lineStyleContainer1.AddEnumField(lineCapsProperty);

                lineStyleContainer1.AddToggleField(drawWithSplineProperty, this.OnDrawWithSplineValueChanged);
                this.splineStyleContainer = lineStyleContainer1.AddEmptyVisualElement();
                this.splineStyleContainer.AddEnumField(splineTangentModeProperty);
                this.splineStyleContainer.AddPropertyField(splineResolutionProperty);
                this.splineStyleContainer.style.SetDisplay(drawWithSplineProperty.boolValue);
            }
            
            private void OnGraphicFoldoutValueChanged(ChangeEvent<bool> changed)
            {
                this.TargetEditor.graphicFoldoutOpen = changed.newValue;
            }

            private void OnPointsFoldoutValueChanged(ChangeEvent<bool> changeEvent) {
                this.TargetEditor.pointsFoldoutOpen = changeEvent.newValue;
            }

            private void OnLineStyleFoldoutValueChanged(ChangeEvent<bool> changeEvent) {
                this.TargetEditor.styleSettingsOpen = changeEvent.newValue;
            }
            
            private void OnDrawWithSplineValueChanged(ChangeEvent<bool> changeEvent) {
                this.splineStyleContainer.style.SetDisplay(changeEvent.newValue);
            }
        }

        
    }
}