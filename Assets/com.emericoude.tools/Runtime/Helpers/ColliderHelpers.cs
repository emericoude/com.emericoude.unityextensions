using UnityEngine;

namespace Emericoude.Helpers
{
	public static class ColliderHelpers
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

			Vector3 lossyScale = transform.lossyScale;
			
			if (capsuleCollider.direction == 0) //X
			{
				radius *= Mathf.Max(lossyScale.y, lossyScale.z);
				height *= lossyScale.x;

				if (height >= radius * 2)
				{
					Vector3 right = transform.right;
					point0 -= (right * (height / 2 - radius));
					point1 += (right * (height / 2 - radius));
				}
			}
			else if (capsuleCollider.direction == 1) //Y
			{
				radius *= Mathf.Max(lossyScale.x, lossyScale.z);
				height *= lossyScale.y;

				if (height >= radius * 2)
				{
					Vector3 up = transform.up;
					point0 -= (up * (height / 2 - radius));
					point1 += (up * (height / 2 - radius));
				}
			}
			else if (capsuleCollider.direction == 2) //Z
			{
				radius *= Mathf.Max(lossyScale.x, lossyScale.y);
				height *= lossyScale.z;

				if (height >= radius * 2)
				{
					Vector3 forward = transform.forward;
					point0 -= (forward * (height / 2 - radius));
					point1 += (forward * (height / 2 - radius));
				}
			}
		}
	}
}
