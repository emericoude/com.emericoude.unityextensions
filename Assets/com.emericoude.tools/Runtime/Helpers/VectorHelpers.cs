using UnityEngine;

namespace Emericoude.Helpers
{
    public static class VectorHelpers
    {
        private const float TAU = Mathf.PI * 2f;
        
        /// <returns> The normalized direction to the point. <c>(to - from).normalized</c> </returns>
        public static Vector2 DirectionTo(this Vector2 from, Vector2 to) => (to - from).normalized;
        /// <returns> The normalized direction to the point. <c>(to - from).normalized</c> </returns>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => (to - from).normalized;
        
        
        /// <returns> True if the square distance between a and b is below tolerance; otherwise false. <br/><c>a.DistanceSqr(b) &lt;= tolerance</c> </returns>
        public static bool IsApproximately(this Vector2 a, Vector2 b, float tolerance = 0.0002f) => a.DistanceSqr(b) <= tolerance;
        /// <returns> True if the square distance between a and b is below tolerance; otherwise false. <br/><c>a.DistanceSqr(b) &lt;= tolerance</c> </returns>
        public static bool IsApproximately(this Vector3 a, Vector3 b, float tolerance = 0.0002f) => a.DistanceSqr(b) <= tolerance;
        
        
        /// <returns> The supplied vector, where each component is forced to be positive. </returns>
        public static Vector2 AbsoluteComponents(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        /// <returns> The supplied vector, where each component is forced to be positive. </returns>
        public static Vector3 AbsoluteComponents(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        
        
        /// <returns> The vector's X or Y component, whichever is largest. <br/><c>Mathf.Max(components)</c> </returns>
        public static float LargestComponent(this Vector2 vector) => Mathf.Max(vector.x, vector.y);
        /// <returns> The vector's X, Y or Z component, whichever is largest. <br/><c>Mathf.Max(components)</c> </returns>
        public static float LargestComponent(this Vector3 vector) => Mathf.Max(vector.x, vector.y, vector.z);
        
        /// <returns> The vector's X or Y component, whichever is smallest. <br/><c>Mathf.Min(components)</c> </returns>
        public static float SmallestComponent(this Vector2 vector) => Mathf.Min(vector.x, vector.y);
        /// <returns> The vector's X, Y or Z component, whichever is smallest. <br/><c>Mathf.Min(components)</c> </returns>
        public static float SmallestComponent(this Vector3 vector) => Mathf.Min(vector.x, vector.y, vector.z);
        
        /// <returns> The average of vector's X and Y components. <br/><c>(x + y) / 2f</c> </returns>
        public static float AverageComponents(this Vector2 vector) => (vector.x + vector.y) / 2.0f;
        /// <returns> The average of vector's X, Y and Z components. <br/><c>(x + y + z) / 3f</c> </returns>
        public static float AverageComponents(this Vector3 vector) => (vector.x + vector.y + vector.z) / 3.0f;
        
        
        /// <returns> True if a and b are 90° of each other. <br/><c>Mathf.Abs(Vector2.Dot(a, b)) &lt; Mathf.Epsilon</c>  </returns>
        public static bool IsPerpendicularTo(this Vector2 a, Vector2 b) => Mathf.Abs(Vector2.Dot(a, b)) < Mathf.Epsilon;
        /// <returns> True if a and b are 90° of each other. <br/><c>Mathf.Abs(Vector3.Dot(a, b)) &lt; Mathf.Epsilon</c> </returns>
        public static bool IsPerpendicularTo(this Vector3 a, Vector3 b) => Mathf.Abs(Vector3.Dot(a, b)) < Mathf.Epsilon;
        
        /// <returns> True if a and b are pointing in the same or opposite directions. <br/><c>Mathf.Abs(Vector2.Dot(a, b)) &gt; (1.0f - Mathf.Epsilon)</c> </returns>
        public static bool IsParallelTo(this Vector2 a, Vector2 b) => Mathf.Abs(Vector2.Dot(a, b)) > (1.0f - Mathf.Epsilon);
        /// <returns> True if a and b are pointing in the same or opposite directions. <br/><c>Mathf.Abs(Vector3.Dot(a, b)) &gt; (1.0f - Mathf.Epsilon)</c> </returns>
        public static bool IsParallelTo(this Vector3 a, Vector3 b) => Mathf.Abs(Vector3.Dot(a, b)) > (1.0f - Mathf.Epsilon);
        
        
        /// <returns> The distance between point a and point b, squared. <br/><c>(from - to).sqrMagnitude</c> </returns>
        /// <example> When checking if something is in range of another thing, it's more efficient to do <c>if (DistanceSqr(a, b) &lt; (range * range)) { }</c>,
        /// than it is to do <c>if (Distance(a, b) &lt; range) { }</c>. Albeit less readable. You can use <see cref="IsInRangeOf(UnityEngine.Vector2,UnityEngine.Vector2,float)"/>. </example>
        /// <remark> More efficient than <see cref="Vector2.Distance(Vector2, Vector2)"/> (as this avoids calculating the Root). </remark>
        public static float DistanceSqr(this Vector2 from, Vector2 to) => (from - to).sqrMagnitude;
        /// <returns> The distance between point a and point b, squared. <br/><c>(from - to).sqrMagnitude</c> </returns>
        /// <example> When checking if something is in range of another thing, it's more efficient to do <c>if (DistanceSqr(a, b) &lt; (range * range)) { }</c>,
        /// than it is to do <c>if (Distance(a, b) &lt; range) { }</c>. Albeit less readable. You can use <see cref="IsInRangeOf(UnityEngine.Vector3,UnityEngine.Vector3,float)"/>. </example>
        /// <remark> More efficient than <see cref="Vector3.Distance(Vector3, Vector3)"/> (as this avoids calculating the Root). </remark>
        public static float DistanceSqr(this Vector3 from, Vector3 to) => (from - to).sqrMagnitude;

        
        /// <returns> True if from or to are in range of each other (using the provided range radially); otherwise false. </returns>
        /// <remarks> This uses the <see cref="DistanceSqr(UnityEngine.Vector2,UnityEngine.Vector2)"/>, which can be more efficient than performing normal distance checks. </remarks>
        public static bool IsInRangeOf(this Vector2 from, Vector2 to, float range) => DistanceSqr(from, to) <= (range * range);
        /// <returns> True if from or to are in range of each other (using the provided range spherically); otherwise false. </returns>
        /// <remarks> This uses the <see cref="DistanceSqr(UnityEngine.Vector3,UnityEngine.Vector3)"/>, which can be more efficient than performing normal distance checks. </remarks>
        public static bool IsInRangeOf(this Vector3 from, Vector3 to, float range) => DistanceSqr(from, to) <= (range * range);
        
        
        /// <returns> If from is inside the range, then from; otherwise the nearest position on the edge of the range. </returns>
        public static Vector2 NearestPointInRange(this Vector2 from, Vector2 to, float radius) {
            float distanceSqr = from.DistanceSqr(to); //get the squared distance between the two points
            if (distanceSqr <= radius * radius) return from; //if we are in the range, simply return the position of from.
            float distance = Mathf.Sqrt(distanceSqr); //otherwise, calculate the distance between the two points
            Vector2 direction = from.DirectionTo(to); //get the direction between the two points
            return from + direction * distance; //return the position of from, but offset by the direction and distance between the two points
        }
        /// <returns> If from is inside the range, then from; otherwise the nearest position on the edge of the range. </returns>
        public static Vector3 NearestPointInRange(this Vector3 from, Vector3 to, float radius) {
            float distanceSqr = from.DistanceSqr(to); //get the squared distance between the two points
            if (distanceSqr <= radius * radius) return from; //if we are in the range, simply return the position of from.
            float distance = Mathf.Sqrt(distanceSqr); //otherwise, calculate the distance between the two points
            Vector3 direction = from.DirectionTo(to); //get the direction between the two points
            return from + direction * distance; //return the position of from, but offset by the direction and distance between the two points
        }
        
        
        /// <returns> The point on the edge of a circle around to, nearest to from. </returns>
        public static Vector2 NearestPointOnRangeEdge(this Vector2 from, Vector2 to, float radius) {
            float distance = Vector2.Distance(from, to) - radius;
            Vector2 direction = from.DirectionTo(to);
            return from + direction * distance;
        }
        /// <returns> The point on the surface of a sphere around to, nearest to from. </returns>
        public static Vector3 NearestPointOnRangeSurface(this Vector3 from, Vector3 to, float radius) {
            float distance = Vector3.Distance(from, to) - radius;
            Vector3 direction = from.DirectionTo(to);
            return from + direction * distance;
        }
        
        
        /// <returns> The nearest point to from. </returns>
        public static Vector2 NearestPoint(this Vector2 from, Vector2[] toPoints) {
            float nearestDistance = float.PositiveInfinity;
            Vector2 nearestPoint = Vector2.zero;
            foreach (Vector2 to in toPoints) {
                float distance = from.DistanceSqr(to);
                if (distance > nearestDistance) continue;
                nearestDistance = distance;
                nearestPoint = to;
            }
            return nearestPoint;
        }
        /// <returns> The nearest point to from. </returns>
        public static Vector3 NearestPoint(this Vector3 from, Vector3[] toPoints) {
            float nearestDistance = float.PositiveInfinity;
            Vector3 nearestPoint = Vector3.zero;
            foreach (Vector3 to in toPoints) {
                float distance = from.DistanceSqr(to);
                if (distance > nearestDistance) continue;
                nearestDistance = distance;
                nearestPoint = to;
            }
            return nearestPoint;
        }

        
        /// <summary> Basically direction.ToSegmentedDirection(4). </summary>
        /// <returns> The current direction snapped to the nearest orthogonal. </returns>
        public static Vector2 ToOrthogonalDirection(this Vector2 direction) => direction.ToSegmentedDirection(4);

        /// <summary> Basically direction.ToSegmentedDirection(8). </summary>
        /// <returns> The current direction snapped to the nearest orthogonal or diagonal. </returns>
        public static Vector2 ToOrthoDiagonalDirection(this Vector2 direction) => direction.ToSegmentedDirection(4);
        
        //TODO ToDiagonalDirection (we need a 4 + an rotation offset for this)

        /// <summary>
        /// Snaps the given direction to the nearest segmented direction.
        /// For instance, if you have 4 segments, it will be orthogonal only.
        /// If you have 8 segments, it will be orthogonal and diagonals.
        /// </summary>
        /// <returns> A clamped direction. </returns>
        /// TODO: initial rotation? basically improve this beyond the Ai generation... I should understand what's going on here
        /// TODO: 3D version?
        public static Vector2 ToSegmentedDirection(this Vector2 direction, int segments) {
            if (direction.sqrMagnitude < Mathf.Epsilon) return Vector2.zero; // if direction is zero, return zero
            direction = direction.normalized;

            // optimized path for 4 segments (i.e. orthogonal)
            if (segments == 4) {
                return Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                    ? new Vector2(Mathf.Sign(direction.x), 0)
                    : new Vector2(0, Mathf.Sign(direction.y));
            }

            //TODO: optimized path for 8
            
            float angle = Mathf.Atan2(direction.y, direction.x);
            float snappedAngle = Mathf.Round(angle / TAU * segments) * TAU / segments;

            // Return the normalized direction vector without rounding the components
            return new Vector2(
                Mathf.Cos(snappedAngle),
                Mathf.Sin(snappedAngle)
            );
        }
        
        
        //TODO: understand this
        //TODO: 2D version
        /// <summary> Determines where a vector (<paramref name="t"/>) stands between two points (<paramref name="a"/> and <paramref name="b"/>). </summary>
        /// <param name="a">The start of the range.</param>
        /// <param name="b">The end of the range.</param>
        /// <param name="t">The point within the range you want to calculate.</param>
        /// <returns>A value between 0 and 1, representing where <paramref name="t"/> falls between <paramref name="a"/> (0) and <paramref name="b"/> (1).</returns>
        public static float InverseLerp (Vector3 a, Vector3 b, Vector3 t)
        {
            Vector3 AB = b - a;
            Vector3 AV = t - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
        
        
        //TODO: 2D version
        /// <returns> The point rotated in space around the pivot by the given angles. </returns>
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 eulerAngles) => RotateAroundPivot(point, pivot, Quaternion.Euler(eulerAngles));

        //TODO: 2D version
        /// <returns> The point rotated in space around the pivot by the given rotation. </returns>
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion rotation) => pivot + rotation * (point - pivot);
        
        
        //TODO: I renamed this to specifically "Triangle" barycentric, but I have no clue if that's accurate lol...
        //sourced from https://discussions.unity.com/t/raycasthit-texturecoord-does-the-reverse-exist/36255/3
        /// <returns> The barycentric point (i.e. center of mass) from the given inputs.  </returns>
        public static Vector3 GetTriangleBarycentric (Vector2 v1,Vector2 v2,Vector2 v3,Vector2 p)
        {
            Vector3 B = new Vector3();
            B.x = ((v2.y - v3.y)*(p.x-v3.x) + (v3.x - v2.x)*(p.y - v3.y)) /
                  ((v2.y-v3.y)*(v1.x-v3.x) + (v3.x-v2.x)*(v1.y -v3.y));
            B.y = ((v3.y - v1.y)*(p.x-v3.x) + (v1.x - v3.x)*(p.y - v3.y)) /
                  ((v3.y-v1.y)*(v2.x-v3.x) + (v1.x-v3.x)*(v2.y -v3.y));
            B.z = 1 - B.x - B.y;
            return B;
        }
        
        //TODO: understand this
        //sourced from https://discussions.unity.com/t/raycasthit-texturecoord-does-the-reverse-exist/36255/3
        /// <returns> Whether the given barycentric point is inside a triangle. </returns>
        public static bool IsBarycentricInTriangle(Vector3 barycentric)
        {
            return (barycentric.x >= 0.0f) && (barycentric.x <= 1.0f)
                                           && (barycentric.y >= 0.0f) && (barycentric.y <= 1.0f)
                                           && (barycentric.z >= 0.0f); //(barycentric.z <= 1.0f)
        }
    }
}