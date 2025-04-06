using UnityEngine;

namespace Emericoude.Physics
{
    [RequireComponent(typeof(CollisionContactEvents))]
    public abstract class CollisionContactEventListener : MonoBehaviour
    {
        [Header("Collision Contact Events")]
        [SerializeField] private CollisionContactEvents contactEvents;

        private void Reset()
        {
            this.contactEvents = this.GetComponent<CollisionContactEvents>();
        }

        private void OnEnable()
        {
            this.contactEvents.onContactEnter.AddListener(this.OnCollisionContact);
        }

        private void OnDisable()
        {
            this.contactEvents.onContactEnter.RemoveListener(this.OnCollisionContact);
        }

        protected abstract void OnCollisionContact(Collision collision);
    }
}