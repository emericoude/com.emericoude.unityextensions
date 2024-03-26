using System;
using System.Collections;
using System.Collections.Generic;
using Emeric.Utilities.Physics;

using UnityEngine;

namespace Emeric.Utilities.Gizmos
{
	public static class GizmosExtensions
	{
		/// <summary> Thickness used by various functions. Default is 0.0001f. </summary>
		public static float Thickness = 0.0001f;

		#region Draw Capsule

		/// <summary> Draws a "wired" capsule. </summary>
		/// <remarks> Rotation of the capsule is not accurate. </remarks>
		/// <param name="capsuleCollider"> The collider to source. </param>
		public static void DrawWireCapsule (CapsuleCollider capsuleCollider)
		{
			capsuleCollider.GetParametersForOverlap(out Vector3 point0, out Vector3 point1, out float radius);
			GizmosExtensions.DrawWireCapsule(point0, point1, radius);
		}

		/// <summary> Draws a "wired" capsule. </summary>
		/// <param name="point0"> The center of the sphere at the start of the capsule. </param>
		/// <param name="point1"> The center of the sphere at the end of the capsule. </param>
		/// <param name="radius"> The radius of the capsule. </param>
		/// <param name="cylinderResolution"> The resolution of the cylinder. In other words, the amount of faces to draw around the circumference. </param>
		public static void DrawWireCapsule (Vector3 point0, Vector3 point1, float radius)
		{
			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			//Calculate matrix
			Vector3 orientation = (point0 - point1).normalized;
			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, orientation);
			Vector3 center = Vector3.Lerp(point0, point1, 0.5f);
			Matrix4x4 matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
			UnityEngine.Gizmos.matrix = matrix;

			float height = Vector3.Distance(point0, point1);
			Vector3 point0InMatrix = Vector3.up * height / 2.0f;
			Vector3 point1InMatrix = Vector3.down * height / 2.0f;

			//Draw end points
			UnityEngine.Gizmos.DrawWireSphere(point0InMatrix, radius);
			UnityEngine.Gizmos.DrawWireSphere(point1InMatrix, radius);

			//Draw connection lines
			GizmosExtensions.DrawWireCylinder(center, orientation, radius, height, 16);

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		/// <summary> Draws a "solid" capsule. </summary>
		/// <param name="capsuleCollider"> The collider to source. </param>
		/// <param name="cylinderResolution"> The resolution of the cylinder. In other words, the amount of faces to draw around the circumference. </param>
		public static void DrawCapsule (CapsuleCollider capsuleCollider, int cylinderResolution = 128)
		{
			capsuleCollider.GetParametersForOverlap(out Vector3 point0, out Vector3 point1, out float radius);
			GizmosExtensions.DrawCapsule(point0, point1, radius, cylinderResolution);
		}

		/// <summary> Draws a "solid" capsule. </summary>
		/// <param name="point0"> The center of the sphere at the start of the capsule. </param>
		/// <param name="point1"> The center of the sphere at the end of the capsule. </param>
		/// <param name="radius"> The radius of the capsule. /param>
		/// <param name="cylinderResolution"> The resolution of the cylinder. In other words, the amount of faces to draw around the circumference. </param>
		public static void DrawCapsule (Vector3 point0, Vector3 point1, float radius, int cylinderResolution = 128)
		{
			Vector3 center = Vector3.Lerp(point0, point1, 0.5f);
			Vector3 orientation = (point1 - point0).normalized;
			float length = Vector3.Distance(point0, point1);

			UnityEngine.Gizmos.DrawSphere(point0, radius);
			UnityEngine.Gizmos.DrawSphere(point1, radius);
			GizmosExtensions.DrawCylinder(center, orientation, radius, length, cylinderResolution);
		}

		#endregion
		#region Draw Cylinder

		/// <summary> Draws a cylinder using faces. </summary>
		/// <param name="position"> The center of the cylinder. </param>
		/// <param name="orientation"> The orientation of the cylinder. </param>
		/// <param name="radius"> The radius of the cylinder. </param>
		/// <param name="height"> The height of the cylinder. </param>
		/// <param name="resolution"> The resolution of the cylinder. In other words, the amount of faces to draw around the circumference. </param>
		/// <param name="thickness"> The thickness of the cylinder's faces. </param>
		public static void DrawWireCylinder (Vector3 position, Vector3 orientation, float radius, float height, int resolution = 128)
		{
			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, orientation);
			float circumference = 2 * Mathf.PI * radius;
			float faceWidth = circumference / resolution;

			for (int i = 0; i < resolution; i++)
			{
				// Calculate the rotation matrix for the current segment
				float angle = 360f * i / resolution;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
				Vector3 capsulePosition = position + rotation * q * Vector3.forward * radius;
				Vector3 size = new Vector3(faceWidth, height, Thickness);
				Quaternion segmentRotation = rotation * q;
				Matrix4x4 matrix = Matrix4x4.TRS(capsulePosition, segmentRotation, Vector3.one);
				UnityEngine.Gizmos.matrix = matrix;

				//Draw face
				UnityEngine.Gizmos.DrawWireCube(Vector3.zero, size);
			}

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		/// <summary> Draws a cylinder using faces. </summary>
		/// <param name="position"> The center of the cylinder. </param>
		/// <param name="orientation"> The orientation of the cylinder. </param>
		/// <param name="radius"> The radius of the cylinder. </param>
		/// <param name="height"> The height of the cylinder. </param>
		/// <param name="resolution"> The resolution of the cylinder. In other words, the amount of faces to draw around the circumference. </param>
		/// <param name="thickness"> The thickness of the cylinder's faces. </param>
		public static void DrawCylinder (Vector3 position, Vector3 orientation, float radius, float height, int resolution = 128)
		{
			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			Quaternion rotation = Quaternion.FromToRotation(Vector3.up, orientation);
			float circumference = 2 * Mathf.PI * radius;
			float faceWidth = circumference / resolution;

			for (int i = 0; i < resolution; i++)
			{
				// Calculate the rotation matrix for the current segment
				float angle = 360f * i / resolution;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
				Vector3 capsulePosition = position + rotation * q * Vector3.forward * radius;
				Vector3 size = new Vector3(faceWidth, height, Thickness);
				Quaternion segmentRotation = rotation * q;
				Matrix4x4 matrix = Matrix4x4.TRS(capsulePosition, segmentRotation, Vector3.one);
				UnityEngine.Gizmos.matrix = matrix;

				//Draw face
				UnityEngine.Gizmos.DrawCube(Vector3.zero, size);
			}

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		#endregion
		#region Draw Collider

		/// <summary> Draws a shape based on the type of collider that is provided. </summary>
		/// <param name="collider"> The collider you want to draw. </param>
		public static void DrawCollider (Collider collider)
		{
			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			UnityEngine.Gizmos.matrix = collider.transform.localToWorldMatrix;

			if (collider is BoxCollider boxCollider)
			{
				UnityEngine.Gizmos.DrawCube(boxCollider.center, boxCollider.size);
			}
			else if (collider is SphereCollider sphereCollider)
			{
				UnityEngine.Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
			}
			else if (collider is CapsuleCollider capsuleCollider)
			{
				GizmosExtensions.DrawCapsule(capsuleCollider);
			}
			else if (collider is MeshCollider meshCollider)
			{
				UnityEngine.Gizmos.DrawMesh(meshCollider.sharedMesh, 0);
			}

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		/// <summary> Draws a wired shape based on the type of collider that is provided. </summary>
		/// <param name="collider"> The collider you want to draw. </param>
		public static void DrawWireCollider (Collider collider)
		{
			if (collider is MeshCollider) return;

			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			UnityEngine.Gizmos.matrix = collider.transform.localToWorldMatrix;

			if (collider is BoxCollider boxCollider)
			{
				UnityEngine.Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
			}
			else if (collider is SphereCollider sphereCollider)
			{
				UnityEngine.Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);
			}
			else if (collider is CapsuleCollider capsuleCollider)
			{
				GizmosExtensions.DrawWireCapsule(capsuleCollider);
			}
			else if (collider is MeshCollider meshCollider)
			{
				UnityEngine.Gizmos.DrawWireMesh(meshCollider.sharedMesh, 0);
			}

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		#endregion
		#region Double Sided

		/// <summary> Draws a hollow box at center with size. </summary>
		/// <remarks> This is useful when you want to see the box from inside, e.g. for a large killzone that englobes your level. <br/>
		/// This however draws 6 cubes rather than 1, so use it sparingly. </remarks>
		public static void DrawCubeDoubleSided (Vector3 center, Vector3 size, Quaternion rotation = default)
		{
			Matrix4x4 oldMatrix = UnityEngine.Gizmos.matrix;

			UnityEngine.Gizmos.matrix = Matrix4x4.TRS(center, rotation, size);
			Vector3 thicknessVector = UnityEngine.Gizmos.matrix.lossyScale * Thickness;
			UnityEngine.Gizmos.DrawCube(new Vector3(0.5f, 0f, 0f), new Vector3(thicknessVector.x, 1f, 1f));
			UnityEngine.Gizmos.DrawCube(new Vector3(-0.5f, 0f, 0f), new Vector3(thicknessVector.x, 1f, 1f));
			UnityEngine.Gizmos.DrawCube(new Vector3(0f, 0.5f, 0f), new Vector3(1f, thicknessVector.y, 1f));
			UnityEngine.Gizmos.DrawCube(new Vector3(0f, -0.5f, 0f), new Vector3(1f, thicknessVector.y, 1f));
			UnityEngine.Gizmos.DrawCube(new Vector3(0f, 0f, 0.5f), new Vector3(1f, 1f, thicknessVector.z));
			UnityEngine.Gizmos.DrawCube(new Vector3(0f, 0f, -0.5f), new Vector3(1f, 1f, thicknessVector.z));

			UnityEngine.Gizmos.matrix = oldMatrix;
		}

		#endregion
	}
}
