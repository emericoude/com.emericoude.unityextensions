using System;
using Emericoude.Helpers;
using UnityEngine;
using ZLinq;

namespace Emericoude.UI
{
    /// <summary>
    /// A controller for <see cref="UILineRenderer"/> to control common use cases.
    /// </summary>
    public class UILineRendererController : MonoBehaviour {
        
        public enum PointDrawingMethods {
            Linear,
            Elbow,
            Manual
        }

        public enum EndpointPositionMethods
        {
            Center,
            EdgeOrthogonal,
            Corners,
            EdgeOrthogonalOrCorners,
            EdgeNearest,
            EdgeNearestAsRadial,
            EdgeNearestAsRadialLong
        }
        
        public enum ElbowEndpointDirections {
            Auto,
            Up, Down, Left, Right
        }

        public enum ElbowPositionMethods
        {
            ZeroIsStart,
            ZeroIsNearestEndpoint
        }

        public RectTransform FromRect
        {
            get => this.m_From;
            set {
                this.m_From = value;
                this.fromPosition = this.m_From.localPosition;
            }
        }

        public RectTransform ToRect {
            get => this.m_To; 
            set {
                this.m_To = value;
                this.toPosition = this.m_To.localPosition;
            }
        }

        [Tooltip("The line renderer component.")]
        [SerializeField] private UILineRenderer m_Line;
        [Tooltip("The rect transform used as the start point for the line.")]
        [SerializeField] private RectTransform m_From;
        [Tooltip("The rect transform used as the end point for the line.")]
        [SerializeField] private RectTransform m_To;

        //TODO: THESE TWO SETTINGS
        [Tooltip("Draw the line on start.")]
        [SerializeField] private bool m_DrawOnStart = true;
        [Tooltip("Redraw the line whenever the from or to rects move (locally). Evaluated during Late Update.")]
        [SerializeField] private bool m_RedrawIfEndpointMoves = true;
        [Tooltip("If the from rect or to rect is null, destroy self. Evaluated during Late Update.")]
        [SerializeField] private bool m_DestroySelfIfEndpointIsNull = true;
        
        [Tooltip("Use this to increase accuracy at the expense of performance, though generally one or two will be fine.")]
        [SerializeField] private int m_EndpointsPositionIterations = 1;
        [Tooltip("In which form the points will be fed to the line renderer.")]
        [SerializeField] private PointDrawingMethods m_PointStyle = PointDrawingMethods.Linear;
        [Tooltip("Where the start point will be placed (in relation to the from rect transform).")]
        [SerializeField] private EndpointPositionMethods m_StartPositionMethod = EndpointPositionMethods.Center;
        [Tooltip("Where the end point will be placed (in relation to the to rect transform).")]
        [SerializeField] private EndpointPositionMethods m_EndPositionMethod = EndpointPositionMethods.Center;

        [Tooltip("How we use the elbow center property." +
                 "\n\n  - ZeroIsStart means 0 is the start point, 0.5 is the middle and 1 is the end point." +
                 "\n\n  - ZeroIsNearestEndpoint means that the center is evaluate per endpoint. So 0.25 is 0.25 from the end for the second corner (whereas it would've been 0.75 from the end with the other method). Useful when feeding into a smooth curve and you don't want it to overshoot.")]
        [SerializeField] private ElbowPositionMethods m_ElbowPositionMethod = ElbowPositionMethods.ZeroIsStart;
        [Tooltip("The placement of the elbow(s)/corner(s). This value's usage depends on the ElbowPositionMethod.")]
        [SerializeField, Range(0.01f, 0.99f)] private float m_ElbowCenter = 0.5f;
        [Tooltip("The position from which the elbow will come out. Best left on auto.")]
        [SerializeField] private ElbowEndpointDirections m_ElbowFromDirection = ElbowEndpointDirections.Auto;
        [Tooltip("The position from which the elbow will come out. Best left on auto.")]
        [SerializeField] private ElbowEndpointDirections m_ElbowToDirection = ElbowEndpointDirections.Auto;

        [Tooltip("The points you can set manually. Consider not using this component at all when using manual, as it will simply set your points into the UILineRenderer.")]
        [SerializeField] private Vector3[] m_ManualPoints;
        
        private Vector3 fromPosition;
        private Vector3 toPosition;

        private void Reset()
        {
            this.m_Line = this.GetComponent<UILineRenderer>();
        }

        private void Start()
        {
            if (this.m_From != null) this.fromPosition = m_From.localPosition;
            if (this.m_To != null) this.toPosition = m_To.localPosition;
            
            if (this.m_DrawOnStart)
            {
                this.RedrawPoints();
            }
        }

        private void LateUpdate()
        {
            if (this.m_DestroySelfIfEndpointIsNull)
            {
                if (m_From == null || m_To == null)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
            
            if (this.m_RedrawIfEndpointMoves)
            {
                if (this.fromPosition != this.m_From.localPosition || this.toPosition != this.m_To.localPosition)
                {
                    this.fromPosition = this.m_From.localPosition;
                    this.fromPosition = this.m_To.localPosition;
                    this.RedrawPoints();
                }
            }
        }

        public void RedrawPoints()
        {
            if (this.m_PointStyle != PointDrawingMethods.Manual
                && (this.m_From == null || this.m_To == null)) {
                this.m_Line.SetPoints(Array.Empty<Vector3>());
                return;
            }
            
            this.m_Line.SetPoints(this.m_Line.UseWorldSpace
                ? this.GetPoints()
                : this.GetPoints().AsValueEnumerable().Select(p => this.m_Line.rectTransform.TransformPoint(p)).ToArray()
            );
        }

        private Vector3[] GetPoints()
        {
            return this.m_PointStyle switch
            {
                PointDrawingMethods.Linear => this.GetPoints_Linear(),
                PointDrawingMethods.Elbow => this.GetPoints_Elbow(),
                PointDrawingMethods.Manual => this.GetPoints_Manual(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Vector3[] GetPoints_Linear()
        {
            (Vector3 start, Vector3 end) = this.GetEndpoints();
            return new[] { start, end };
        }

        private Vector3[] GetPoints_Elbow()
        {
            (Vector3 start, Vector3 end) = this.GetEndpoints();
            Vector2 startDirection = this.m_ElbowFromDirection switch {
                ElbowEndpointDirections.Auto => ((Vector2)(start - this.m_From.position)).ToNearestOrthogonal(),
                ElbowEndpointDirections.Up => Vector2.up,
                ElbowEndpointDirections.Down => Vector2.down,
                ElbowEndpointDirections.Left => Vector2.left,
                ElbowEndpointDirections.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            Vector2 endDirection = this.m_ElbowToDirection switch {
                ElbowEndpointDirections.Auto => ((Vector2)(end - this.m_To.position)).ToNearestOrthogonal(),
                ElbowEndpointDirections.Up => Vector2.up,
                ElbowEndpointDirections.Down => Vector2.down,
                ElbowEndpointDirections.Left => Vector2.left,
                ElbowEndpointDirections.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException()
            };

            //backup, usually occurs when using center position as endpoint
            if (startDirection == Vector2.zero) startDirection = ((Vector2)(end - start)).ToNearestOrthogonal();
            if (endDirection == Vector2.zero) endDirection = ((Vector2)(start - end)).ToNearestOrthogonal();
            
            //we need only one corner while perpendicular
            if (startDirection.IsPerpendicularTo(endDirection)) {
                Vector3 alignedStartPerp = startDirection * start;
                Vector3 alignedEndPerp = startDirection * end;
                float cornerDistanceFromStart = Vector3.Distance(alignedStartPerp, alignedEndPerp);
                Vector3 corner = start + ((Vector3)startDirection * cornerDistanceFromStart);
                return new[] { start, corner, end };
            }
            
            //If they are aligned at this point, the line is basically straight
            bool isXOrYAligned = Mathf.Approximately(start.x, end.x) || Mathf.Approximately(start.y, end.y);
            if (isXOrYAligned) return new[] { start, end };
            
            //calculate the position of both corners
            Vector3 alignedStart = startDirection * start;
            Vector3 alignedEnd = startDirection * end;
            float alignedDirectionDistance = Vector3.Distance(alignedStart, alignedEnd);
            Vector3 elbowCorner01 = start + ((Vector3)startDirection * (alignedDirectionDistance * (this.m_ElbowCenter) )); //first corner is always a position away from the start point, so no need for special checks
            Vector3 elbowCorner02 = end + ((Vector3)endDirection * (alignedDirectionDistance * (this.m_ElbowPositionMethod == ElbowPositionMethods.ZeroIsStart ? (1f - this.m_ElbowCenter) : (this.m_ElbowCenter))));
            return new[] { start, elbowCorner01, elbowCorner02, end };
        }

        private Vector3[] GetPoints_Manual()
        {
            return this.m_ManualPoints;
        }

        private (Vector3 start, Vector3 end) GetEndpoints()
        {
            int iterations = this.m_EndpointsPositionIterations;
            if (this.m_StartPositionMethod is EndpointPositionMethods.Center
                || this.m_EndPositionMethod is EndpointPositionMethods.Center) {
                //if one of the point is center, it'll always be as accurate as can be on the first iteration, so we don't need to run it again
                iterations = 1;
            }

            //As a general note, we are working in world-space as the math is easier.
            Vector3 start = this.m_From.position;
            Vector3 end = this.m_To.position;
            while (iterations > 0)
            {
                start = this.GetEndpointIterative(end, this.m_From, this.m_To, this.m_StartPositionMethod);
                end = this.GetEndpointIterative(start, this.m_To, this.m_From, this.m_EndPositionMethod);
                iterations--;
            }

            return (start, end);
        }

        private Vector3 GetEndpointIterative(Vector3 opposingPoint, RectTransform from, RectTransform to, EndpointPositionMethods method)
        {
            Vector3 localPoint = method switch {
                EndpointPositionMethods.Center => Vector3.zero,
                EndpointPositionMethods.EdgeOrthogonal => from.GetNearestOrthogonalOnEdge(opposingPoint),
                EndpointPositionMethods.Corners => from.GetNearestCorner(opposingPoint),
                EndpointPositionMethods.EdgeOrthogonalOrCorners => from.GetNearestCornerOrOrthogonalOnEdge(opposingPoint),
                EndpointPositionMethods.EdgeNearest => from.GetNearestPointOnEdge(opposingPoint),
                EndpointPositionMethods.EdgeNearestAsRadial => (opposingPoint  - from.position).normalized * (Mathf.Min(from.rect.width, from.rect.height) * 0.5f),
                EndpointPositionMethods.EdgeNearestAsRadialLong => (opposingPoint - from.position).normalized * (Mathf.Max(from.rect.width, from.rect.height) * 0.71f),
                _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
            };

            //TODO: there's probably a way to avoid this transforming points back and forth, but I can't be bothered with the math...
            return from.TransformPoint(localPoint);
        }
    }
}