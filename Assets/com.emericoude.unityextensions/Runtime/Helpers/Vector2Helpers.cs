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
	}
}
