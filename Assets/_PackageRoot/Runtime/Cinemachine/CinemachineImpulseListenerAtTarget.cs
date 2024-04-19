using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if CINEMACHINE
using Cinemachine;
#endif

namespace Emericoude.Cinemachine
{
#if CINEMACHINE
	/// <summary> Overrides the <seealso cref="CinemachineImpulseListener"/> to listen for impulses from the target's location. </summary>
	/// <remarks> See related thread <see href="https://forum.unity.com/threads/cinemachine-impulse-listener-on-player-object.539063/"></see> </remarks>
	[SaveDuringPlay]
	[AddComponentMenu("")] // Hide in menu
	[ExecuteAlways]
	public class CinemachineImpulseListenerAtTarget : CinemachineImpulseListener
	{
		/// <summary> The target from which to listen for impulses. </summary>
		[SerializeField] private Transform target;

		/// <summary> React to any detected impulses. </summary>
		/// <param name="vcam"> The virtual camera being processed. </param>
		/// <param name="stage"> The current pipeline stage. </param>
		/// <param name="state"> The current virtual camera state. </param>
		/// <param name="deltaTime"> The current applicable deltaTime. </param>
		/// <remarks> This is an override of <see cref="CinemachineImpulseListener"/> which modifies <see cref="CinemachineImpulseManager.GetImpulseAt(Vector3, bool, int, out Vector3, out Quaternion)"/> to use the <see cref="target"/>'s position. </remarks>
		protected override void PostPipelineStageCallback (CinemachineVirtualCameraBase vcam,
			 CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == m_ApplyAfter && deltaTime >= 0)
			{
				//this is copied from the base class, the only difference is target.position inside of GetImpulseAt.
				bool haveImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(target.position, m_Use2DDistance, m_ChannelMask, out var impulsePos, out var impulseRot);
				bool haveReaction = m_ReactionSettings.GetReaction(deltaTime, impulsePos, out var reactionPos, out var reactionRot);

				if (haveImpulse)
				{
					impulseRot = Quaternion.SlerpUnclamped(Quaternion.identity, impulseRot, m_Gain);
					impulsePos *= m_Gain;
				}

				if (haveReaction)
				{
					impulsePos += reactionPos;
					impulseRot *= reactionRot;
				}

				if (haveImpulse || haveReaction)
				{
					if (m_UseCameraSpace) impulsePos = state.RawOrientation * impulsePos;

					state.PositionCorrection += impulsePos;
					state.OrientationCorrection = state.OrientationCorrection * impulseRot;
				}
			}
		}
	}
#endif
}
