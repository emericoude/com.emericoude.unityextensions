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
            NearestPointOnCircleEdge,
            QuadraticBezierCurve,
        }

        [Header("Test")]
        [SerializeField] private Test test = Test.None;
        [SerializeField] private Color inputColor = Color.blue;
        [SerializeField] private Color resultColor = Color.green;
        
        [Header("Snap direction")]
        [SerializeField] private int snapDirectionSegments = 3;
        
        [Header("Nearest point on circle edge")]
        [SerializeField] private float nearestPointOnCircleEdgeRadius = 1f;

        [Header("Quadratic Bezier Curve")]
        [SerializeField] private Vector2[] quadraticBezierCurvePoints;
        [SerializeField, Range(0f, 1f)] private float quadraticBezierT = 0.5f;
        
        private new Camera camera => Camera.main;
        
        private void OnDrawGizmos() {
            if (test == Test.None) return;

            switch (this.test) {
                case Test.SnapDirection: this.OnDrawGizmos_SnapDirection(); break;
                case Test.NearestPointOnCircleEdge: this.OnDrawGizmos_NearestPointOnCircleEdge(); break;
                case Test.QuadraticBezierCurve: this.OnDrawGizmos_QuadraticBezierCurve(); break;
                default: break;
            }
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

        private void OnDrawGizmos_QuadraticBezierCurve() {
            if (this.quadraticBezierCurvePoints.Length < 2) return;

            //draw the points / raw shape
            Gizmos.color = Color.black;
            for (int i = 0; i < this.quadraticBezierCurvePoints.Length; i++) {
                Gizmos.DrawSphere(this.quadraticBezierCurvePoints[i], 0.025f);
                if (i < this.quadraticBezierCurvePoints.Length - 1) {
                    Gizmos.DrawLine(this.quadraticBezierCurvePoints[i], this.quadraticBezierCurvePoints[i + 1]);
                }
            }
            
            //draw the curve
            Gizmos.color = Color.blue;
            for (int i = 0; i <= (this.quadraticBezierCurvePoints.Length + 1) % 4; i++) {
                Vector2 p0 = this.quadraticBezierCurvePoints[i];
                Vector2 p1 = this.quadraticBezierCurvePoints[i + 1];
                Vector2 p2 = this.quadraticBezierCurvePoints[i + 2];
                Vector2 bezierPoint = VectorHelpers.QuadraticBezier(p0, p1, p2, this.quadraticBezierT);
                Gizmos.DrawSphere(bezierPoint, 0.025f);
            }
        }
    }
}
