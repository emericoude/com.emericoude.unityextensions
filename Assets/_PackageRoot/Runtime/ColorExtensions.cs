using UnityEngine;

namespace Emericoude
{
	public static class ColorExtensions
	{
		/// <summary> Makes a new color from the given color with the provided alpha. Alpha should be between 0 and 1. </summary>
		/// <returns> The color with the provided alpha. </returns>
		public static Color WithAlpha (this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}
	}
}
