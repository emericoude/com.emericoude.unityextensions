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
		public static bool Enabled = false;

#if UNITY_EDITOR
		static AnimatedGizmos ()
		{
			EditorApplication.update += Update;
		}

		static void Update ()
		{
			if (!AnimatedGizmos.Enabled) return;
		}
#endif
	}
}
