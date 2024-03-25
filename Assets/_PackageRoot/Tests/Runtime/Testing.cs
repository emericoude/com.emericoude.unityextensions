using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Emeric.Utilities.Gizmos;

public class Testing : MonoBehaviour
{
	public Color color = Color.red;

	private void OnDrawGizmos ()
	{
		Gizmos.color = color;
		GizmosExtensions.DrawCubeDoubleSided(transform.position, transform.localScale, transform.rotation);
	}
}
