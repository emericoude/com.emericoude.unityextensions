using UnityEngine;

namespace Emericoude.Helpers
{
	public static class Vector3Helpers
	{
		/// <summary>Finds the largest component inside the vector; either x, y or z.</summary>
		/// <returns>The <paramref name="vector"/>'s X, Y or Z component, whichever is the largest.</returns>
		public static float LargestComponent (this Vector3 vector)
		{
			return Mathf.Max(vector.x, vector.y, vector.z);
		}
		
		/// <summary>Finds the smallest component inside the vector; either x, y or z.</summary>
		/// <returns>The <paramref name="vector"/>'s X, Y or Z component, whichever is the smallest.</returns>
		public static float SmallestComponent (this Vector3 vector)
		{
			return Mathf.Min(vector.x, vector.y, vector.z);
		}

		/// <summary>Finds the average of all three components.</summary>
		/// <returns>The average of <paramref name="vector"/>'s X, Y and Z components.</returns>
		public static float AverageComponents(this Vector3 vector)
		{
			return (vector.x + vector.y + vector.z) / 3.0f;
		}

		/// <summary> Returns a vector where all values have the absolute of 1.</summary>
		public static Vector3 Abs (this Vector3 vector)
		{
			return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
		}

		/// <summary>Determines where a vector (<paramref name="value"/>) stands between two points (<paramref name="a"/> and <paramref name="b"/>).</summary>
		/// <param name="a">The start of the range.</param>
		/// <param name="b">The end of the range.</param>
		/// <param name="value">The point within the range you want to calculate.</param>
		/// <returns>A value between 0 and 1, representing where <paramref name="value"/> falls between <paramref name="a"/> (0) and <paramref name="b"/> (1).</returns>
		public static float InverseLerp (Vector3 a, Vector3 b, Vector3 value)
		{
			Vector3 AB = b - a;
			Vector3 AV = value - a;
			return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
		}

        /// <summary> Slightly more efficient than <see cref="Vector3.Distance(Vector3, Vector3)"/>. </summary>
        /// <returns> The distance between point a and point b, squared. </returns>
        public static float DistanceSqr(this Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

		/// <summary> Checks if a vector is approximately the same as another. Using a tolerance value (default: 0.0001). </summary>
		/// <returns> True if the distance sqr is within the tolerance margin; otherwise false. </returns>
		public static bool Approximately(this Vector3 a, Vector3 b, float tolerance = 0.0001f)
		{
			return (a.DistanceSqr(b) <= tolerance);
		}
		
		/// <summary> Rotates a point in space around a pivot. </summary>
		/// <returns> The point rotated in space around the pivot by the given angles. </returns>
		public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 eulerAngles)
		{
			return RotateAroundPivot(point, pivot, Quaternion.Euler(eulerAngles));
		}

		/// <summary> Rotates a point in space around a pivot. </summary>
		/// <returns> The point rotated in space around the pivot by the given rotation. </returns>
		public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Quaternion rotation)
		{
			Vector3 direction = point - pivot;
			return pivot + rotation * direction;
		}
		
		/// <summary> Returns the nearest point from the given point to a list of points. </summary>
		/// <returns> The nearest vector2 to the given point. </returns>
		public static Vector3 GetNearestPoint(this Vector3 from, Vector3[] to) {
			float nearestDistance = float.PositiveInfinity;
			Vector3 nearestPoint = Vector3.zero;
			foreach (Vector3 point in to) {
				float distance = (from - point).sqrMagnitude;
				if (distance < nearestDistance) {
					nearestDistance = distance;
					nearestPoint = point;
				}
			}

			return nearestPoint;
		}
		
		/// <returns> Basically, if there is a sphere around the to point, the nearest point on the surface of that sphere. </returns>
		public static Vector3 GetNearestPointOnSphereSurface(this Vector3 from, Vector3 to, float radius) {
			float distance = Vector3.Distance(from, to) - radius;
			Vector3 direction = (to - from).normalized;
			return from + direction * distance;
		}
		
		//I renamed this to specifically "Triangle" barycentric, but I have no clue if that's accurate lol...
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
