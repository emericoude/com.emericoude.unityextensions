using UnityEngine;

namespace Emericoude.Helpers
{
	public enum DeltaTimeScale
	{
		DeltaTime,
		DeltaTimeUnscaled,
		FixedDeltaTime,
		FixedDeltaTimeUnscaled
	}

	public static class TimeHelpers
	{
		public static float GetDeltaTime(this DeltaTimeScale deltaTimeScale)
		{
			return deltaTimeScale switch
			{
				DeltaTimeScale.DeltaTime => Time.deltaTime,
				DeltaTimeScale.DeltaTimeUnscaled => Time.unscaledDeltaTime,
				DeltaTimeScale.FixedDeltaTime => Time.fixedDeltaTime,
				DeltaTimeScale.FixedDeltaTimeUnscaled => Time.fixedUnscaledDeltaTime,
				_ => Time.deltaTime
			};
		}
	}
}
