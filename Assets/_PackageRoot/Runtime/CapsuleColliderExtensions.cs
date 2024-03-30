using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.UnityExtensions
{
	public static class CapsuleColliderExtensions
	{
		/// <summary>Outs the necessary information to use in a <see cref="Physics.OverlapCapsule(Vector3, Vector3, float)"/> from this <paramref name="capsuleCollider"/>.</summary>
		/// <param name="capsuleCollider">The <see cref="CapsuleCollider"/> to source.</param>
		/// <param name="point0">The center of the sphere at the start of the capsule.</param>
		/// <param name="point1">The center of the sphere at the end of the capsule.</param>
		/// <param name="radius">The radius of the capsule.</param>
		public static void GetParametersForOverlap (this CapsuleCollider capsuleCollider, out Vector3 point0, out Vector3 point1, out float radius)
		{
			Transform transform = capsuleCollider.transform;
			float height = capsuleCollider.height;

			point0 = point1 = capsuleCollider.bounds.center;
			radius = capsuleCollider.radius;

			if (capsuleCollider.direction == 0) //X
			{
				radius *= Mathf.Max(transform.lossyScale.y, transform.lossyScale.z);
				height *= transform.lossyScale.x;

				if (height >= radius * 2)
				{
					point0 -= (transform.right * (height / 2 - radius));
					point1 += (transform.right * (height / 2 - radius));
				}
			}
			else if (capsuleCollider.direction == 1) //Y
			{
				radius *= Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
				height *= transform.lossyScale.z;

				if (height >= radius * 2)
				{
					point0 -= (transform.up * (height / 2 - radius));
					point1 += (transform.up * (height / 2 - radius));
				}
			}
			else if (capsuleCollider.direction == 2) //Z
			{
				radius *= Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
				height *= transform.lossyScale.z;

				if (height >= radius * 2)
				{
					point0 -= (transform.forward * (height / 2 - radius));
					point1 += (transform.forward * (height / 2 - radius));
				}
			}
		}
	}
}
