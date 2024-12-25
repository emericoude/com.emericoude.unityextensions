using UnityEngine;

#if CINEMACHINE
using Unity.Cinemachine;
#endif

namespace Emericoude.Cinemachine
{
#if CINEMACHINE_3
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
		protected override void PostPipelineStageCallback (CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage != ApplyAfter || !(deltaTime >= 0)) return;

			if (target == null)
			{
				Debug.LogWarning("Cinemachine Impulse Listener's target is null, reverting to camera.");
				target = vcam.transform;
			}
			
			//this is copied from the base class, the only difference is target.position inside of GetImpulseAt.
			bool hasImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(target.position, Use2DDistance, ChannelMask, out var impulsePos, out var impulseRot);
			bool hasReaction = ReactionSettings.GetReaction(deltaTime, impulsePos, out var reactionPos, out var reactionRot);

			if (hasImpulse)
			{
				impulseRot = Quaternion.SlerpUnclamped(Quaternion.identity, impulseRot, Gain);
				impulsePos *= Gain;
			}

			if (hasReaction)
			{
				impulsePos += reactionPos;
				impulseRot *= reactionRot;
			}

			if (hasImpulse || hasReaction)
			{
				if (UseCameraSpace) impulsePos = state.RawOrientation * impulsePos;

				state.PositionCorrection += impulsePos;
				state.OrientationCorrection = state.OrientationCorrection * impulseRot;
			}
		}
	}
#elif CINEMACHINE_2
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
		protected override void PostPipelineStageCallback (CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage != m_ApplyAfter || !(deltaTime >= 0)) return;
			
			//this is copied from the base class, the only difference is target.position inside of GetImpulseAt.
			bool hasImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(target.position, m_Use2DDistance, m_ChannelMask, out var impulsePos, out var impulseRot);
			bool hasReaction = m_ReactionSettings.GetReaction(deltaTime, impulsePos, out var reactionPos, out var reactionRot);

			if (hasImpulse)
			{
				impulseRot = Quaternion.SlerpUnclamped(Quaternion.identity, impulseRot, m_Gain);
				impulsePos *= m_Gain;
			}

			if (hasReaction)
			{
				impulsePos += reactionPos;
				impulseRot *= reactionRot;
			}

			if (hasImpulse || hasReaction)
			{
				if (m_UseCameraSpace) impulsePos = state.RawOrientation * impulsePos;

				state.PositionCorrection += impulsePos;
				state.OrientationCorrection = state.OrientationCorrection * impulseRot;
			}
		}
	}
#else
public class CinemachineImpulseListenerAtTarget : Monobehaviour { }
#endif
}
