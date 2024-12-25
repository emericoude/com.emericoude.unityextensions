using UnityEngine;

namespace Emericoude.Helpers
{
	public static class JointHelpers
	{
		/// <summary> Sets the joints motion and angular motions to the provided motion. </summary>
		public static void SetMotionGlobal (this ConfigurableJoint joint, ConfigurableJointMotion motion)
		{
			joint.xMotion = motion;
			joint.yMotion = motion;
			joint.zMotion = motion;

			joint.angularXMotion = motion;
			joint.angularYMotion = motion;
			joint.angularZMotion = motion;
		}
	}
}
