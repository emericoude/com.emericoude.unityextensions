using System.Collections;
using Emericoude.Framework;
using UnityEngine;
using UnityEngine.VFX;

namespace Emericoude.Framework
{
    /// <summary> A simple singleton object pool for vfx. Feed it <see cref="VisualEffect"/> prefabs and it will group them into individual pools. </summary>
    public class LazyVisualEffectPool : LazyObjectPool<LazyVisualEffectPool, string, VisualEffect>
    {
        private const float WAIT_FOR_EFFECT_TICK_RATE = 0.1f;
        private bool currentTargetShouldAutoPlay = false;

        /// <summary> Use this instead of GetOrCreate to make sure autoPlay is set for the next get. </summary>
        /// <param name="autoPlayAndAutoRelease"> If true, the effect's <see cref="VisualEffect.Play()"/> will be called,
        /// and a coroutine will check every 0.1s if the effect's <see cref="VisualEffect.HasAnySystemAwake"/> return false.
        /// Once no more systems are awake, the effect will be release. Note that, if set to true, there is no additional
        /// safety net for if you manually release the effect later implemented currently. </param>
        /// <returns> A visual effect from the correct object pool. </returns>
        public VisualEffect GetOrCreateEffect(VisualEffect prefab, bool autoPlayAndAutoRelease)
        {
            this.currentTargetShouldAutoPlay = autoPlayAndAutoRelease;
            return this.GetOrCreate(prefab);
        }
        
        /// <returns> The gameobject.name. DO NOT RENAME INSTANCES OF VISUAL EFFECTS. </returns>
        public override string GetObjectKey(VisualEffect prefab)
        {
            return prefab.gameObject.name;
        }

        protected override VisualEffect CreatePoolObject()
        {
            var visualEffectInstance = base.CreatePoolObject();
            visualEffectInstance.gameObject.name = this.CurrentKey;
            return visualEffectInstance;
        }

        protected override void GetPoolObject(VisualEffect effect)
        {
            effect.gameObject.SetActive(true);
            if (this.currentTargetShouldAutoPlay)
            {
                effect.Play();
                this.StartCoroutine(this.WaitForEffectToBeDone(effect));
            }
        }

        protected override void ReleasePoolObject(VisualEffect effect)
        {
            effect.Stop();
            effect.gameObject.SetActive(false);
        }

        protected override void DestroyPoolObject(VisualEffect effect)
        {
            base.DestroyPoolObject(effect);
        }

        private IEnumerator WaitForEffectToBeDone(VisualEffect effect)
        {
            yield return new WaitForSeconds(WAIT_FOR_EFFECT_TICK_RATE); //wait a frame just to be sure
            while (effect.HasAnySystemAwake()) yield return new WaitForSeconds(WAIT_FOR_EFFECT_TICK_RATE);
            this.ReleasePoolObject(effect);
        }
    }
}
