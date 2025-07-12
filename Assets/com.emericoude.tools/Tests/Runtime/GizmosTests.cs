using System;

using Emericoude.Helpers;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Emericoude.Tests
{
    public class GizmosTests : MonoBehaviour
    {
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
        
        private void OnDrawGizmos() {
            //Draw an arrow from to
            Gizmos.color = Color.green;
            GizmosHelpers.DrawArrow(this.arrowSource.position, this.arrowTarget.position, this.arrowHeadLength, this.arrowHeadAngle);

            //draw the segmented directions, an input direction, and the result of snapping it
            Gizmos.color = Color.blue;
            GizmosHelpers.DrawSegmentedDirections(this.transform.position, this.transform.rotation, this.segments, this.segmentedDirectionAngle, this.segmentedDirectionLength);
            Gizmos.color = Color.yellow;
            Vector3 directionInputInMatrix = this.transform.localToWorldMatrix * this.directionInput.normalized;
            GizmosHelpers.DrawArrow(this.transform.position, this.transform.position + directionInputInMatrix, this.arrowHeadLength, this.arrowHeadAngle);
            Gizmos.color = Color.green;
            Vector3 directionSnappedInMatrix = this.transform.localToWorldMatrix * this.directionInput.ToSegmentedDirection(this.segments, this.segmentedDirectionAngle);
            GizmosHelpers.DrawArrow(this.transform.position, this.transform.position + directionSnappedInMatrix, this.arrowHeadLength, this.arrowHeadAngle);
        }
    }
}
