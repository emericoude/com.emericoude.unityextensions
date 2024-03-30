using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emeric.Utilities.Physics
{
	public static class PhysicsExtensions
	{
		/// <summary>Checks whether the <paramref name="sourceCollider"/> is in contact with the <paramref name="targetCollider"/>. The <see cref="QueryTriggerInteraction"/> and <see cref="LayerMask"/> are defined based on the target.</summary>
		/// <remarks>The only supported colliders are <see cref="BoxCollider"/>, <see cref="CapsuleCollider"/> and <see cref="SphereCollider"/>. <see cref="MeshCollider"/> is not supported.</remarks>
		/// <param name="sourceCollider">The collider to source.</param>
		/// <param name="targetCollider">The collider to look for.</param>
		/// <param name="maxAmountForNonAlloc">Default is 8. This uses "NonAlloc" version of <see cref="Physics.OverlapBoxNonAlloc(Vector3, Vector3, Collider[])"/> functions, you must specify a number of allocation possible if you expect a higher amount of possible hits.</param>
		/// <returns><see langword="false"/> if <paramref name="sourceCollider"/> is a <see cref="MeshCollider"/>; otherwise <see langword="true"/> if <paramref name="sourceCollider"/> touches <paramref name="targetCollider"/>;  otherwise <see langword="false"/>.</returns>
		public static bool IsInContactWith (this Collider sourceCollider, Collider targetCollider, int maxAmountForNonAlloc = 8)
		{
			int layerMask = (1 << targetCollider.gameObject.layer);
			QueryTriggerInteraction triggerInteraction = targetCollider.isTrigger ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

			Collider[] hits = new Collider[maxAmountForNonAlloc];
			int hitAmount = -1;
			if (sourceCollider is BoxCollider boxCollider)
			{
				hitAmount = UnityEngine.Physics.OverlapBoxNonAlloc(boxCollider.bounds.center, boxCollider.size / 2, hits, boxCollider.transform.rotation, layerMask, triggerInteraction);
			}
			else if (sourceCollider is CapsuleCollider capsuleCollider)
			{
				capsuleCollider.GetParametersForOverlap(out Vector3 point0, out Vector3 point1, out float radius);
				hitAmount = UnityEngine.Physics.OverlapCapsuleNonAlloc(point0, point1, radius, hits, layerMask, triggerInteraction);
			}
			else if (sourceCollider is SphereCollider sphereCollider)
			{
				hitAmount = UnityEngine.Physics.OverlapSphereNonAlloc(sphereCollider.bounds.center, sphereCollider.radius, hits, layerMask, triggerInteraction);
			}
#if UNITY_EDITOR || UNITY_DEVELOPMENT
			else
			{
				Debug.LogWarning($"{sourceCollider.gameObject.name}.{nameof(IsInContactWith)}({targetCollider.gameObject.name}): Collider of type {sourceCollider.GetType()} is not supported, this will always return false.");
				return false;
			}
#endif

			if (hitAmount > 0)
			{
				for (int i = 0; i < hitAmount; i++)
				{
					if (hits[i] == targetCollider) return true;
				}
			}

			return false;
		}
	}
}
