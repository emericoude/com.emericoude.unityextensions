//inspired by https://discussions.unity.com/t/new-ui-and-line-drawing/542009/63

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using ZLinq;

namespace Emericoude.UI
{
    //TODO: CUSTOM EDITOR
    /// <summary>
    /// A line renderer for uGUI.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    public class UILineRenderer : MaskableGraphic
    {
        private const float MINIMUM_MITER_NICE = 15.0f * Mathf.Deg2Rad;
        private const float MINIMUM_BEVEL_NICE = 30.0f * Mathf.Deg2Rad;
        private const int MAX_VERTEX_COUNT = 64000; //some people seem to suggest its 65534... not sure which is right
        
        private enum SegmentType { Start, Middle, End }
        public enum CornerType { None, Bevel, Miter }
        public enum LineCapsPosition { None, Start, End, Both }
        
        private Vector2[] startUvs, middleUvs, endUvs;
        
        [SerializeField] private Vector3[] m_Points;
        
        [SerializeField] private Sprite m_Sprite;
        public Sprite Sprite {
            get => m_Sprite;
            set {
                if (this.m_Sprite == value) return;
                this.m_Sprite = value;
                this.SetAllDirty();
            }
        }

        [SerializeField] private bool m_UseWorldSpace = false;
        public bool UseWorldSpace {
            get => this.m_UseWorldSpace;
            set {
                if (this.m_UseWorldSpace == value) return;
                this.m_UseWorldSpace = value;
                this.SetAllDirty();
            }
        }

        [SerializeField] private float m_LineThickness = 10f;
        public float LineThickness {
            get => this.m_LineThickness;
            set {
                this.m_LineThickness = value;
                this.SetAllDirty();
            }
        }

        [SerializeField] private CornerType m_CornerType = CornerType.Miter;
        public CornerType JointType {
            get => this.m_CornerType;
            set {
                if (this.m_CornerType == value) return;
                this.m_CornerType = value;
                this.SetAllDirty();
            }
        }
        
        [SerializeField] private bool m_DrawWithSpline = true;
        public bool DrawWithSpline {
            get => this.m_DrawWithSpline;
            set {
                if (this.m_DrawWithSpline == value) return;
                this.m_DrawWithSpline = value;
                this.SetAllDirty();
            }
        }
        
        //TODO: option to reference a spline component or something of the sort?
        
        [SerializeField] private int m_SplineResolution = 32;
        public int SplineResolution {
            get => this.m_SplineResolution;
            set {
                if (this.m_SplineResolution == value) return;
                this.m_SplineResolution = value;
                this.SetAllDirty();
            }
        }
        
        [SerializeField] private TangentMode m_SplineTangentMode = TangentMode.AutoSmooth;
        public TangentMode SplineTangentMode {
            get => this.m_SplineTangentMode;
            set {
                if (this.m_SplineTangentMode == value) return;
                this.m_SplineTangentMode = value;
                this.SetAllDirty();
            }
        }

        //TODO: Cap sprites
        [SerializeField] private LineCapsPosition m_LineCaps = LineCapsPosition.None;
        public LineCapsPosition LineCaps {
            get => this.m_LineCaps;
            set {
                if (this.m_LineCaps == value) return;
                this.m_LineCaps = value;
                this.SetAllDirty();
            }
        }

        protected UILineRenderer() {
            useLegacyMeshGeneration = false;
        }

        public void SetPoints(Vector3[] points) {
            this.m_Points = points;
            this.SetAllDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh) {
            if (this.m_Points == null || this.m_Points.Length < 2) return;
            
            this.GenerateUVs();
            vh.Clear();

            var pointsToDraw = this.UseWorldSpace
                ? this.m_Points.AsValueEnumerable().Select(p => (Vector2)this.transform.InverseTransformPoint(p)).ToArray()
                : this.m_Points.AsValueEnumerable().Select(p => (Vector2)p).ToArray();
            
            if (this.DrawWithSpline) {
                Spline spline = new Spline(
                    pointsToDraw
                        .AsValueEnumerable()
                        .Select(v2 => new float3(v2.x, v2.y, 0f))
                        .AsEnumerable(), 
                    this.SplineTangentMode
                );

                List<float3> splinePoints = new List<float3>();
                for (int i = 0; i < this.SplineResolution; i++) {
                    splinePoints.Add(spline.EvaluatePosition(Mathf.InverseLerp(0, this.SplineResolution - 1, i))); 
                }
                pointsToDraw = splinePoints.AsValueEnumerable().Select(v3 => new Vector2(v3.x, v3.y)).ToArray();
            }
            
            //create necessary segments
            var segments = new List<UIVertex[]>();
            Vector2 offset = -(this.rectTransform.pivot);
            for (int i = 1; i < pointsToDraw.Length; i++) {
                var start = pointsToDraw[i - 1] + offset;
                var end = pointsToDraw[i] + offset;

                if (i == 1 && this.LineCaps is LineCapsPosition.Both or LineCapsPosition.Start) {
                    segments.Add(this.CreateLineCap(start, end, SegmentType.Start));
                }
                
                segments.Add(this.CreateLineSegment(start, end, SegmentType.Middle));
                
                if (i == pointsToDraw.Length - 1 && this.LineCaps is LineCapsPosition.Both or LineCapsPosition.End) {
                    segments.Add(this.CreateLineCap(start, end, SegmentType.End));
                }
            }
            
            //TODO: This doesn't really work that well with splines imo...
            // I feel like we can do better math to have triangles fill joints without overlap
            //TODO: not gonna lie, I'd prefer if corners could be rounded, though maybe then just use bezier...
            //modify segments to fill corners if possible, and then add the to the vertex helper
            for (int i = 0; i < segments.Count; i++) {
                if (i < segments.Count - 1 && this.m_CornerType != CornerType.None) {
                    //bunch of math, effectively to calculate joints
                    Vector3 currentSegmentPos = segments[i][1].position - segments[i][2].position;
                    Vector3 nextSegmentPos = segments[i + 1][2].position - segments[i + 1][1].position;
                    float angleBetweenSegments = Vector2.Angle(currentSegmentPos, nextSegmentPos) * Mathf.Deg2Rad;
                    float sign = Mathf.Sign(Vector3.Cross(currentSegmentPos.normalized, nextSegmentPos.normalized).z);
                    float miterDistance = this.LineThickness / (2f * Mathf.Tan(angleBetweenSegments / 2f));
                    Vector3 miterPointA = segments[i][2].position - currentSegmentPos.normalized * miterDistance * sign;
                    Vector3 miterPointB = segments[i][3].position + currentSegmentPos.normalized * miterDistance * sign;

                    bool miterLooksGood(float minAngleToConsiderNice) {
                        return miterDistance < (currentSegmentPos.magnitude / 2.0f) 
                            && miterDistance < (nextSegmentPos.magnitude / 2.0f)
                            && angleBetweenSegments > minAngleToConsiderNice;
                    }
                    
                    if (this.JointType == CornerType.Miter && miterLooksGood(MINIMUM_MITER_NICE)) {
                        segments[i][2].position = miterPointA;
                        segments[i][3].position = miterPointB;
                        segments[i + 1][0].position = miterPointB;
                        segments[i + 1][1].position = miterPointA;
                    }
                    else { //using Bevel corner type
                        if (miterLooksGood(MINIMUM_BEVEL_NICE)) {
                            if (sign < 0f) { // Positive sign is Clockwise
                                segments[i][2].position = miterPointA;
                                segments[i + 1][1].position = miterPointA;
                            }
                            else {
                                segments[i][3].position = miterPointB;
                                segments[i + 1][0].position = miterPointB;
                            }
                        }
                        
                        // add corner
                        vh.AddUIVertexQuad(new UIVertex[] {
                            segments[i][2], 
                            segments[i][3], 
                            segments[i + 1][0], 
                            segments[i + 1][1]
                        });
                    }
                }
                
                // add segment
                vh.AddUIVertexQuad(segments[i]);
            }

            if (vh.currentVertCount > MAX_VERTEX_COUNT) {
                Debug.LogError($"Max vertices exceeded by {vh.currentVertCount - MAX_VERTEX_COUNT} (current: {vh.currentVertCount}, max: {MAX_VERTEX_COUNT}). The line will not be drawn.", this);
                vh.Clear();
            }
        }

        //TODO: Caps look awful, need a way to add a resolution to them so they can be rounded...
        private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, SegmentType type) {
            return type switch {
                SegmentType.Start => this.CreateLineSegment(
                    start - ((end - start).normalized * this.LineThickness / 2f),
                    start,
                    type
                ),
                SegmentType.End => this.CreateLineSegment(
                    end,
                    end + ((end - start).normalized * this.LineThickness / 2f),
                    type
                ),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, SegmentType type) {
            Vector2 offset = new Vector2(start.y - end.y, end.x - start.x).normalized * this.LineThickness / 2f;
            return this.SetVertexBufferObject(
                new Vector2[4] {
                    start - offset,
                    start + offset,
                    end + offset,
                    end - offset
                },
                type switch {
                    SegmentType.Start => this.startUvs,
                    SegmentType.Middle => this.middleUvs,
                    SegmentType.End => this.endUvs,
                    _ => throw new ArgumentOutOfRangeException()
                }
            );
        }

        private UIVertex[] SetVertexBufferObject(Vector2[] vertices, Vector2[] uvs) {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++) {
                var vert = UIVertex.simpleVert;
                vert.color = this.color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }

        private void GenerateUVs() {
            Vector2 topLeft, bottomLeft, topCenterLeft, topCenterRight, bottomCenterLeft, bottomCenterRight, topRight, bottomRight;
            
            if (this.Sprite == null) {
                topLeft = Vector2.zero;
                bottomLeft = new Vector2(0, 1);
                topCenterLeft = new Vector2(0.5f, 0);
                topCenterRight = new Vector2(0.5f, 0);
                bottomCenterLeft = new Vector2(0.5f, 1);
                bottomCenterRight = new Vector2(0.5f, 1);
                topRight = new Vector2(1, 0);
                bottomRight = Vector2.one;
            }
            else {
                var outer = UnityEngine.Sprites.DataUtility.GetOuterUV(this.Sprite);
                var inner = UnityEngine.Sprites.DataUtility.GetInnerUV(this.Sprite);
                topLeft = new Vector2(outer.x, outer.y);
                bottomLeft = new Vector2(outer.x, outer.w);
                topCenterLeft = new Vector2(inner.x, inner.y);
                topCenterRight = new Vector2(inner.z, inner.y);
                bottomCenterLeft = new Vector2(inner.x, inner.w);
                bottomCenterRight = new Vector2(inner.z, inner.w);
                topRight = new Vector2(outer.z, outer.y);
                bottomRight = new Vector2(outer.z, outer.w);
            }
            
            this.startUvs = new[] { topLeft, bottomLeft, bottomCenterLeft, topCenterLeft };
            this.middleUvs = new[] { topCenterLeft, bottomCenterLeft, bottomCenterRight, topCenterRight };
            this.endUvs = new[] { topCenterRight, bottomCenterRight, bottomRight, topRight };
        }
        
        #region Maskable Graphic Overrides

        public override Texture mainTexture {
            get {
                if (this.Sprite != null) return Sprite.texture;
                if (this.material != null && this.material.mainTexture != null) return this.material.mainTexture;
                return base.mainTexture;
            }
        }
        
        public override Material material {
            get {
                if (this.m_Material != null) return this.m_Material;
                if (this.Sprite != null && this.Sprite.associatedAlphaSplitTexture != null) return ETC1SupportedCanvasMaterial;
                return base.defaultMaterial;
            }
        }
        
        private static Material s_ETC1SupportedCanvasMaterial;
        private static Material ETC1SupportedCanvasMaterial {
            get {
                if (s_ETC1SupportedCanvasMaterial == null) {
                    s_ETC1SupportedCanvasMaterial = Canvas.GetETC1SupportedCanvasMaterial();
                }
                
                return s_ETC1SupportedCanvasMaterial;
            }
        }

        #endregion
    }
}
