using System;
using Emericoude.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Tests
{
    public class Test_Vector2Helpers : MonoBehaviour
    {
        public enum Test
        {
            None,
            SnapDirection,
            NearestPointOnCircleEdge
        }

        [Header("Test")]
        [SerializeField] private Test test = Test.None;
        [SerializeField] private Color inputColor = Color.blue;
        [SerializeField] private Color resultColor = Color.green;
        
        [Header("Snap direction")]
        [SerializeField] private int snapDirectionSegments = 3;
        
        [Header("Nearest point on circle edge")]
        [SerializeField] private float nearestPointOnCircleEdgeRadius = 1f;

        private new Camera camera => Camera.main;


        private void OnDrawGizmos() {
            if (test == Test.None) return;

            switch (this.test) {
                case Test.SnapDirection: this.OnDrawGizmos_SnapDirection(); break;
                case Test.NearestPointOnCircleEdge: this.OnDrawGizmos_NearestPointOnCircleEdge(); break;
                default: break;
            }
            
            GizmosHelpers.DrawArrow(Vector3.zero, new Vector3 (1f, 0.35f, 0.5f));
        }

        private void OnDrawGizmos_SnapDirection() {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 centerScreen = this.camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            
            Vector2 mouseDirectionFromCenter = (mousePosition - centerScreen).normalized;
            Vector2 snappedDirection = mouseDirectionFromCenter.ToSegmentedDirection(this.snapDirectionSegments);

            var distanceFromCamera = this.camera.transform.forward * (this.camera.nearClipPlane * 2f);
            var centerScreenToWorld = this.camera.ScreenToWorldPoint((Vector3)centerScreen + distanceFromCamera);
            var mousePositionToWorld = this.camera.ScreenToWorldPoint((Vector3)mousePosition + distanceFromCamera);

            Gizmos.color = inputColor;
            Gizmos.DrawSphere(mousePositionToWorld, 0.01f);
            Gizmos.DrawLine(centerScreenToWorld, mousePositionToWorld);
            Gizmos.color = resultColor;
            Gizmos.DrawLine(centerScreenToWorld, centerScreenToWorld + (Vector3)snappedDirection);
        }

        private void OnDrawGizmos_NearestPointOnCircleEdge() {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 centerScreen = this.camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            Vector2 nearestPointOnCircleEdge = mousePosition.NearestPointOnRangeEdge(centerScreen, this.nearestPointOnCircleEdgeRadius);
            
            var distanceFromCamera = this.camera.transform.forward * (this.camera.nearClipPlane * 2f);
            var centerScreenToWorld = this.camera.ScreenToWorldPoint((Vector3)centerScreen + distanceFromCamera);
            var mousePositionToWorld = this.camera.ScreenToWorldPoint((Vector3)mousePosition + distanceFromCamera);
            var nearestPointOnCircleEdgeToWorld = this.camera.ScreenToWorldPoint((Vector3)nearestPointOnCircleEdge + distanceFromCamera);
            float radiusToWorld = Vector3.Distance(centerScreenToWorld, nearestPointOnCircleEdgeToWorld);
            
            Gizmos.color = inputColor;
            Gizmos.DrawSphere(mousePositionToWorld, 0.01f);
            Gizmos.DrawWireSphere(centerScreenToWorld, radiusToWorld);
            Gizmos.color = resultColor;
            Gizmos.DrawLine(mousePositionToWorld, nearestPointOnCircleEdgeToWorld);
            Gizmos.DrawSphere(nearestPointOnCircleEdgeToWorld, 0.01f);
        }
    }
}
