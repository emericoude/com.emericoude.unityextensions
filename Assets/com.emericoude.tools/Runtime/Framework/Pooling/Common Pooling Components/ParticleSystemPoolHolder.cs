using UnityEngine;

namespace Emericoude.Framework
{
    public class ParticleSystemPoolHolder : PooledComponentsHolder<ParticleSystem>
    {
        protected override bool IsComponentReadyForRelease(ParticleSystem component) {
            return !component.IsAlive(false); //we don't check children because in this context we have already grabbed all children in Awake()
        }
    }
}