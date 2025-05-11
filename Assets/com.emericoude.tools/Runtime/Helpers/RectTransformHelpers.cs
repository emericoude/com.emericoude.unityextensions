using UnityEngine;

//very good resource: https://2dengine.com/doc/intersections.html

namespace Emericoude.Helpers
{
    public static class RectTransformHelpers
    {
        public static Vector2 GetNearestOrthogonalOnEdge(this RectTransform rectTransform, Vector3 point, bool isPointInWorldSpace = true) {
            Vector2 localPoint = isPointInWorldSpace ? rectTransform.InverseTransformPoint(point) : (Vector2)point;
            Rect rect = rectTransform.rect;
            float halfHeight = rect.height / 2f;
            float halfWidth = rect.width / 2f;
            return localPoint.GetNearestPoint(new []{
                rect.center + Vector2.up * halfHeight,
                rect.center + Vector2.down * halfHeight,
                rect.center + Vector2.right * halfWidth,
                rect.center + Vector2.left * halfWidth
            });
        }

        public static Vector2 GetNearestCorner(this RectTransform rectTransform, Vector3 point, bool isPointInWorldSpace = true) {
            Vector2 localPoint = isPointInWorldSpace ? rectTransform.InverseTransformPoint(point) : (Vector2)point;
            Rect rect = rectTransform.rect;
            return localPoint.GetNearestPoint(new []{
                rect.min,
                new Vector2(rect.min.x, rect.max.y),
                new Vector2(rect.max.x, rect.min.y),
                rect.max
            });
        }

        public static Vector2 GetNearestCornerOrOrthogonalOnEdge(this RectTransform rectTransform, Vector3 point, bool isPointInWorldSpace = true) {
            Vector2 localPoint = isPointInWorldSpace ? rectTransform.InverseTransformPoint(point) : (Vector2)point;
            Rect rect = rectTransform.rect;
            float halfHeight = rect.height / 2f;
            float halfWidth = rect.width / 2f;
            return localPoint.GetNearestPoint(new []{
                //orthogonals
                rect.center + Vector2.up * halfHeight,
                rect.center + Vector2.down * halfHeight,
                rect.center + Vector2.right * halfWidth,
                rect.center + Vector2.left * halfWidth,
                //corners, we multiply them by 71 to basically normalize them as directions... it's dumb, but it works
                rect.center + (Vector2.up * halfHeight + Vector2.left * halfWidth) * 0.71f,
                rect.center + (Vector2.up * halfHeight + Vector2.right * halfWidth) * 0.71f,
                rect.center + (Vector2.down * halfHeight + Vector2.left * halfWidth) * 0.71f,
                rect.center + (Vector2.down * halfHeight + Vector2.right * halfWidth) * 0.71f,
            });
        }
        
        /// <returns> The nearest position to the given point, on the edge of the rect. </returns>
        public static Vector2 GetNearestPointOnEdge(this RectTransform rectTransform, Vector3 point, bool isPointInWorldSpace = true) {
            Vector2 localPoint = isPointInWorldSpace ? rectTransform.InverseTransformPoint(point) : (Vector2)point;
            Rect rect = rectTransform.rect;
            
            float halfHeight = rect.height / 2f;
            float halfWidth = rect.width / 2f;
            var qx = localPoint.x - rect.center.x;
            var qy = localPoint.y - rect.center.y;

            if (qx > halfWidth) qx = halfWidth;
            else if (qx < -halfWidth) qx = -halfWidth;
            
            if (qy > halfHeight) qy = halfHeight;
            else if (qy < -halfHeight) qy = -halfHeight;
            
            return new Vector2(qx, qy);
        }
    }
}