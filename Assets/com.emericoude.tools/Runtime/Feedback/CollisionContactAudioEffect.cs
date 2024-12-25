using System.Collections;
using Emericoude.Attributes;
using Emericoude.Physics;
using UnityEngine;

namespace Emericoude.Feedback
{
    public class CollisionContactAudioEffect : CollisionContactEventListener
    {
        [Header("SFX Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip; //TODO: Use AudioResource API once available...
        [BetterCurveField("Velocity Magnitude", "Volume")]
        [SerializeField] private AnimationCurve velocityToVolumeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float cooldownDuration = 0.1f;

        private bool isOnCooldown = false;

        protected override void OnCollisionContact(Collision collision)
        {
            if (this.isOnCooldown) return;

            var volume = this.velocityToVolumeCurve.Evaluate(collision.relativeVelocity.magnitude);
            if (volume <= 0f) return;
            
            this.audioSource.PlayOneShot(this.audioClip);
            if (this.cooldownDuration > 0f)
            {
                this.StartCoroutine(this.Cooldown());
            }
        }

        private IEnumerator Cooldown()
        {
            this.isOnCooldown = true;
            yield return new WaitForSeconds(this.cooldownDuration);
            this.isOnCooldown = false;
        }
    }
}
