using UnityEngine;

namespace Emericoude.Helpers
{
	public enum DeltaTimeScale
	{
		DeltaTime,
		DeltaTimeUnscaled,
		DeltaTimeUnscaledExceptPause,
		FixedDeltaTime,
		FixedDeltaTimeUnscaled,
		FixedDeltaTimeUnscaledExceptPause
	}

	public static class TimeHelpers
	{
		public static float GetDeltaTime(this DeltaTimeScale deltaTimeScale)
		{
			return deltaTimeScale switch
			{
				DeltaTimeScale.DeltaTime => Time.deltaTime,
				DeltaTimeScale.DeltaTimeUnscaled =>  Time.unscaledDeltaTime,
				DeltaTimeScale.DeltaTimeUnscaledExceptPause => Time.timeScale == 0 ? 0 : Time.unscaledDeltaTime,
				DeltaTimeScale.FixedDeltaTime => Time.fixedDeltaTime,
				DeltaTimeScale.FixedDeltaTimeUnscaled => Time.fixedUnscaledDeltaTime,
				DeltaTimeScale.FixedDeltaTimeUnscaledExceptPause => Time.timeScale == 0 ? 0 : Time.fixedUnscaledDeltaTime,
				_ => Time.deltaTime
			};
		}
	}
}
