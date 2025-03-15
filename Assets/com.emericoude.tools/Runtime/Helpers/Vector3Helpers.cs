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
    }
}
