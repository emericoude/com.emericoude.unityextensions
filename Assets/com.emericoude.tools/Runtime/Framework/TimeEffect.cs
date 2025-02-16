using System;
using System.Linq;
using Emericoude.Attributes;
using Emericoude.Helpers;
using UnityEngine;

namespace Emericoude
{
    [Serializable]
    public class TimeEffect {
        public float Timer { get; private set; }
        public int Priority => this.priority;
        public float Duration => this.duration;
        public bool Infinite => this.infinite;

        [SerializeField] private int priority = 0;
        [SerializeField] private bool infinite = false;
        [SerializeField] private float duration = 0f;
        [SerializeField] private float timeScale = 0.5f;
        [SerializeField] private DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTimeUnscaledExceptPause;
        [BetterCurveField("Timer", "Time Scale Value")]
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        /// <summary> Use this constructor for effects you want to stop manually. </summary>
        public TimeEffect(float timeScale, AnimationCurve curve, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTimeUnscaledExceptPause, int priority = 0) {
            this.priority = priority;
            this.duration = -1;
            this.infinite = false;
            this.timeScale = timeScale;
            this.deltaTime = deltaTime;
            this.curve = curve;
        }

        /// <summary> Use this constructor for effects you want to stop automatically. </summary>
        public TimeEffect(float duration, float timeScale, AnimationCurve curve, DeltaTimeScale deltaTime = DeltaTimeScale.DeltaTimeUnscaledExceptPause, int priority = 0) {
            this.priority = priority;
            this.duration = -1;
            this.infinite = false;
            this.timeScale = timeScale;
            this.deltaTime = deltaTime;
            this.curve = curve;
        }
        
        public TimeEffect(TimeEffect other) {
            this.priority = other.priority;
            this.duration = other.duration;
            this.infinite = other.infinite;
            this.timeScale = other.timeScale;
            this.deltaTime = other.deltaTime;
            this.curve = other.curve;
        }

        public void TickTimer() { this.Timer += this.deltaTime.GetDeltaTime(); }
        public float GetTimeScaleAnimated() => Mathf.LerpUnclamped(1f, this.timeScale, this.curve.Evaluate(this.Timer));

        /// <summary> Effectively converts this effect to use a duration and lerp from its current animated state back to normal. </summary>
        /// <remarks> This expects a curve that goes from a value of 1 to 0f. Where the value of the curve represents the strength of the effect. </remarks>
        public void MarkForRemoval(AnimationCurve exitCurve) {
            this.curve = exitCurve;
            this.Timer = 0;
            this.duration = exitCurve.keys.Last().time;
            this.infinite = false;
            this.timeScale = this.GetTimeScaleAnimated();
        }
    }
}