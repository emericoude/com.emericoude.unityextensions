using System;
using System.Collections.Generic;
using Emericoude.Attributes;
using Emericoude.Helpers;
using UnityEngine;

namespace Emericoude.Tests
{
	public class AnimatedGizmosTests : MonoBehaviour
	{
		[Header("Curve field")]
		[BetterCurveField]
		[SerializeField] AnimationCurve Curve;

		[Header("Sphere Cast")]
		public List<SphereCastParameters> sphereCastParameters = new List<SphereCastParameters>();

		private void OnEnable ()
		{
			AnimatedGizmos.Enabled = true;
		}

		private void OnDisable ()
		{
			AnimatedGizmos.Enabled = false;
		}

		private void OnDrawGizmos ()
		{
			foreach (var sphereCastParam in this.sphereCastParameters)
			{
				if (sphereCastParam.wired) GizmosHelpers.DrawWireSphereCast(transform.position, transform.localScale.LargestComponent(), sphereCastParam.direction, out RaycastHit hit, sphereCastParam.maxDistance);
				else GizmosHelpers.DrawSphereCast(transform.position, transform.localScale.LargestComponent(), sphereCastParam.direction, out RaycastHit hit, sphereCastParam.maxDistance);
			}
		}
	}

	[Serializable]
	public class SphereCastParameters
	{
		public bool wired = false;
		public Vector3 direction = Vector3.forward;
		public float maxDistance = 100.0f;
	}
}

