using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.Math
{
	public static class Vector3Extensions
	{
		/// <summary>Finds the largest component inside the vector; either x, y or z.</summary>
		/// <returns>The <paramref name="vector"/>'s X, Y or Z component, whichever is the largest.</returns>
		public static float LargestComponent (this Vector3 vector)
		{
			return Mathf.Max(vector.x, vector.y, vector.z);
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
	}
}
