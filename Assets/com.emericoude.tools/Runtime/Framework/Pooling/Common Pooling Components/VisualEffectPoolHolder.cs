using UnityEngine.VFX;

namespace Emericoude.Framework
{
    public class VisualEffectPoolHolder : PooledComponentsHolder<VisualEffect>
    {
        protected override bool IsComponentReadyForRelease(VisualEffect component) {
            return !component.HasAnySystemAwake();
        }
    }
}