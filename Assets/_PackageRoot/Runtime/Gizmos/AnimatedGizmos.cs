using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace Emeric.Utilities.Gizmos
{

	//TODO explore singleton version, editor update is not useful.
	//TODO explore per-gizmos time, associate each to an identifier string object+component+identifier

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public static class AnimatedGizmos
	{
		/// <summary> Toggle this to turn animated gizmos on or off. </summary>
		public static bool Enabled = true;

		/// <summary> Color to describe a positive feedback, such as a hit. Green with half transparency by default. </summary>
		public static Color PositiveFeedbackColor = Color.green.WithAlpha(0.5f);
		/// <summary> Color to describe a negative feedback, such as a miss. Red with half transparency by default. </summary>
		public static Color NegativeFeedbackColor = Color.red.WithAlpha(0.5f);

		private static float targetTime;
		private static float lastRecordedTime;
		private static float animationDuration = 0.5f;
        private static float animationTime;

#if UNITY_EDITOR
        static AnimatedGizmos ()
		{
			EditorApplication.update += EditorUpdate;
		}

		private static void EditorUpdate ()
		{
			if (!AnimatedGizmos.Enabled) return;

			float timeSinceStartup = (float)EditorApplication.timeSinceStartup;
			if (timeSinceStartup > AnimatedGizmos.targetTime)
			{
				AnimatedGizmos.lastRecordedTime = timeSinceStartup;
				AnimatedGizmos.targetTime = timeSinceStartup + AnimatedGizmos.animationDuration;
			}

            AnimatedGizmos.animationTime = Mathf.InverseLerp(
				AnimatedGizmos.lastRecordedTime, 
				AnimatedGizmos.targetTime, 
				timeSinceStartup
			);
        }
#endif

		public static float GetAnimationTime()
		{
			return AnimatedGizmos.animationTime;
		}
	}
}
