using UnityEngine;

namespace Emericoude.Framework
{
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Holds the object from returning to the pool until the audio source is no longer playing.")]
    #endif
    [RequireComponent(typeof(AudioSource),typeof(PooledGameObjectHolderHandler))]
    public class AudioSourcePooledGameObjectHolder : PooledGameObjectHolder
    {
        private AudioSource audioSource;
        
        private void Awake()
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }

        public override bool IsReadyForRelease()
        {
            return !this.audioSource.isPlaying;
        }
    }
}