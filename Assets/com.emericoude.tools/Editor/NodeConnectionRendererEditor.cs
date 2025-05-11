using System;
using Emericoude.Helpers;
using Emericoude.UI.NodeGraph;
using UnityEditor;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(NodeConnectionRenderer))]
    public class NodeConnectionRendererEditor : Editor
    {
        public override VisualElement CreateInspectorGUI() {
            return new NodeConnectionRendererVisualElement(this, this.serializedObject);
        }

        private sealed class NodeConnectionRendererVisualElement : VisualElement
        {
            private readonly VisualElement elbowContainer;
            private readonly VisualElement manualPointsContainer;
            
            public NodeConnectionRendererVisualElement(NodeConnectionRendererEditor editor, SerializedObject serializedObject) {
                this.ApplyCustomEditorRootStyle();
                
                var startPointPositionProperty = serializedObject.FindProperty("m_StartPointPosition");
                var endPointPositionProperty = serializedObject.FindProperty("m_EndPointPosition");
                var startAndEndPointIterationsProperty = serializedObject.FindProperty("m_StartAndEndPointIterations");
                var lineStyleProperty = serializedObject.FindProperty("m_LineStyle");
                
                var elbowCenterMethod = serializedObject.FindProperty("m_ElbowCenterMethod");
                var elbowCenterProperty = serializedObject.FindProperty("m_ElbowCenter");
                var fromElbowDirectionProperty = serializedObject.FindProperty("m_FromElbowDirection");
                var toElbowDirectionProperty = serializedObject.FindProperty("m_ToElbowDirection");
                
                var pointsManualProperty = serializedObject.FindProperty("m_PointsManual");

                this.AddEnumField(startPointPositionProperty);
                this.AddEnumField(endPointPositionProperty);
                this.AddPropertyField(startAndEndPointIterationsProperty);
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