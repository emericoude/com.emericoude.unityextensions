using System.Linq;
using UnityEngine;
using Emericoude.Gameplay;

namespace Emericoude.Math
{
	public static class CurveExtensions
	{
		/// <summary> Moves <paramref name="time"/> along towards the <paramref name="curve"/>'s end using <paramref name="deltaTime"/>. </summary>
		/// <param name="curve"> The curve used for evaluation. </param>
		/// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
		/// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
		/// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
		public static float AutoEvaluate(this AnimationCurve curve, ref float time, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime)
		{
			float maxTime = curve.keys.LastOrDefault().time;
			time = Mathf.MoveTowards(time, maxTime, deltaTime.GetDeltaTime());
			return curve.Evaluate(time);
		}

		/// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, towards <paramref name="targetTime"/> using <paramref name="deltaTime"/>. </summary>
		/// <param name="curve"> The curve used for evaluation. </param>
		/// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
		/// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
		/// <param name="targetTime"> The target time. This is clamped in the curve's minimum and maximum duration. </param>
		/// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
		public static float AutoEvaluateTowards (this AnimationCurve curve, ref float time, float targetTime, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime)
		{
			float minTime = curve.keys[0].time;
			float maxTime = curve.keys.LastOrDefault().time;

			targetTime = Mathf.Clamp(targetTime, minTime, maxTime);
			time = Mathf.MoveTowards(time, targetTime, deltaTime.GetDeltaTime());

			return curve.Evaluate(time);
		}

		/// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, towards <paramref name="targetTimeRelative"/> (between 0 and 1) using <paramref name="deltaTime"/>. </summary>
		/// <param name="curve"> The curve used for evaluation. </param>
		/// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
		/// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
		/// <param name="targetTimeRelative"> The target time. This is clamped between 0 and 1. </param>
		/// <remarks> This differs from <see cref="AutoEvaluateTowards(AnimationCurve, ref float, float, DeltaTimeScale)"/> in that the target time is a relative point in the curve, 0 is the beginning, 1 is the end. </remarks>
		/// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
		public static float AutoEvaluateTowardsRelative (this AnimationCurve curve, ref float time, float targetTimeRelative, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime)
		{
			float minTime = curve.keys[0].time;
			float maxTime = curve.keys.LastOrDefault().time;

			targetTimeRelative = Mathf.Lerp(minTime, maxTime, targetTimeRelative);
			time = Mathf.MoveTowards(time, targetTimeRelative, deltaTime.GetDeltaTime());

			return curve.Evaluate(time);
		}

		/// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, in a back and forth fashion using <paramref name="deltaTime"/>. </summary>
		/// <param name="curve"> The curve used for evaluation. </param>
		/// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
		/// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
		/// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
		public static float AutoEvaluatePingPong(this AnimationCurve curve, ref float time, ref bool forward, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime)
		{
			float minTime = curve.keys[0].time;
			float maxTime = curve.keys.LastOrDefault().time;
			
			float targetTime = forward ? maxTime : minTime;
			time = Mathf.MoveTowards(time, targetTime, deltaTime.GetDeltaTime());

			return curve.Evaluate(time);
		}

		/// <summary> Scales a curve by the given factor, keeping the curve's shape. </summary>
		/// <param name="curve"> The curve to scale. </param>
		/// <param name="timeScalingFactor"> The horizontal (or time) scaling factor. </param>
		/// <param name="valueScalingFactor"> The vertical (or value) scaling factor. </param>
		/// <returns> A new animation curve scaled by the given factor. </returns>
		public static AnimationCurve Scale (this AnimationCurve curve, float timeScalingFactor, float valueScalingFactor)
		{
			AnimationCurve scaledCurve = new AnimationCurve();
			for (int i = 0; i < curve.keys.Length; i++)
			{
				Keyframe keyframe = curve.keys[i];
				keyframe.value = curve.keys[i].value * valueScalingFactor;
				keyframe.time = curve.keys[i].time * timeScalingFactor;
				keyframe.inTangent = curve.keys[i].inTangent * valueScalingFactor / timeScalingFactor;
				keyframe.outTangent = curve.keys[i].outTangent * valueScalingFactor / timeScalingFactor;

				scaledCurve.AddKey(keyframe);
			}

			return scaledCurve;
		}
	}
}
