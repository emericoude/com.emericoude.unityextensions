using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.Math
{
	public static class Vector2Extensions
	{
		/// <summary> Returns a vector where all values have the absolute of 1.</summary>
		public static Vector2 Abs (this Vector2 vector)
		{
			return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
		}

		/// <summary>Finds the largest component inside the vector; either x, y or z.</summary>
		/// <returns>The <paramref name="vector"/>'s X or Y component, whichever is the largest.</returns>
		public static float LargestComponent (this Vector2 vector)
		{
			return Mathf.Max(vector.x, vector.y);
		}
	}
}
