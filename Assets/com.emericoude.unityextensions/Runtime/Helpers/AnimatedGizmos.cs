using System.Collections.Generic;
using Emericoude.Framework;
using UnityEngine;

namespace Emericoude.Helpers
{
	public class AnimatedGizmos : LazySingletonMonoBehaviour<AnimatedGizmos>
	{
		private static bool _enabled = true;
		public static bool Enabled 
		{ 
			get => _enabled && Application.isPlaying;
			set => _enabled = value;
		}

		/// <summary> Color to describe a positive feedback, such as a hit. Green with half transparency by default. </summary>
		public static Color PositiveFeedbackColor = Color.green.WithAlpha(0.5f);
		/// <summary> Color to describe a negative feedback, such as a miss. Red with half transparency by default. </summary>
		public static Color NegativeFeedbackColor = Color.red.WithAlpha(0.5f);

		private readonly Dictionary<float, AnimationInfo> _animationInfoByDuration = new Dictionary<float, AnimationInfo>();

		private class AnimationInfo {
			public float TargetTime;
			public float PreviousTime;
			public bool Pong = false;

			public AnimationInfo(float duration)
			{
				this.PreviousTime = Time.time;
				this.TargetTime = Time.time + duration;
			}
		}

		private void OnDrawGizmos ()
		{
			if (!AnimatedGizmos.Enabled) return;

			float currentTime = Time.time;
			foreach (var kvp in this._animationInfoByDuration)
			{
				if (!(kvp.Value.TargetTime <= currentTime)) continue;
				
				kvp.Value.PreviousTime = currentTime;
				kvp.Value.TargetTime = currentTime + kvp.Key;
				kvp.Value.Pong = !kvp.Value.Pong;
			}
		}

		public float GetAnimationTime(float duration = 1.0f, bool isPingPong = false)
		{
			duration = RoundToPrecision(duration);
			this.AddAnimationDurationIfMissing(duration);

			if (this._animationInfoByDuration.TryGetValue(duration, out var info))
			{
				if (isPingPong && info.Pong) return Mathf.InverseLerp(info.TargetTime, info.PreviousTime, Time.time);
				return Mathf.InverseLerp(info.PreviousTime, info.TargetTime, Time.time);
			}

			return 0.0f;
		}

		private void AddAnimationDurationIfMissing (float duration)
		{
			if (this._animationInfoByDuration.ContainsKey(duration)) return;
			this._animationInfoByDuration.Add(duration, new AnimationInfo(duration));
		}

		private static float RoundToPrecision(float duration)
		{
			return Mathf.Max(0.1f, Mathf.Round(duration * 10.0f) / 10.0f);
		}
	}
}
