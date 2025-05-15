using System;
using Emericoude.Helpers;
using Emericoude.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(UILineRendererController))]
    public class UILineRendererControllerEditor : SerializedEditor
    {
        private const string EDITOR_PREF_KEY = "EMERICOUDE_UI_LINE_RENDERER_CONTROLLER_EDITOR";
        protected override string EditorPrefKey => EDITOR_PREF_KEY;
        
        //TODO: handles for some things could be useful (such as for visualizing corners, or moving manual points).

        [SerializeField] private bool referenceContainerFoldout;
        [SerializeField] private bool updateLoopContainerFoldout;
        [SerializeField] private bool styleContainerFoldout;
        
        public override VisualElement CreateInspectorGUI() {
            return new UILineRendererControllerVisualElement(this, this.serializedObject);
        }

        private sealed class UILineRendererControllerVisualElement : VisualElement
        {
            private readonly VisualElement elbowContainer;
            private readonly VisualElement manualPointsContainer;
            private readonly UILineRendererControllerEditor TargetEditor;
            
            public UILineRendererControllerVisualElement(UILineRendererControllerEditor editor, SerializedObject serializedObject) {
                this.TargetEditor = editor;
                this.ApplyCustomEditorRootStyle();

                var lineRendererProperty = serializedObject.FindProperty("m_Line");
                var fromProperty = serializedObject.FindProperty("m_From");
                var toProperty = serializedObject.FindProperty("m_To");

                var drawOnStartProperty = serializedObject.FindProperty("m_DrawOnStart");
                var redrawOnMoveProperty = serializedObject.FindProperty("m_RedrawIfEndpointMoves");
                var destroySelfOnNull = serializedObject.FindProperty("m_DestroySelfIfEndpointIsNull");
                
                var startPointPositionProperty = serializedObject.FindProperty("m_StartPositionMethod");
                var endPointPositionProperty = serializedObject.FindProperty("m_EndPositionMethod");
                var endpointsIterationsProperty = serializedObject.FindProperty("m_EndpointsPositionIterations");
                var lineStyleProperty = serializedObject.FindProperty("m_PointStyle");
                
                var elbowCenterMethod = serializedObject.FindProperty("m_ElbowPositionMethod");
                var elbowCenterProperty = serializedObject.FindProperty("m_ElbowCenter");
                var fromElbowDirectionProperty = serializedObject.FindProperty("m_ElbowFromDirection");
                var toElbowDirectionProperty = serializedObject.FindProperty("m_ElbowToDirection");
                
                var pointsManualProperty = serializedObject.FindProperty("m_ManualPoints");

                var referenceContainer = this.AddEditorFoldoutContainer(editor.referenceContainerFoldout, "References", "", this.OnReferenceFoldoutValueChange);
                referenceContainer.AddPropertyField(lineRendererProperty);
                referenceContainer.AddPropertyField(fromProperty);
                referenceContainer.AddPropertyField(toProperty);

                var updateLoopContainer = this.AddEditorFoldoutContainer(editor.updateLoopContainerFoldout, "Lifetime Settings", "", this.OnUpdateLoopContainerValueChange);
                updateLoopContainer.AddToggleField(drawOnStartProperty);
                updateLoopContainer.AddToggleField(redrawOnMoveProperty);
                updateLoopContainer.AddToggleField(destroySelfOnNull);

                var styleContainer = this.AddEditorFoldoutContainer(editor.styleContainerFoldout, "Style", "", this.OnStyleContainerValueChange);
                styleContainer.AddEnumField(startPointPositionProperty);
                styleContainer.AddEnumField(endPointPositionProperty);
                styleContainer.AddPropertyField(endpointsIterationsProperty);
                styleContainer.AddEnumField(lineStyleProperty, this.OnLineStyleChanged);

                this.elbowContainer = styleContainer.AddEmptyVisualElement();
                this.elbowContainer.AddEnumField(elbowCenterMethod);
                this.elbowContainer.AddPropertyField(elbowCenterProperty);
                this.elbowContainer.AddEnumField(fromElbowDirectionProperty);
                this.elbowContainer.AddEnumField(toElbowDirectionProperty);
                
                this.manualPointsContainer = styleContainer.AddEmptyVisualElement();
                this.manualPointsContainer.AddPropertyField(pointsManualProperty);
                
                var value = (UILineRendererController.PointDrawingMethods)lineStyleProperty.enumValueIndex;
                this.UpdateContainerVisibilityFromLineStyle(value);
            }
            
            private void OnLineStyleChanged(ChangeEvent<Enum> changeEvent) {
                var value = (UILineRendererController.PointDrawingMethods)changeEvent.newValue;
                this.UpdateContainerVisibilityFromLineStyle(value);
            }

            private void UpdateContainerVisibilityFromLineStyle(UILineRendererController.PointDrawingMethods value) {
                this.manualPointsContainer.style.SetDisplay(value is UILineRendererController.PointDrawingMethods.Manual);
                this.elbowContainer.style.SetDisplay(value is UILineRendererController.PointDrawingMethods.Elbow);
            }

            private void OnReferenceFoldoutValueChange(ChangeEvent<bool> changed)
            {
                this.TargetEditor.referenceContainerFoldout = changed.newValue;
            }

            private void OnUpdateLoopContainerValueChange(ChangeEvent<bool> changed)
            {
                this.TargetEditor.updateLoopContainerFoldout = changed.newValue;
            }

            private void OnStyleContainerValueChange(ChangeEvent<bool> changed)
            {
                this.TargetEditor.styleContainerFoldout = changed.newValue;
            }
        }
    }
}