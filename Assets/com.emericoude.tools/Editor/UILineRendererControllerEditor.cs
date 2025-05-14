using System;
using Emericoude.Helpers;
using Emericoude.UI;
using Emericoude.UI.NodeGraph;
using UnityEditor;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(UILineRendererController))]
    public class UILineRendererControllerEditor : Editor
    {
        //TODO: handles for some things could be useful (such as for visualizing corners, or moving manual points).
        
        public override VisualElement CreateInspectorGUI() {
            return new UILineRendererControllerVisualElement(this, this.serializedObject);
        }

        private sealed class UILineRendererControllerVisualElement : VisualElement
        {
            private readonly VisualElement elbowContainer;
            private readonly VisualElement manualPointsContainer;
            
            public UILineRendererControllerVisualElement(UILineRendererControllerEditor editor, SerializedObject serializedObject) {
                this.ApplyCustomEditorRootStyle();

                var lineRendererProperty = serializedObject.FindProperty("m_Line");
                var fromProperty = serializedObject.FindProperty("m_From");
                var toProperty = serializedObject.FindProperty("m_To");
                
                var startPointPositionProperty = serializedObject.FindProperty("m_StartPositionMethod");
                var endPointPositionProperty = serializedObject.FindProperty("m_EndPositionMethod");
                var endpointsIterationsProperty = serializedObject.FindProperty("m_EndpointsPositionIterations");
                var lineStyleProperty = serializedObject.FindProperty("m_PointStyle");
                
                var elbowCenterMethod = serializedObject.FindProperty("m_ElbowPositionMethod");
                var elbowCenterProperty = serializedObject.FindProperty("m_ElbowCenter");
                var fromElbowDirectionProperty = serializedObject.FindProperty("m_ElbowFromDirection");
                var toElbowDirectionProperty = serializedObject.FindProperty("m_ElbowToDirection");
                
                var pointsManualProperty = serializedObject.FindProperty("m_ManualPoints");

                this.AddPropertyField(lineRendererProperty);
                this.AddPropertyField(fromProperty);
                this.AddPropertyField(toProperty);

                this.AddEnumField(startPointPositionProperty);
                this.AddEnumField(endPointPositionProperty);
                this.AddPropertyField(endpointsIterationsProperty);
                this.AddEnumField(lineStyleProperty, this.OnLineStyleChanged);

                this.elbowContainer = this.AddEmptyVisualElement();
                this.elbowContainer.AddEnumField(elbowCenterMethod);
                this.elbowContainer.AddPropertyField(elbowCenterProperty);
                this.elbowContainer.AddEnumField(fromElbowDirectionProperty);
                this.elbowContainer.AddEnumField(toElbowDirectionProperty);
                
                this.manualPointsContainer = this.AddEmptyVisualElement();
                this.manualPointsContainer.AddPropertyField(pointsManualProperty);
                
                var value = (NodeConnectionRenderer.PointsDrawingFormation)lineStyleProperty.enumValueIndex;
                this.UpdateContainerVisibilityFromLineStyle(value);
            }
            
            private void OnLineStyleChanged(ChangeEvent<Enum> changeEvent) {
                var value = (NodeConnectionRenderer.PointsDrawingFormation)changeEvent.newValue;
                this.UpdateContainerVisibilityFromLineStyle(value);
            }

            private void UpdateContainerVisibilityFromLineStyle(NodeConnectionRenderer.PointsDrawingFormation value) {
                this.manualPointsContainer.style.SetDisplay(value is NodeConnectionRenderer.PointsDrawingFormation.Manual);
                this.elbowContainer.style.SetDisplay(value is NodeConnectionRenderer.PointsDrawingFormation.Elbow);
            }
        }
    }
}