using UnityEngine;
using UnityEngine.VFX;

namespace Emericoude.Framework
{
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Holds the object from returning to the pool until the visual effect's systems are all sleeping.")]
    #endif
    [RequireComponent(typeof(VisualEffect),typeof(PooledGameObjectHolderHandler))]
    public class VisualEffectPooledGameObjectHolder : PooledGameObjectHolder
    {
        private VisualEffect visualEffect;

        private void Awake()
        {
            this.visualEffect = this.GetComponent<VisualEffect>();
        }

        public override bool IsReadyForRelease()
        {
            return !this.visualEffect.HasAnySystemAwake();
        }
    }
}