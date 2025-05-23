using UnityEngine;
#if CYSHARP_ZLINQ
using ZLinq;
#endif

namespace Emericoude.Helpers
{
    public static class CurveHelpers
    {
        /// <summary> Gets the last keyframe in the array, or the default value if the array is empty. </summary>
        /// <returns> The last keyframe in the array, or the default value if the array is empty. </returns>
        public static Keyframe GetLastKeyframeOrDefault(this Keyframe[] keyframes) {
            #if CYSHARP_ZLINQ
            return keyframes.AsValueEnumerable().LastOrDefault();
            #else
			if (keyframes.Length <= 0) return (Keyframe)default;
			return keyframes[^1];
            #endif
        }

        /// <summary>
        /// Finds the time from the value given value using an iterative method O(log n).
        /// In most cases an accuracy of 16-32 is enough. If you have a very simple curve you can do with less.
        /// </summary>
        /// <remarks> Your curve should not have overlap in time or value. </remarks>
        /// <returns> A (possibly inaccurate) time in the curve that corresponds to the given curve value. </returns>
        //source: https://stackoverflow.com/questions/25527855/animationcurve-evaluate-get-time-by-value
        public static float GetTimeFromValue(this AnimationCurve curve, float value, int accuracy) {
            float startTime = curve.keys[0].time;
            float endTime = curve.keys[curve.length - 1].time;
            float nearestTime = startTime;
            float step = endTime - startTime;
            
            float startValue = curve.Evaluate(startTime);
            float endValue = curve.Evaluate(endTime);
            int valueDirection = startValue > endValue ? -1 : 1;
            
            for (int i = 0; i < accuracy; i++) {
                float valueAtNearestTime = curve.Evaluate(nearestTime);
                float distanceToValueAtNearestTime = Mathf.Abs(value - valueAtNearestTime);

                float timeToCompare = nearestTime + step;
                float valueAtTimeToCompare = curve.Evaluate(timeToCompare);
                float distanceToValueAtTimeToCompare = Mathf.Abs(value - valueAtTimeToCompare);

                if (distanceToValueAtTimeToCompare < distanceToValueAtNearestTime) {
                    nearestTime = timeToCompare;
                    valueAtNearestTime = valueAtTimeToCompare;
                }

                step = Mathf.Abs(step * 0.5f) * Mathf.Sign(value - valueAtNearestTime) * valueDirection;
            }

            return nearestTime;
        }

        /// <summary> Moves <paramref name="time"/> along towards the <paramref name="curve"/>'s end using <paramref name="deltaTime"/>. </summary>
        /// <param name="curve"> The curve used for evaluation. </param>
        /// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
        /// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
        /// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
        public static float AutoEvaluate(this AnimationCurve curve, ref float time, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime) {
            float maxTime = curve.keys.GetLastKeyframeOrDefault().time;
            time = Mathf.MoveTowards(time, maxTime, deltaTime.GetDeltaTime());
            return curve.Evaluate(time);
        }

        /// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, towards <paramref name="targetTime"/> using <paramref name="deltaTime"/>. </summary>
        /// <param name="curve"> The curve used for evaluation. </param>
        /// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
        /// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
        /// <param name="targetTime"> The target time. This is clamped in the curve's minimum and maximum duration. </param>
        /// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
        public static float AutoEvaluateTowards(this AnimationCurve curve, ref float time, float targetTime, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime) {
            float minTime = curve.keys[0].time;
            float maxTime = curve.keys.GetLastKeyframeOrDefault().time;

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
        public static float AutoEvaluateTowardsRelative(this AnimationCurve curve, ref float time, float targetTimeRelative, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime) {
            float minTime = curve.keys[0].time;
            float maxTime = curve.keys.GetLastKeyframeOrDefault().time;

            targetTimeRelative = Mathf.Lerp(minTime, maxTime, targetTimeRelative);
            time = Mathf.MoveTowards(time, targetTimeRelative, deltaTime.GetDeltaTime());

            return curve.Evaluate(time);
        }

        /// <summary> Moves <paramref name="time"/> along the <paramref name="curve"/>, in a back and forth fashion using <paramref name="deltaTime"/>. </summary>
        /// <param name="curve"> The curve used for evaluation. </param>
        /// <param name="time"> The curve's current time passed in reference. For you, this should likely be a variable in your script. </param>
        /// <param name="deltaTime"> The timescale used for moving the curve, <see cref="Time.deltaTime"/> by default. </param>
        /// <returns> A value between 0 and 1, representing where <paramref name="time"/> stands in the <paramref name="curve"/>, using <see cref="AnimationCurve.Evaluate(float)"/>. </returns>
        public static float AutoEvaluatePingPong(this AnimationCurve curve, ref float time, ref bool forward, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTime) {
            float minTime = curve.keys[0].time;
            float maxTime = curve.keys.GetLastKeyframeOrDefault().time;
            float targetTime = forward ? maxTime : minTime;

            if (Mathf.Approximately(time, targetTime)) {
                forward = !forward;
                targetTime = forward ? maxTime : minTime;
            }

            time = Mathf.MoveTowards(time, targetTime, deltaTime.GetDeltaTime());
            return curve.Evaluate(time);
        }

        /// <summary> Scales a curve by the given factor, keeping the curve's shape. </summary>
        /// <param name="curve"> The curve to scale. </param>
        /// <param name="timeScalingFactor"> The horizontal (or time) scaling factor. </param>
        /// <param name="valueScalingFactor"> The vertical (or value) scaling factor. </param>
        /// <returns> A new animation curve scaled by the given factor. </returns>
        public static AnimationCurve Scale(this AnimationCurve curve, float timeScalingFactor, float valueScalingFactor) {
            AnimationCurve scaledCurve = new AnimationCurve();
            for (int i = 0; i < curve.keys.Length; i++) {
                Keyframe keyframe = curve.keys[i];
                keyframe.time = curve.keys[i].time * timeScalingFactor;
                keyframe.value = curve.keys[i].value * valueScalingFactor;
                keyframe.inTangent = curve.keys[i].inTangent * valueScalingFactor / timeScalingFactor;
                keyframe.outTangent = curve.keys[i].outTangent * valueScalingFactor / timeScalingFactor;

                scaledCurve.AddKey(keyframe);
            }

            return scaledCurve;
        }

        /// <summary> Moves a key to the givne time and value, but tries to retain the general shape of the tangents. </summary>
        /// <param name="curve"> The target curve. </param>
        /// <param name="index"> The key index to target. </param>
        /// <param name="time"> The new time (x-axis) of the key. </param>
        /// <param name="value"> The new value (y-axis) of the key. </param>
        /// <returns> The new keyframe, modified. </returns>
        public static Keyframe MoveKey(this AnimationCurve curve, int index, float time, float value) {
            var keyframe = curve.keys[index];
            var timeScalingFactor = keyframe.time == 0f ? 0f : time / keyframe.time;
            var valueScalingFactor = keyframe.value == 0f ? 0f : value / keyframe.value;
            keyframe.time = timeScalingFactor == 0f ? time : keyframe.time * timeScalingFactor;
            keyframe.value = valueScalingFactor == 0f ? value : keyframe.value * valueScalingFactor;
            if (timeScalingFactor != 0f) {
                keyframe.inTangent *= valueScalingFactor / timeScalingFactor;
                keyframe.outTangent *= valueScalingFactor / timeScalingFactor;
            }

            curve.MoveKey(index, keyframe);
            return keyframe;
        }
    }
}