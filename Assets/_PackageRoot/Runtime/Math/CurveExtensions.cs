using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emeric.Utilities.Math
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
			float maxTime = curve.keys.Last().time;
			time = Mathf.MoveTowards(time, maxTime, deltaTime.GetDeltaTime());
			return curve.Evaluate(time);
		}

		/// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, towards <paramref name="targetTime"/> using <paramref name="deltaTime"/>. </summary>
		/// <param name="curve"> The curve used for evaluation. </param>
		/// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
		/// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
		/// <param name="targetTime"> The target time. This is clamped in the curve's minimum and maximum duration. </param>
		/// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
		public static float AutoEvaluate (this AnimationCurve curve, ref float time, float targetTime, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime)
		{
			float minTime = curve.keys[0].time;
			float maxTime = curve.keys.Last().time;

			targetTime = Mathf.Clamp(targetTime, minTime, maxTime);
			time = Mathf.MoveTowards(time, targetTime, deltaTime.GetDeltaTime());

			return curve.Evaluate(time);
		}
	}
}
