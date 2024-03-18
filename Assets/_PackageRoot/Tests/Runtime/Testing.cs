using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Emeric.Utilities.Gizmos;

public class Testing : MonoBehaviour
{
	public Color color = Color.red;
	public CapsuleCollider capsuleCollider;

	private void OnDrawGizmos ()
	{
		Gizmos.color = color;
		GizmosExtensions.DrawWireCapsule(capsuleCollider);
	}
}
