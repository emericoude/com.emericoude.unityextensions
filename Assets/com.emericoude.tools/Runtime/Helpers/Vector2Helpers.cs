using UnityEngine;

namespace Emericoude.Helpers
{
	public static class Vector2Helpers
	{
		
		/// <summary> Returns a vector where all values have the absolute of 1.</summary>
		public static Vector2 Abs (this Vector2 vector)
		{
			return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}

		/// <summary>Finds the largest component inside the vector; either x or y.</summary>
		/// <returns>The <paramref name="vector"/>'s X or Y component, whichever is the largest.</returns>
		public static float LargestComponent (this Vector2 vector)
		{
			return Mathf.Max(vector.x, vector.y);
		}
		
		/// <summary>Finds the smallest component inside the vector; either x or y.</summary>
		/// <returns>The <paramref name="vector"/>'s X or Y component, whichever is the smallest.</returns>
		public static float SmallestComponent (this Vector2 vector)
		{
			return Mathf.Min(vector.x, vector.y);
		}

		/// <summary> Slightly more efficient than <see cref="Vector2.Distance(Vector2, Vector2)"/>. </summary>
		/// <returns> The distance between point a and point b, squared. </returns>
		public static float DistanceSqr(this Vector2 a, Vector2 b)
		{
			return (a - b).sqrMagnitude;
		}

		/// <summary> Returns the nearest point from the given point to a list of points. </summary>
		/// <returns> The nearest vector2 to the given point. </returns>
		public static Vector2 GetNearestPoint(this Vector2 from, Vector2[] toPoints) {
			float nearestDistance = float.PositiveInfinity;
			Vector2 nearestPoint = Vector2.zero;
			foreach (Vector2 to in toPoints) {
				float distance = (from - to).sqrMagnitude;
				if (distance < nearestDistance) {
					nearestDistance = distance;
					nearestPoint = to;
				}
			}

			return nearestPoint;
		}
		
		/// <returns> The nearest point to the other point, minus the radius. </returns>
		public static Vector2 GetNearestPointOnCircleEdge(this Vector2 from, Vector2 to, float radius) {
			float distance = Vector2.Distance(from, to) - radius;
			Vector2 direction = (to - from).normalized;
			return from + direction * distance;
		}

		/// <summary> Basically vector.SnapDirection(4). </summary>
		/// <returns> The current vector snapped to the nearest orthogonal. </returns>
		public static Vector2 ToNearestOrthogonal(this Vector2 vector) {
			return vector.SnapDirection(4);
		}

		/// <summary> Basically vector.SnapDirection(8). </summary>
		/// <returns> The current vector snapped to the nearest orthogonal or diagonal. </returns>
		public static Vector2 ToNearestOrthogonalOrDiagonal(this Vector2 vector) {
			return vector.SnapDirection(8);
		}
		
		/// <summary>
		/// Snaps the given direction to the nearest segmented direction.
		/// For instance, if you have 4 segments, it will be orthogonal only.
		/// If you have 8 segments, it will be orthogonal and diagonals.
		/// </summary>
		/// <returns> A clamped direction. </returns>
		public static Vector2 SnapDirection(this Vector2 direction, int segments) {
			if (direction.sqrMagnitude < Mathf.Epsilon) return Vector2.zero; // Handle zero vector case
			direction = direction.normalized;
			
			// optimized path for 4 segments (i.e. orthogonal)
			if (segments == 4) {
				return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) 
					? new Vector2(Mathf.Sign(direction.x), 0) 
					: new Vector2(0, Mathf.Sign(direction.y));
			}
    
			const float TAU = Mathf.PI * 2f;
			float angle = Mathf.Atan2(direction.y, direction.x);
			float snappedAngle = Mathf.Round(angle / TAU * segments) * TAU / segments;
    
			// Return the normalized direction vector without rounding the components
			return new Vector2(
				Mathf.Cos(snappedAngle),
				Mathf.Sin(snappedAngle)
			);
		}
		
		public static bool IsPerpendicularTo(this Vector2 a, Vector2 b) {
			return Mathf.Abs(Vector2.Dot(a, b)) < Mathf.Epsilon;
		}
	}
}
