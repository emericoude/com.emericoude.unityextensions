using System.Collections.Generic;
using Emericoude.Framework;
using UnityEngine;

namespace Emericoude
{
    /// <summary>
    /// ⚠️ Lazy Singleton. TimeManager is a class that allows you to manage time effects in your game.
    /// It will automatically tick effects, and resolve which timeScale value via a priority system (highest value wins, then lowest (aninmated) timeScale wins).
    /// </summary>
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Handles time effects. Use this to slow down, speed up, or freeze time in your game.")]
    #endif
    public class TimeManager : LazySingleton<TimeManager> {
        /*
         *  List of time effects
         *   Priority should be based an index field, then based on the strength of the effect (lowest time first)
         *   Each effect should have a duration, a strength, and a target time scale, lerp, etc
         *   - Freeze frames
         *   - Slow motion
         */

        public const float DEFAULT_TIME_SCALE = 1.0f;
        public static bool AdjustFixedDeltaTimeOnSlowDown = true;

        private float defaultFixedDeltaTime;
        private readonly List<TimeEffect> noNameTimeEffects = new();
        private readonly Dictionary<string, TimeEffect> namedTimeEffects = new();

        private void Start()
        {
            this.defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Update() {
            //resolve effects first, then tick them down
            this.ResolveTimeEffects();
            
            //tick regular effects
            for (int i = this.noNameTimeEffects.Count - 1; i >= 0; i--) {
                var effect = this.noNameTimeEffects[i];
                effect.TickTimer();
                
                if (!effect.Infinite && effect.Timer >= effect.Duration) {
                    this.noNameTimeEffects.RemoveAt(i);
                }
            }
            
            //tick named effects
            List<string> keysToRemove = new();
            foreach (var effect in this.namedTimeEffects) {
                effect.Value.TickTimer();
                if (!effect.Value.Infinite && effect.Value.Timer >= effect.Value.Duration) {
                    keysToRemove.Add(effect.Key);
                }
            }
            
            foreach (string key in keysToRemove) {
                this.namedTimeEffects.Remove(key);
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = DEFAULT_TIME_SCALE;
            Time.fixedDeltaTime = this.defaultFixedDeltaTime;
        }

        private void ResolveTimeEffects() {
            if (this.noNameTimeEffects.Count == 0 && this.namedTimeEffects.Count == 0) {
                Time.timeScale = DEFAULT_TIME_SCALE;
                return;
            }

            TimeEffect highestPriorityEffect = null;
            TryAssignHighestPriorityEffectFrom(ref highestPriorityEffect, this.noNameTimeEffects);
            TryAssignHighestPriorityEffectFrom(ref highestPriorityEffect, this.namedTimeEffects.Values);
            
            Time.timeScale = highestPriorityEffect.GetTimeScaleAnimated();
            if (AdjustFixedDeltaTimeOnSlowDown) {
                Time.fixedDeltaTime = this.defaultFixedDeltaTime * Time.timeScale;
            }
        }

        private static void TryAssignHighestPriorityEffectFrom(ref TimeEffect highestPriorityEffectRef, IEnumerable<TimeEffect> from)
        {
            foreach (var effect in from) {
                if (highestPriorityEffectRef == null) {
                    highestPriorityEffectRef = effect;
                    continue;
                }
                
                if (effect.Priority > highestPriorityEffectRef.Priority) {
                    highestPriorityEffectRef = effect;
                    continue;
                }
                
                if (effect.GetTimeScaleAnimated() < highestPriorityEffectRef.GetTimeScaleAnimated()) {
                    highestPriorityEffectRef = effect;
                }
            }
        }

        /// <summary> Starts a generic time effect. See also <see cref="StartTimeEffectWithUniqueName"/> for more control. </summary>
        /// <remarks> This makes a copy of the effect. </remarks>
        public void StartTimeEffect(TimeEffect effect) {
            if (effect.Infinite) Debug.LogWarning("You are starting an infinite effect with no way to stop it. Start it with 'StartEffectWithUniqueName' instead.", this);
            this.noNameTimeEffects.Add(new TimeEffect(effect));
        }
        
        /// <summary> Adds a time effect linked to a unique name. You can use <see cref="StopTimeEffectWithName"/> to manually remove the effect. Useful for infinite effects that stop at a specific trigger point. </summary>
        /// <remarks> This makes a copy of the effect. </remarks>
        /// <returns> The string key, modified if there was a duplicate and overwrite was set to false. </returns>
        public string StartTimeEffectWithUniqueName(TimeEffect effect, string key, bool overwrite = false) {
            while (this.namedTimeEffects.ContainsKey(key)) {
                if (overwrite) {
                    this.namedTimeEffects[key] = new TimeEffect(effect);
                    return key;
                }
                
                key += "_";
            }
            
            this.namedTimeEffects.Add(key, new TimeEffect(effect));
            return key;
        }
        
        /// <summary> Removes the effect with the given name instantly. Use the key returned by <see cref="StartTimeEffectWithUniqueName"/>, as it may modify the value you feed if there are duplicate. </summary>
        public void StopTimeEffectWithName(string key) {
            this.namedTimeEffects.Remove(key);
        }

        /// <summary> Converts the effect with the given name to a duration-based effect that animates back to normal speed. </summary>
        /// <remarks> This expects an animation curve that starts at a value of 1, and ends at a value of 0, where 'value' is the strength of the effect. </remarks>
        public void StopTimeEffectWithName(string key, AnimationCurve exitCurve)
        {
            if (this.namedTimeEffects.TryGetValue(key, out var effect))
            {
                effect.MarkForRemoval(exitCurve);
            }
        }
    }
}
