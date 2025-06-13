using UnityEngine;

namespace Emericoude.Framework
{
    public class AudioSourcePoolHolder : PooledComponentsHolder<AudioSource>
    {
        protected override bool IsComponentReadyForRelease(AudioSource component) {
            return !component.isPlaying || component.loop;
        }
    }
}