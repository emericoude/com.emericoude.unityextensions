using UnityEngine;

namespace Emericoude.Helpers
{
    public static class VectorHelpers
    {
        #region Determinant

        /// <summary> Similar to the Dot product, but instead of how much two vectors point in the same direction, it's how much it points left or right. </summary>
        /// <returns> The determinant of the two vectors (i.e. how much is a point to the right or left of b). <br/><c>a.x * b.y - a.y * b.x</c> </returns>
        public static float Determinant(this Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

        #endregion
        #region Direction To

        /// <returns> The normalized direction to the point. <c>(to - from).normalized</c> </returns>
        public static Vector2 DirectionTo(this Vector2 from, Vector2 to) => (to - from).normalized;
        /// <returns> The normalized direction to the point. <c>(to - from).normalized</c> </returns>
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to) => (to - from).normalized;

        #endregion
        #region Is Approximately

        /// <returns> True if the square distance between a and b is below tolerance; otherwise false. <br/><c>a.DistanceSqr(b) &lt;= tolerance</c> </returns>
        public static bool IsApproximately(this Vector2 a, Vector2 b, float tolerance = 0.0002f) => a.DistanceSqr(b) <= tolerance;
        /// <returns> True if the square distance between a and b is below tolerance; otherwise false. <br/><c>a.DistanceSqr(b) &lt;= tolerance</c> </returns>
        public static bool IsApproximately(this Vector3 a, Vector3 b, float tolerance = 0.0002f) => a.DistanceSqr(b) <= tolerance;

        #endregion
        #region Absolute

        /// <returns> The supplied vector, where each component is forced to be positive. </returns>
        public static Vector2 AbsoluteComponents(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        /// <returns> The supplied vector, where each component is forced to be positive. </returns>
        public static Vector3 AbsoluteComponents(this Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

        #endregion
        #region Get Smallest/Largest/Average Component

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

        #endregion
        #region Is Perpendicular To / Is Parallel To

        /// <returns> True if a and b are 90° of each other. <br/><c>Mathf.Abs(Vector2.Dot(a, b)) &lt; Mathf.Epsilon</c>  </returns>
        public static bool IsPerpendicularTo(this Vector2 a, Vector2 b) => Mathf.Abs(Vector2.Dot(a, b)) < Mathf.Epsilon;
        /// <returns> True if a and b are 90° of each other. <br/><c>Mathf.Abs(Vector3.Dot(a, b)) &lt; Mathf.Epsilon</c> </returns>
        public static bool IsPerpendicularTo(this Vector3 a, Vector3 b) => Mathf.Abs(Vector3.Dot(a, b)) < Mathf.Epsilon;
        
        /// <returns> True if a and b are pointing in the same or opposite directions. <br/><c>Mathf.Abs(Vector2.Dot(a, b)) &gt; (1.0f - Mathf.Epsilon)</c> </returns>
        public static bool IsParallelTo(this Vector2 a, Vector2 b) => Mathf.Abs(Vector2.Dot(a, b)) > (1.0f - Mathf.Epsilon);
        /// <returns> True if a and b are pointing in the same or opposite directions. <br/><c>Mathf.Abs(Vector3.Dot(a, b)) &gt; (1.0f - Mathf.Epsilon)</c> </returns>
        public static bool IsParallelTo(this Vector3 a, Vector3 b) => Mathf.Abs(Vector3.Dot(a, b)) > (1.0f - Mathf.Epsilon);

        #endregion
        #region Distance Squared / Is In Range Of
        
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

        #endregion
        #region Nearest Point
        
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

        #endregion
        #region Segmented Directions
        
        /// <summary> Basically direction.ToSegmentedDirection(4). </summary>
        /// <returns> The direction snapped to the nearest orthogonal. </returns>
        public static Vector2 ToOrthogonalDirection(this Vector2 direction) => direction.ToSegmentedDirection(4);

        /// <summary> Basically direction.ToSegmentedDirection(8). </summary>
        /// <returns> The direction snapped to the nearest orthogonal or diagonal. </returns>
        public static Vector2 ToOrthoDiagonalDirection(this Vector2 direction) => direction.ToSegmentedDirection(4);

        /// <summary> Basically direction.ToSegmentedDirection(4, TAU * 0.125f </summary>
        /// <returns> The direction snapped to the nearest diagonal. </returns>
        public static Vector2 ToDiagonalDirection(this Vector2 direction) => direction.ToSegmentedDirection(4, MathHelpers.TAU * 0.125f);

        /// <summary> Snaps the given direction to the nearest "segment". You can imagine a compass where each segment is an equally distributed area. </summary>
        /// <param name="direction"> The direction to snap. This is expected to be normalized as it is a direction. </param>
        /// <param name="segments"> The amount of possible directions to snap to. For instance, if you have 4, this will mean only orthogonal directions. </param>
        /// <param name="compassRotationRad"> The rotation of the "compass" in radians. Basically, this will rotate the possible angles (but you will still snap to the nearest direction). </param>
        /// <returns> The direction snapped to the nearest segmented direction. </returns>
        public static Vector2 ToSegmentedDirection(this Vector2 direction, int segments, float compassRotationRad = 0f) {
            if (direction.sqrMagnitude < Mathf.Epsilon) return Vector2.zero; // if direction is zero, return zero
            float angleRad = direction.DirectionToAngle() - compassRotationRad;
            float snappedAngle = Mathf.Round(angleRad / MathHelpers.TAU * segments) * MathHelpers.TAU / segments;
            return TrigonometryHelpers.AngleToDirection(snappedAngle + compassRotationRad);
        }
        
        #endregion
        #region Inverse Lerp / Remap / Bezier
        
        //TODO: understand this
        //TODO: 2D version
        /// <summary> Determines where a vector (<paramref name="t"/>) stands between two points (<paramref name="a"/> and <paramref name="b"/>). </summary>
        /// <param name="a">The start of the range.</param>
        /// <param name="b">The end of the range.</param>
        /// <param name="t">The point within the range you want to calculate.</param>
        /// <returns> A value between 0 and 1, representing where <paramref name="t"/> falls between <paramref name="a"/> (0) and <paramref name="b"/> (1). </returns>
        public static float InverseLerp (Vector3 a, Vector3 b, Vector3 t)
        {
            Vector3 ab = b - a;
            Vector3 av = t - a;
            return Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
        }


        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Vector3.Lerp(oMin, oMax, Vector3.InverseLerp(iMin, iMax, t));</c> </returns>
        public static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 t) => Vector2.Lerp(oMin, oMax, InverseLerp(iMin, iMax, t));
        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Vector3.Lerp(oMin, oMax, Vector3.InverseLerp(iMin, iMax, t));</c> </returns>
        public static Vector3 Remap(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 t) => Vector3.Lerp(oMin, oMax, InverseLerp(iMin, iMax, t));
        
        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Vector3.LerpUnclamped(oMin, oMax, Vector3.InverseLerp(iMin, iMax, t));</c> </returns>
        public static Vector2 RemapUnclamped(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 t) => Vector2.LerpUnclamped(oMin, oMax, InverseLerp(iMin, iMax, t));
        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Vector3.LerpUnclamped(oMin, oMax, Vector3.InverseLerp(iMin, iMax, t));</c> </returns>
        public static Vector3 RemapUnclamped(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 t) => Vector3.LerpUnclamped(oMin, oMax, InverseLerp(iMin, iMax, t));

        /// <returns> A bezier quadratic value. You can think of this as a lerp that outputs a Bézier curve (i.e. nested lerp between more than two points).
        /// <br/> Optimized version of <c>Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);</c></returns>
        public static Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t) {
            //return Vector2.Lerp(Vector2.Lerp(p0, p1, t), Vector2.Lerp(p1, p2, t), t);
            float u = 1f - t;
            return p0 * u * u
                 + p1 * 2f * u * t 
                 + p2 * t * t;
        }
        /// <returns> A bezier quadratic value. You can think of this as a lerp that outputs a Bézier curve (i.e. nested lerp between more than two points).
        /// <br/> Optimized version of <c>Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);</c></returns>
        public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
            //return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
            float u = 1f - t;
            return p0 * u * u
                 + p1 * 2f * u * t 
                 + p2 * t * t;
        }
        
        
        #endregion
        #region Rotate / Rotate 90 / Rotate Around Pivot
        
        //TODO: 2D version
        /// <returns> The point rotated in space around the pivot by the given angles. </returns>
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 eulerAngles) => RotateAroundPivot(point, pivot, Quaternion.Euler(eulerAngles));

        //TODO: 2D version
        /// <returns> The point rotated in space around the pivot by the given rotation. </returns>
        public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion rotation) => pivot + rotation * (point - pivot);
        

        /// <returns> The supplied vector, rotated by 90° counter-clockwise. </returns>
        /// <remarks> This is cheaper than any-angle rotating. Also, you can use this to calculate the normal of a 2D surface. </remarks>
        public static Vector2 Rotate90(this Vector2 vector) => new Vector2(-vector.y, vector.x);
        /// <returns> The supplied vector, rotated by 90° clockwise. </returns>
        /// <remarks> This is cheaper than any-angle rotating. </remarks>
        public static Vector2 Rotate90Clockwise(this Vector2 vector) => new Vector2(vector.y, -vector.x);

        
        /// <returns> The vector, rotated counter-clockwise by the given angle. </returns>
        /// <remarks> <c>vector.Rotate(TrigonometryHelpers.AngleToDirection(angleRad))</c> </remarks>
        public static Vector2 Rotate(this Vector2 vector, float angleRad) => vector.Rotate(TrigonometryHelpers.AngleToDirection(angleRad));
        /// <returns> The vector, rotated counter-clockwise by the given direction angle. </returns>
        public static Vector2 Rotate(this Vector2 vector, Vector2 angleVector) {
            return new Vector2(
                (angleVector.x * vector.x) - (angleVector.y * vector.y),
                (angleVector.y * vector.x) + (angleVector.x * vector.y)
            ); 
            
            //OLD CODE, basically the above is the same thing, but more optimize since we don't need to construct a matrix
            //also, Matrix2x2 I had was removed because it was a no-license script on GitHub
            //Vector2 vector90 = vector.Rotate90();
            //Matrix2x2 vectorToWorldSpace = new Matrix2x2(vector.x, vector.y, vector90.x, vector90.y); 
            //return vectorToWorldSpace * angleVector;
        }
        
        #endregion
        #region  Barycentric

        //TODO: I renamed this to specifically "Triangle" barycentric, but I have no clue if that's accurate lol...
        //sourced from https://discussions.unity.com/t/raycasthit-texturecoord-does-the-reverse-exist/36255/3
        /// <returns> The barycentric point (i.e. center of mass) from the given inputs.  </returns>
        public static Vector3 GetTriangleBarycentric (Vector2 v1,Vector2 v2,Vector2 v3,Vector2 p)
        {
            Vector3 b = new Vector3();
            b.x = ((v2.y - v3.y)*(p.x-v3.x) + (v3.x - v2.x)*(p.y - v3.y)) /
                  ((v2.y-v3.y)*(v1.x-v3.x) + (v3.x-v2.x)*(v1.y -v3.y));
            b.y = ((v3.y - v1.y)*(p.x-v3.x) + (v1.x - v3.x)*(p.y - v3.y)) /
                  ((v3.y-v1.y)*(v2.x-v3.x) + (v1.x-v3.x)*(v2.y -v3.y));
            b.z = 1 - b.x - b.y;
            return b;
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

        #endregion
    }
}