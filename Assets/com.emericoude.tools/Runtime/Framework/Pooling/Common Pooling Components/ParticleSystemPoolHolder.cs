using UnityEngine;

namespace Emericoude.Framework
{
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Holds the object from returning to the pool until the particle system is no longer playing.")]
    #endif
    [RequireComponent(typeof(ParticleSystem),typeof(PooledGameObjectHolderHandler))]
    public class ParticleSystemPooledGameObjectHolder : PooledGameObjectHolder
    {
        private new ParticleSystem particleSystem;

        private void Awake()
        {
            this.particleSystem = this.GetComponent<ParticleSystem>();
        }

        public override bool IsReadyForRelease()
        {
            return !this.particleSystem.isPlaying;
        }
    }
}