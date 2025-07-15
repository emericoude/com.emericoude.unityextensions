using System;

using Emericoude.Helpers;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Tests
{
    public class GizmosTests : MonoBehaviour
    {
        public enum Test
        {
            Arrow,
            SegmentedDirections,
            Torus
        }
        
        [SerializeField] private Test test;
        
        [Header("Arrow")]
        [SerializeField] private Transform arrowSource;
        [SerializeField] private Transform arrowTarget;
        [SerializeField] private float arrowHeadLength = 0.1f;
        [SerializeField] private float arrowHeadAngle = 20f;

        [Header("Segmented Directions")]
        [SerializeField] private int segments = 4;
        [SerializeField, RadiansDisplayedAsDegrees] private float segmentedDirectionAngle = 0f;
        [SerializeField] private float segmentedDirectionLength = 1f;
        [SerializeField] private Vector2 directionInput;

        [Header("Torus")]
        [SerializeField] private int turns = 16;
        [SerializeField] private int resolutionPerTurn = 8;
        [SerializeField] private float minorRadius = 0.2f;
        [SerializeField] private float majorRadius = 1f;
        
        
        private void OnDrawGizmos() {
            if (this.test == Test.Arrow) {
                Gizmos.color = Color.green;
                GizmosHelpers.DrawArrow(this.arrowSource.position, this.arrowTarget.position, this.arrowHeadLength, this.arrowHeadAngle);
            }


            if (this.test == Test.SegmentedDirections) {
                Gizmos.color = Color.blue;
                GizmosHelpers.DrawSegmentedDirections(this.transform.position, this.transform.rotation, this.segments, this.segmentedDirectionAngle, this.segmentedDirectionLength);
                Gizmos.color = Color.yellow;
                Vector3 directionInputInMatrix = this.transform.localToWorldMatrix * this.directionInput.normalized;
                GizmosHelpers.DrawArrow(this.transform.position, this.transform.position + directionInputInMatrix, this.arrowHeadLength, this.arrowHeadAngle);
                Gizmos.color = Color.green;
                Vector3 directionSnappedInMatrix = this.transform.localToWorldMatrix * this.directionInput.ToSegmentedDirection(this.segments, this.segmentedDirectionAngle);
                GizmosHelpers.DrawArrow(this.transform.position, this.transform.position + directionSnappedInMatrix, this.arrowHeadLength, this.arrowHeadAngle);
            }

            if (this.test == Test.Torus) {
                Gizmos.color = Color.blue;
                GizmosHelpers.DrawTorusStrip(this.transform.position, this.transform.rotation, this.minorRadius, this.majorRadius, this.turns, this.resolutionPerTurn);
            }
            
        }
    }
}
