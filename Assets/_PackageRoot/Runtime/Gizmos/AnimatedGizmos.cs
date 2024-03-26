using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace Emeric.Utilities.Gizmos
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public static class AnimatedGizmos
	{
		/// <summary> Toggle this to turn animated gizmos on or off. </summary>
		public static bool Enabled = false;

		/// <summary> Color to describe a positive feedback, such as a hit. Green with half transparency by default. </summary>
		public static Color PositiveFeedbackColor = Color.green.WithAlpha(0.5f);
		/// <summary> Color to describe a negative feedback, such as a miss. Red with half transparency by default. </summary>
		public static Color NegativeFeedbackColor = Color.red.WithAlpha(0.5f);


#if UNITY_EDITOR
		static AnimatedGizmos ()
		{
			EditorApplication.update += Update;
		}

		private static void Update ()
		{
			if (!AnimatedGizmos.Enabled) return;
		}
#endif
	}
}
