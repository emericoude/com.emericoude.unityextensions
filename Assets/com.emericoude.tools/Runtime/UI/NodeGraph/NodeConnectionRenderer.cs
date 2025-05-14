using System;
using Emericoude.Helpers;
using UnityEngine;
using ZLinq;
using Random = System.Random;

namespace Emericoude.UI.NodeGraph
{
    [Obsolete("Use UILineRendererController instead. This should be deleted.")]
    //BUG: Elbow does not work if an endpoint is set to center rect. Likely due to the direction calculation using the same point for from and to (i.e. the rect's center).
    [RequireComponent(typeof(UILineRenderer))]
    [RequireComponent(typeof(NodeConnection))]
    public class NodeConnectionRenderer : MonoBehaviour
    {
        public enum ElbowDirection
        {
            Auto,
            Up,
            Down,
            Left,
            Right,
        }
        
        public enum PointsDrawingFormation
        {
            Linear, //2 points (start, end)
            Elbow, //4 points (start, corner01, corner02, end)
            //TODO: As spline passthrough, i.e. calculate a spline from previous to this to the next, then draw the segment of the spline
            Manual //whatever you want
        }

        public enum ElbowCenterMethods
        {
            ZeroIsStart,
            ZeroIsNearestEndpoint
        }

        public enum EndpointPositioningMethod
        {
            RectCenter,
            RectOrthogonal,
            RectCorners,
            RectOrthogonalAndCorners,
            NearestPointOnRectEdge,
            Radial,
            RadialLong
        }

        [SerializeField] private NodeConnection m_Connection;
        [SerializeField] private UILineRenderer m_Line;

        [Tooltip("Doing iterations will make the connection look more natural, but will also make it take longer to draw. Generally, one or two is good enough.")]
        [SerializeField] private int m_StartAndEndPointIterations = 1;
        
        [SerializeField] private PointsDrawingFormation m_LineStyle = PointsDrawingFormation.Linear;
        public PointsDrawingFormation LineDrawingStyle {
            get => this.m_LineStyle;
            set {
                if (this.m_LineStyle == value) return;
                this.m_LineStyle = value;
                this.Redraw();
            }
        }
        
        [SerializeField] private EndpointPositioningMethod m_StartPointPosition = EndpointPositioningMethod.RectCenter;
        [SerializeField] private EndpointPositioningMethod m_EndPointPosition = EndpointPositioningMethod.RectCenter;

        [SerializeField] private ElbowCenterMethods m_ElbowCenterMethod = ElbowCenterMethods.ZeroIsStart;
        public ElbowCenterMethods ElbowCenterMethod {
            get => this.m_ElbowCenterMethod;
            set {
                if (this.m_ElbowCenterMethod == value) return;
                this.m_ElbowCenterMethod = value;
                this.Redraw();
            }
        }
        
        [SerializeField, Range(0.05f, 0.95f)] private float m_ElbowCenter = 0.5f;
        public float ElbowCenter {
            get => this.m_ElbowCenter;
            set {
                if (Mathf.Approximately(this.m_ElbowCenter, value)) return;
                this.m_ElbowCenter = value;
                this.Redraw();
            }
        }
        
        [SerializeField] private ElbowDirection m_FromElbowDirection = ElbowDirection.Auto;
        public ElbowDirection ElbowDirectionFrom {
            get => this.m_FromElbowDirection;
            set {
                if (this.m_FromElbowDirection == value) return;
                this.m_FromElbowDirection = value;
                this.Redraw();
            }
        }
        
        [SerializeField] private ElbowDirection m_ToElbowDirection = ElbowDirection.Auto;
        public ElbowDirection ElbowDirectionTo {
            get => this.m_ToElbowDirection;
            set {
                if (this.m_ToElbowDirection == value) return;
                this.m_ToElbowDirection = value;
                this.Redraw();
            }
        }
        

        [SerializeField] private Vector3[] m_PointsManual;
        public Vector3[] ManualPoints {
            get => this.m_PointsManual;
            set {
                this.m_PointsManual = value;
                this.Redraw();
            }
        }

        private void Reset() {
            this.m_Connection = this.GetComponent<NodeConnection>();
            this.m_Line = this.GetComponent<UILineRenderer>();
        }

        private void OnEnable() {
            this.m_Connection.OnFromChanged += this.OnConnectionsChanged;
            this.m_Connection.OnToChanged += this.OnConnectionsChanged;
            if (this.m_Connection.To != null) this.m_Connection.To.OnNodeMoved += this.OnNodeMoved;
            if (this.m_Connection.From != null) this.m_Connection.From.OnNodeMoved += this.OnNodeMoved;
        }

        private void OnDisable() {
            if (this.m_Connection == null) return;
            this.m_Connection.OnFromChanged -= this.OnConnectionsChanged;
            this.m_Connection.OnToChanged -= this.OnConnectionsChanged;
            if (this.m_Connection.To != null) this.m_Connection.To.OnNodeMoved -= this.OnNodeMoved;
            if (this.m_Connection.From != null) this.m_Connection.From.OnNodeMoved -= this.OnNodeMoved;
        }

        /*
        private void OnDrawGizmos() {
            if (this.m_Connection == null) return;
            if (this.m_Connection.From == null || this.m_Connection.To == null) return;
            var points = this.GetPoints();
            Gizmos.color = Color.blue;
            foreach (var point in points) {
                var pointToWorld = this.m_Line.UseWorldSpace ? point : this.transform.TransformPoint(point);
                Gizmos.DrawSphere(pointToWorld, 5f);
            }
        }
        */

        private void Start() {
            this.Redraw();
        }

        public void OnConnectionsChanged(Node oldNode, Node newNode) {
            if (oldNode != null) oldNode.OnNodeMoved -= this.OnNodeMoved;
            if (newNode != null) newNode.OnNodeMoved += this.OnNodeMoved;
            this.Redraw();
        }

        public void OnNodeMoved(Vector2 position) {
            this.Redraw();
        }

        public void Redraw() {
            if (this.m_Connection.From == null || this.m_Connection.To == null) {
                this.m_Line.SetPoints(Array.Empty<Vector3>());
                return;
            }
            
            this.m_Line.SetPoints(this.m_Line.UseWorldSpace
                ? this.GetPoints()
                : this.GetPoints().AsValueEnumerable().Select(p => this.m_Line.rectTransform.TransformPoint(p)).ToArray()
            );
        }

        private Vector3[] GetPoints() {
            return this.LineDrawingStyle switch {
                PointsDrawingFormation.Linear => this.GetPoints_Linear(),
                PointsDrawingFormation.Elbow => this.GetPoints_Elbow(),
                PointsDrawingFormation.Manual => this.GetPoints_Manual(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private Vector3[] GetPoints_Linear() {
            (Vector3 start, Vector3 end) = this.GetEndAndStartPointsThroughIteration();
            return new[] { start, end };
        }
        
        //TODO: elbow does not override the position, so it can be counter-intuitive when not using Auto.
        private Vector3[] GetPoints_Elbow() {
            (Vector3 start, Vector3 end) = this.GetEndAndStartPointsThroughIteration();
            Vector2 startDirection = this.ElbowDirectionFrom switch {
                ElbowDirection.Auto => ((Vector2)(start - this.GetNodeCenterPointWorld(this.m_Connection.From))).ToNearestOrthogonal(),
                ElbowDirection.Up => Vector2.up,
                ElbowDirection.Down => Vector2.down,
                ElbowDirection.Left => Vector2.left,
                ElbowDirection.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            Vector2 endDirection = this.ElbowDirectionTo switch {
                ElbowDirection.Auto => ((Vector2)(end - this.GetNodeCenterPointWorld(this.m_Connection.To))).ToNearestOrthogonal(),
                ElbowDirection.Up => Vector2.up,
                ElbowDirection.Down => Vector2.down,
                ElbowDirection.Left => Vector2.left,
                ElbowDirection.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            //we need only one corner while perpendicular
            if (startDirection.IsPerpendicularTo(endDirection)) {
                Vector3 alignedStart = startDirection * start;
                Vector3 alignedEnd = startDirection * end;
                float cornerDistanceFromStart = Vector3.Distance(alignedStart, alignedEnd);
                Vector3 corner = start + ((Vector3)startDirection * cornerDistanceFromStart);
                return new[] { start, corner, end };
            }

            bool isXOrYAligned = Mathf.Approximately(start.x, end.x) || Mathf.Approximately(start.y, end.y);
            if (isXOrYAligned) {
                return new[] { start, end };
            }
            else {
                Vector3 alignedStart = startDirection * start;
                Vector3 alignedEnd = startDirection * end;
                float alignedDirectionDistance = Vector3.Distance(alignedStart, alignedEnd);
                Vector3 elbowCorner01 = start + ((Vector3)startDirection * (alignedDirectionDistance * (this.ElbowCenter) ));
                Vector3 elbowCorner02 = end + ((Vector3)endDirection * (alignedDirectionDistance * (this.ElbowCenterMethod == ElbowCenterMethods.ZeroIsStart ? (1f - this.ElbowCenter) : (this.ElbowCenter))));
                return new[] { start, elbowCorner01, elbowCorner02, end };
            }
        }

        private Vector3[] GetPoints_Manual() {
            return this.ManualPoints;
        }

        private (Vector3 start, Vector3 end) GetEndAndStartPointsThroughIteration() {
            int iterations = this.m_StartAndEndPointIterations;
            if (this.m_StartPointPosition is EndpointPositioningMethod.RectCenter || this.m_EndPointPosition is EndpointPositioningMethod.RectCenter) {
                iterations = 1;
            }

            Vector3 startPoint = this.GetNodeCenterPointWorld(this.m_Connection.From);
            Vector3 endPoint = this.GetNodeCenterPointWorld(this.m_Connection.To);
            while (iterations > 0) {
                startPoint = this.GetEndpointWorld(endPoint, this.m_Connection.From, this.m_Connection.To, this.m_StartPointPosition);
                endPoint = this.GetEndpointWorld(startPoint, this.m_Connection.To, this.m_Connection.From, this.m_EndPointPosition);
                iterations--;
            }
            
            return (startPoint, endPoint);
        }
        
        private Vector3 GetNodeCenterPointWorld(Node node) => node.RectTransform.transform.position;
        private Vector3 GetEndpointWorld(Vector3 opposingEndpoint, Node source, Node target, EndpointPositioningMethod method) {
            return method switch {
                EndpointPositioningMethod.RectCenter => this.GetNodeCenterPointWorld(source),
                EndpointPositioningMethod.RectOrthogonal => source.RectTransform.TransformPoint(source.RectTransform.GetNearestOrthogonalOnEdge(opposingEndpoint)),
                EndpointPositioningMethod.RectCorners => source.RectTransform.TransformPoint(source.RectTransform.GetNearestCorner(opposingEndpoint)),
                EndpointPositioningMethod.RectOrthogonalAndCorners => source.RectTransform.TransformPoint(source.RectTransform.GetNearestCornerOrOrthogonalOnEdge(opposingEndpoint)),
                EndpointPositioningMethod.NearestPointOnRectEdge => source.RectTransform.TransformPoint(source.RectTransform.GetNearestPointOnEdge(opposingEndpoint)),
                EndpointPositioningMethod.Radial => source.RectTransform.TransformPoint((opposingEndpoint - source.RectTransform.position).normalized * (Mathf.Min(source.RectTransform.rect.width, source.RectTransform.rect.height) * 0.5f)),
                EndpointPositioningMethod.RadialLong => source.RectTransform.TransformPoint((opposingEndpoint - source.RectTransform.position).normalized * (Mathf.Max(source.RectTransform.rect.width, source.RectTransform.rect.height) * 0.71f)),
                _ => throw new NotImplementedException()
            };
        }
    }
}